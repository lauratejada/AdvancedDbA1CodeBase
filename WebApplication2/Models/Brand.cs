namespace WebApplication2.Models
{
    public class Brand
    {
        public int Id { get; set; }
        
        private string _name;
        
        public string Name { get => _name;
            set
            {
                if (string.IsNullOrEmpty(value) || value.Length < 2 )
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Brand name must be at least two characters in length.");
                }
                _name = value;
            }
        }

        public HashSet<Laptop> Laptops = new HashSet<Laptop>();
    }
}
