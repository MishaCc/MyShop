
namespace Shop.Models
{
    public class OrderProduct
    {
        public int Id { get; set; }
        public int ProductId{get;set;}
		public string UserId{get;set;}
		public string PostOffice{get;set;}
        public string Location { get; set;}
		public string Number {get;set;}
		public string? CardNumber{get;set;}
		public string PaymentMethod{get;set;}
    }
}
