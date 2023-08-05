using WebApplication2.Data;
using WebApplication2.Models;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<WebApplication2DbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("WebApplication2DbContextConnection"));
});

builder.Services.Configure<JsonOptions>(options => {
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    IServiceProvider serviceProvider = scope.ServiceProvider;

    await SeedData.Initialize(serviceProvider);
}

/*
 * The main endpoint, laptops/search
This endpoint will by default show all laptops in the database
It should have the following filters available to it, all of which are optional, and can be combined to work in any number:
Price above [amount]
Price below [amount]
Has stock greater than zero at store [store number] OR a stock greater than zero in any store in [province] 
Is in condition [LaptopCondition]
Belongs to brand [brandId]
Contains [searchPhrase] in the model name
*/
app.MapGet("/laptops/search", (WebApplication2DbContext db,
                                     decimal? priceAbove,
                                     decimal? priceBelow,
                                     Guid? storeNumber,
                                     string? province,
                                     LaptopCondition? condition,
                                     int? brandId,
                                     string? searchPhrase) => 
{
    try
    {
        // Price above [amount]
        if (priceAbove.HasValue)
        {
            if (!(priceAbove > 0))
            {
                throw new ArgumentOutOfRangeException(nameof(priceAbove));
            }
            
            return Results.Ok(db.Laptops
                            .Where(l => l.Price > priceAbove)
                            .Include(l => l.Brand)
                            .ToHashSet()); 
        }

        //Price below[amount]
        if (priceBelow.HasValue)
        {
            if (!(priceBelow > 0))
            {
                throw new ArgumentOutOfRangeException(nameof(priceBelow));
            }

            return Results.Ok(db.Laptops
                            .Where(l => l.Price < priceBelow)
                            .Include(l => l.Brand)
                            .ToHashSet());
        }

        // Has stock greater than zero at store [store number] OR a stock greater than zero in any store in [province] 
        if (storeNumber.HasValue)
        {
            return Results.Ok(db.StoresLaptops
                    .Where(sl =>
                    sl.StoreId == storeNumber
                    && sl.Quantity > 0)
                    .Include(sl => sl.Store)
                    .ToHashSet());
        }
        else if (!String.IsNullOrEmpty(province)) 
        {
            return Results.Ok(db.StoresLaptops
                    .Where(sl =>
                    sl.Store.Province.Equals(province)
                    && sl.Quantity > 0)
                    .Include(sl => sl.Store)
                    .ToHashSet());
        }
       
        //Is in condition [LaptopCondition]
        if (condition.HasValue)
        {
            return Results.Ok(db.Laptops
                 .Where(l => l.Condition == condition)
                 .Include(l => l.Brand)
                 .ToHashSet());
        }

        // Belongs to brand [brandId]
        if (brandId.HasValue)
        {
            return Results.Ok(db.Laptops
                .Where(l => l.BrandId == brandId)
                .Include(l => l.Brand)
                .ToHashSet());
        }

        //Contains [searchPhrase] in the model name
        if (!string.IsNullOrEmpty(searchPhrase))
        {
            return Results.Ok(db.Laptops
                .Where(l => l.Model.Contains(searchPhrase))
                .Include(l => l.Brand)
                .ToHashSet());
        }

        // default return
        return Results.Ok(db.Laptops
                      .Include(l => l.Brand)
                      .ToHashSet());
    }
    catch (InvalidOperationException ex)
    {
         return Results.NotFound(ex.Message);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

/*
 * An endpoint that shows all of the laptops available in a store with stores/{storeNumber}/inventory. Laptops with 0 or less quantity should not be shown.
 */
app.MapGet("/stores/{storeNumber}/inventory", (WebApplication2DbContext db, Guid storeNumber) =>
{
    try
    {
        return Results.Ok(db.StoresLaptops
                    .Where(ls => ls.Store.StoreNumber == storeNumber && ls.Quantity > 0)
                    .Include(ls => ls.Laptop)
                    .ToHashSet());
    }
    catch (ArgumentNullException ex)
    {
        return Results.NotFound(ex.Message);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

/*
An endpoint for posting a new Quantity for a Laptop at a specific store in stores/{storeNumber}/{laptopNumber}/changeQuantity?amount
*/
app.MapPost("stores/{storeNumber}/{laptopNumber}/changeQuantity", (WebApplication2DbContext db, Guid storeNumber, Guid laptopNumber, int amount) => 
{
    try 
    {
        StoreLaptop laptopFound = db.StoresLaptops
                                    .FirstOrDefault(sl => sl.LaptopId == laptopNumber
                                                            && sl.StoreId == storeNumber);

        if (laptopFound == null)
        {
            return Results.NotFound();
        }

        laptopFound.Quantity += amount;
        db.SaveChanges();

        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }

});

/*
An endpoint for getting the average price of all laptops among a specific brand, returned as { LaptopCount: [value], AveragePrice: [value]}
*/

app.MapGet("/laptops/{brandId}/averagePrice", (WebApplication2DbContext db, int brandId) =>
{
    try
    {
        HashSet<Laptop> LaptopsList = db.Laptops
                                        .Where(l => l.BrandId == brandId)
                                        .ToHashSet();

        if (LaptopsList.Count == 0)
        {
            return Results.NotFound();
        }

        return Results.Ok(new
        {
            LaptopCount = LaptopsList.Count,
            AveragePrice = LaptopsList.Average(l => l.Price)
        });
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});


/*
 * An endpoint which dynamically groups and returns all Stores according to the Province in which they are in. This endpoint should not display any data from any model other than the Stores queried, and should only display provinces that have stores in them.
 */

app.Run();
