namespace Shop.Models
{
    public class OrderProduct
    {
        public int Id { get; set; }
        public int ProductId{get;set;}
        public string UserSenderId { get; set; }
        public string UserRecipientId { get; set; }
        public string Location { get; set; }

    }
}
