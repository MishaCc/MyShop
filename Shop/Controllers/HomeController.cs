using Microsoft.AspNetCore.Mvc;
using Shop.Models;
using Shop.ModelHelpers;
using System.Diagnostics;
using Shop.Areas.Identity.Data;
using Shop.Paging;
using Shop.Interfaces;
using System.Dynamic;
using System.Linq;
using System.Collections.Generic;


namespace Shop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext _context;
        private readonly IProductRepo _productRepo;
        public HomeController(ILogger<HomeController> logger,ApplicationContext context
            ,IProductRepo productRepo)
        {
            _logger = logger;
            _context = context;
            _productRepo = productRepo;
            
        }

        public IActionResult Index(int page)
        {
            dynamic model = new ExpandoObject();
            int pageSize = 15;
            model.Count = _context.Product.ToList().Count;
            model.Product = _context.Product.ToList().Skip((page - 1) * pageSize).Take(pageSize);
            model.Photo = _context.Photo.ToList();
            
            return View(model);
        }
        
        public IActionResult Privacy()
        {
            return View();
        }
        
        public IActionResult CreateProduct(List<string> Titels,List<string> TitelsId
            ,Product product, List<IFormFile> photos)
        {
           
            _productRepo.CreateProduct(product, Titels, TitelsId, photos);
            return Redirect("Index");
        }
        [HttpPost]
        public IActionResult CreateProductPage(DeviceType type,Category category)
        {
            Console.WriteLine($"s=>{type.Id}\nC=>{category.Id}");
            dynamic model = new ExpandoObject();
            model.Category = category;
            model.Type = type;
            model.Pro = _context.Specification.ToList();
            return View(model);
        }
        public IActionResult TestProperty()
        {
            dynamic model = new ExpandoObject();
            model.Type = _context.DeviceType.ToList();
            model.Category = _context.Category.ToList();

            return View(model);
        }
        [HttpPost]
        public IActionResult Descrip(List<string> items)
        {
           
            return Redirect("TestProperty");
        }
 
        public IActionResult Basket(int page,string userId,int productId)
        {
           
            dynamic model = new ExpandoObject();
            model.Product = _productRepo.RedirectToBasket(userId);
            return View(model);
        }
	    [HttpPost]
		public IActionResult DeleteProductFromBaket(int productId){
			
            var basketObj = _context.Basket.FirstOrDefault(p => p.ProductId == productId);
            if (basketObj != null)
            {
                _context.Basket.Remove(basketObj);
                _context.SaveChanges();
            }
            return Redirect("Basket");
		}
        [HttpPost]
        public IActionResult AddProduct(string UserId,int ProductId)
        {
            Console.WriteLine($"{UserId}\n{ProductId}");
            _productRepo.AddProductInUserBasket(UserId, ProductId);
            return Redirect("Index");
        }
		 public IActionResult СheckoutPage(int ProductId)
        {
            dynamic model = new ExpandoObject();
			model.Product = _context.Product.FirstOrDefault(p => p.Id == ProductId);
            return View(model);
        }
	     [HttpPost]
		 public IActionResult MakeOrder(OrderProduct order,float sum)
        {
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
			return Redirect("Index");
		}
		public IActionResult OrderRequest(string UserId)
        {
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
                            ProductName = p.Name
                        };
            }
            else { model = null; }



                return View(model);
		}
		public IActionResult UserOrders(string UserId)
        {
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
						PaymentMethod = o.PaymentMethod
                    };
                Console.WriteLine("asdsadad");
            }
            return View(model);
		}
		public IActionResult ProductPage(int productId){
			dynamic model = new ExpandoObject();
			model.ProductInfo = from p in _context.Product 
			join d in _context.Descriptions on p.Id equals d.ProductId
			join s in _context.Specification on d.TitleId equals s.Id where p.Id==productId
			select new
			{
				Name = p.Name,
				Price = p.Price,
				TiteleS = s.Title,
				TitleD = d.Title
			};
            model.Photo = _context.Photo.Where(ph => ph.ProductId == productId);
            
			return View(model);
		}

            [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}