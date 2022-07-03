using Shop.Areas.Identity.Data;
using Shop.Models;




namespace Shop.Interfaces
{
    public interface IProductRepo
    {
        public List<Product> GetAll();
        public Product GetById(int id);
        public List<Product> GetByCategory(string categoryName);
        public void CreateProduct(Product model, List<string> Titels, List<string> TitelsId,
            List<IFormFile> photos);
        public void UpdateProduct(Product model);
        public void DeleteProduct(int id);
        public byte[] GetPhotoById(int id);
        public void AddProductInUserBasket(string UserId, int ProductId);
		public dynamic RedirectToBasket(string userId);
		public bool DetectUserProduct(string UserId,int productId);
    }
}
