

namespace Shop.Models
{
    public class Product
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public string? Name { get; set; }
        public int Amount { get; set; }
        public int Category { get; set; }
        public int TypeId { get; set; }

    }
}
