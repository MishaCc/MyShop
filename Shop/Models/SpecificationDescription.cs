namespace Shop.Models
{
    public class SpecificationDescription
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int ProductId { get; set; }
        public int TitleId { get; set; }
    }
}
