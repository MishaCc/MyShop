using Shop.Areas.Identity.Pages.Account;


namespace Shop.Models
{
    public class Basket
    { 
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; } 
        public int AmountProuduct { get; set; }
        
    }
}
