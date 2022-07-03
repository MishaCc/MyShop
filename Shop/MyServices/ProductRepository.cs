using Shop.Interfaces;
using Shop.Models;
using Shop.Areas.Identity.Data;

namespace Shop.MyServices
{
    public class ProductRepository : IProductRepo
    {
        private readonly ApplicationContext _context;
        public ProductRepository(ApplicationContext context)
        {
            _context = context;
        }
        public void CreateProduct(Product product, List<string> Titels, List<string> TitelsId,
            List<IFormFile> photos)
        {
            _context.Product.Add(product);
            _context.SaveChanges();
            for (int i = 0; i < Titels.Count; i++)
            {
                _context.Descriptions.Add(new SpecificationDescription()
                {
                    Title = Titels[i],
                    TitleId = int.Parse(TitelsId[i]),
                    ProductId = product.Id
                });
                _context.SaveChanges();
            }
            byte[] imageData = null;
            for (int i = 0; i < photos.Count; i++)
            {
                using (var binaryReader = new BinaryReader(photos[i].OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)photos[i].Length);
                }
                _context.Photo.Add(new Photo() { Img = imageData, ProductId = product.Id });
                _context.SaveChanges();
            }
        }

        public void DeleteProduct(int id)
        {
            throw new NotImplementedException();
        }

        public List<Product> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<Product> GetByCategory(string categoryName)
        {
            throw new NotImplementedException();
        }

        public Product GetById(int id)
        {
            Product product = _context.Product.FirstOrDefault(p => p.Id == id);
            if (product!=null)
            {
                return product;
            }
            else { throw new NotImplementedException(); }
        }
        public byte[] GetPhotoById(int id)
        {
           
            byte[] a=null;
            foreach (var item in _context.Photo.ToList())
            {
                if (id == item.ProductId)
                {
                    a = item.Img;
                }
                
            }
            
            return a;
        }
		
        public void AddProductInUserBasket(string userId,int productId)
        {
            Basket basket = new Basket()
            {
                UserId = userId,
                ProductId = productId,
                AmountProuduct=1
            };
            _context.Basket.Add(basket);
            _context.SaveChanges();
        }
		public dynamic RedirectToBasket(string userId){
			var result = _context.Basket.Where(a => a.UserId == userId).Join(_context.Product,
                p => p.ProductId,
                c => c.Id,
                (p, c) => new
                {
                    UserId = p.UserId,
                    Name = c.Name,
                    Id = c.Id,
                    Price = c.Price,
					ProductId=p.ProductId
                });
				return result;
		}
		public bool DetectUserProduct(string userId,int productId){
			var result = _context.PostedProducts.FirstOrDefault(p=>p.ProductID==productId);
			if(result==null){
				return false;
			}
			  if(result.UserId==userId){
				return true;
			   }else return false;
			
			
		}
        public void UpdateProduct(Product model)
        {
            throw new NotImplementedException();
        }
    }
}
