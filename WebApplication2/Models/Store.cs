using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Store
    {
        public Guid StoreNumber { get; set; }
        [Required]

        private string _streetNameAndNumber;
        public string StreetNameAndNumber
        {
            get => _streetNameAndNumber;
            set
            {
                if (string.IsNullOrEmpty(value) || value.Length < 3)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Street Name and Number name must be at least three characters in length.");
                }
                _streetNameAndNumber = value;
            }
        }

        public CanadianProvinces Province { get; set; }

        public HashSet<StoreLaptop> StoresLaptops { get; set; }

        public Store() 
        {
            this.StoresLaptops = new HashSet<StoreLaptop>(); 
        }
    }

    public enum CanadianProvinces
    {
        Alberta,
        [Description("British Columbia")] BritishColumbia, 
        Manitoba,
        [Description("New Brunswick")] NewBrunswick,
        [Description("Newfoundland and Labrador,")] NewfoundlandAndLabrador,
        [Description("Nova Scotia")] NovaScotia, 
        Ontario,
        [Description("Prince Edward Island")] PrinceEdwardIsland, 
        Quebec, 
        Saskatchewan
    }
}
