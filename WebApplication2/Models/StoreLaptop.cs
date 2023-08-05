namespace WebApplication2.Models
{
    public class StoreLaptop
    {
        public Guid StoreId { get; set; }
        public Store Store { get; set; }

        public Guid LaptopId { get; set; }
        public Laptop Laptop { get; set; }

        private int _quantity;
        public int Quantity { get => _quantity; 
            set
            {
                if (value.GetType().Equals(typeof(int)))
                {
                    _quantity = value;
                }
                else
                {
                    throw new ArgumentException("Price must be an integer.");
                }
            }
        } // for quantity stock 
    }
}
