namespace Shop.Models
{
    public class UserDeliveryRequest
    {
		public int Id{get;set;}
		public string UserId{get;set;}
		public int ProductId{get;set;}
        public string SendAdress{get;set;}
    }
}
