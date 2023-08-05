using WebApplication2.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Data
{
    public static class SeedData
    {
        public async static Task Initialize(IServiceProvider serviceProvider)
        {
            WebApplication2DbContext db = new WebApplication2DbContext(serviceProvider.GetRequiredService<DbContextOptions<WebApplication2DbContext>>());

            db.Database.EnsureDeleted();
            db.Database.Migrate();

            // Brand
            Brand brandOne = new Brand { Name = "Dell"};
            Brand brandTwo = new Brand { Name = "HP" };
            Brand brandThree = new Brand { Name = "Lenovo" };

            if (!db.Brands.Any())
            {
                db.Add(brandOne);
                db.Add(brandTwo);
                db.Add(brandThree);
                db.SaveChanges();
            }

            // Laptop
            Laptop laptopOne = new Laptop { Model = "XPS", Price = 1500, Brand = brandOne, BrandId = brandOne.Id, Condition = LaptopCondition.New };
            Laptop laptopTwo = new Laptop { Model = "Pavilion", Price = 1400, Brand = brandTwo, BrandId = brandTwo.Id, Condition = LaptopCondition.Rental };
            Laptop laptopThree = new Laptop { Model = "ThinkPad", Price = 1300, Brand = brandThree, BrandId = brandThree.Id, Condition = LaptopCondition.Refurbished };

            if (!db.Laptops.Any())
            {
                db.Add(laptopOne);
                db.Add(laptopTwo);
                db.Add(laptopThree);
                db.SaveChanges();
            }

            // Store
            Store firstStore = new Store { StreetNameAndNumber = "Address 100", Province = CanadianProvinces.Alberta};
            Store secondStore = new Store { StreetNameAndNumber = "Address 200", Province = CanadianProvinces.Manitoba };
            Store thirdStore = new Store { StreetNameAndNumber = "Address 300", Province = CanadianProvinces.Ontario };

            if (!db.Stores.Any())
            {
                db.Add(firstStore);
                db.Add(secondStore);
                db.Add(thirdStore);
                db.SaveChanges();
            }

            // StoreLaptop
            StoreLaptop storeLaptopOne = new StoreLaptop { Store = firstStore , Laptop = laptopOne , Quantity = 10 };
            StoreLaptop storeLaptopTwo = new StoreLaptop { Store = firstStore, Laptop = laptopTwo, Quantity = 20 };
            StoreLaptop storeLaptopThree = new StoreLaptop { Store = secondStore, Laptop = laptopThree, Quantity = 15 };

            if (!db.StoresLaptops.Any())
            {
                db.Add(storeLaptopOne);
                db.Add(storeLaptopTwo);
                db.Add(storeLaptopThree);
                db.SaveChanges();
            }

        }
    }
}