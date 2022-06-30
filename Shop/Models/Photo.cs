namespace Shop.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public int ProductId { get; set; }

        public byte[]? Img { get; set; }
    }
}
