using Shop.Interfaces;
using Shop.Models;
using Shop.Areas.Identity.Data;
using System.Dynamic;

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

        public dynamic GetProducts(int page)
        {
			dynamic model = new ExpandoObject();
            int pageSize = 15;
            model.Count = _context.Product.ToList().Count;
            model.Product = _context.Product.ToList().Skip((page - 1) * pageSize).Take(pageSize);
            model.Photo = _context.Photo.ToList();
			return model;
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
		public bool CheckBasket(string userId,int productId){
            var result = _context.Basket.FirstOrDefault(b=>b.ProductId==productId&&b.UserId
            ==userId);
            if (result == null)
            {
                return false;
            }
            else {
                return true; }
		}
        public void DeleteProductFromBaket(int productId){
			
			var basketObj = _context.Basket.FirstOrDefault(p => p.ProductId == productId);
            if (basketObj != null)
            {
                _context.Basket.Remove(basketObj);
                _context.SaveChanges();
            }
			
		}
        public void MakeOrder(OrderProduct order,float sum){
			
			var sender = _context.PostedProducts.FirstOrDefault(p => p.ProductID == order.ProductId);
            var product = _context.Product.FirstOrDefault(p=>p.Id==order.ProductId);
			product.Amount--;
            if (sender != null)
            {
                _context.UserDeliveryRequests.Add(new UserDeliveryRequest()
                {
                    UserId = sender.UserId,
                    ProductId = order.ProductId,
                    SendAdress = $"{order.Location},{order.PostOffice}"
                });
                _context.UserOreders.Add(new UserOreders()
                {
                    UserId = order.UserId,
                    ProductId = order.ProductId,
                    Status = false,
                    Sum = sum
                });
                _context.OrderProducts.Add(order);
                _context.SaveChanges();
            }
		}
		public dynamic OrderRequest(string UserId){
			
			var request =  _context.UserDeliveryRequests.FirstOrDefault(p => p.UserId == UserId);

            dynamic model = new ExpandoObject();
            if (request != null)
            {
                model = from d in _context.UserDeliveryRequests
                        join o in _context.OrderProducts on d.ProductId equals o.ProductId
                        join u in _context.Users on o.UserId equals u.Id
                        join p in _context.Product on o.ProductId equals p.Id
                        where d.UserId == UserId
                        select new
                        {
                            SendAdress = d.SendAdress,
                            PaymentMethod = o.PaymentMethod,
                            Number = o.Number,
                            UserName = u.UserName,
                            LastName = u.LastName,
                            ProductName = p.Name,
							ProductId = p.Id
                        };
            }
            else { model = null; }
			return model;
		}
		public dynamic UserOrders(string UserId){
			dynamic model = new ExpandoObject();
			var order = _context.OrderProducts.FirstOrDefault(p=>p.UserId==UserId);
            model = null;
			if(order!= null){
                model = from o in _context.OrderProducts
                    join p in _context.Product on o.ProductId equals p.Id
                    join po in _context.PostedProducts on o.ProductId equals po.ProductID
					join u in _context.Users on po.UserId equals u.Id where o.UserId==UserId 
                    select new
                    {
                        ProductName = p.Name,
                        Price = p.Price,
                        SendAdress = $"{o.Location},{o.PostOffice}",
                        Sender = $"{u.UserName},{u.LastName}",
                        Number = u.PhoneNumber,
						PaymentMethod = o.PaymentMethod,
						ProductId = p.Id
                    };

            }
			return model;
		}
		public dynamic ProductPage(int productId){
			
			dynamic model = new ExpandoObject();
			var result = _context.Product.FirstOrDefault(p=>p.Id==productId);
			if(result!=null){
             model.Photo = _context.Photo.Where(ph => ph.ProductId == productId);
             model.User = from pp in _context.PostedProducts
                         join u in _context.Users on pp.UserId equals u.Id
                         join p  in _context.Product on pp.ProductID equals p.Id
                         where pp.ProductID == productId
                         select new
                         {
                             Name = u.UserName,
                             Email = u.Email,
                             Number = u.PhoneNumber,
                             Price = p.Price
                         };
             model.Productid = productId;
			}else{model.User=null;}
			return model;
		}
		public dynamic Characteristics(int productId){
			dynamic model = new ExpandoObject();
			var result = _context.Product.FirstOrDefault(p=>p.Id==productId);
            model.ProductId = productId;
            model.Char = from d in _context.Descriptions
			        join s in _context.Specification on d.TitleId equals s.Id where d.ProductId==productId
					select new
					{
						Desc = d.Title,
						Property = s.Title
					};

            var same = from p in _context.Product
                         join ph in _context.Photo on p.Id equals ph.ProductId
                         where p.Price == result.Price
                         select new
                         {
                             Name = p.Name,
                             Img = ph.Img
                         };
            model.Same = same.ToList().Take(1);

            return model;
		}
		public dynamic Requests(int productId){
			dynamic model = new ExpandoObject();
			model.ProductId = productId;
		    model.Requests = from r in _context.Requests
			                 join u in _context.Users on r.UserId equals u.Id
							 where r.ProductId==productId
							 select new
							 {
								 Description = r.Description,
								 Adventages = r.Adventages,
								 DisAdventages = r.DisAdventages,
								 PostTime = r.PostTime,
								 Name = u.UserName,
								 Id=u.Id
							 };
		    return model;
		}
		public dynamic Search(string request){
		   dynamic model = new ExpandoObject();
           model=null;
		   var category = _context.Category.ToList().FirstOrDefault(c=>c.Name==request,null);
            if (category != null)
            {
                model = _context.Product.Where(p => p.Category == category.Id);
            }
            else
            {
                model = _context.Product.Where(p => p.Name == request);
            }
			return model;
		}
		public bool IsCompleted(int id){
			var result = _context.UserOreders.FirstOrDefault(o=>o.ProductId==id);
			if(result!=null){
				if(result.Status==true){
					return true;
				}else return false;
			}
		
			return false;
		}
        public void UpdateProduct(Product model)
        {
            throw new NotImplementedException();
        }
    }
}
