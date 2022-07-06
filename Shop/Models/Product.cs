using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        [MinLength(6,ErrorMessage ="Опишiть назву детальніше")]
        public string? Name { get; set; }
        [Required]
        public int Amount { get; set; }
        public int Category { get; set; }
        public int TypeId { get; set; }

    }
}
