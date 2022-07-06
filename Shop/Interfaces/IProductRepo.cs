using Shop.Areas.Identity.Data;
using Shop.Models;




namespace Shop.Interfaces
{
    public interface IProductRepo
    {
        public dynamic GetProducts(int page);
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
		public bool CheckBasket(string UserId,int productId);
		public void DeleteProductFromBaket(int productId);
		public void MakeOrder(OrderProduct order,float sum);
		public dynamic OrderRequest(string UserId);
		public dynamic UserOrders(string UserId);
		public dynamic ProductPage(int productId);
		public dynamic Characteristics(int productId);
		public dynamic Requests(int productId);
		public dynamic Search(string request);
		public bool IsCompleted(int id);
    }
}
