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
            model = _productRepo.GetProducts(page);
            return View(model);
        }
        
        public IActionResult Privacy()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateProduct(List<string> Titels,List<string> TitelsId
            ,Product product, List<IFormFile> photos)
        {
           
            _productRepo.CreateProduct(product, Titels, TitelsId, photos);
            return Redirect("Index");
        }
        [HttpPost]
        public IActionResult CreateProductPage(DeviceType type,Category category)
        {
           
            dynamic model = new ExpandoObject();
            model.Category = category;
            model.Type = type;
            model.Pro = _context.Specification.ToList();
            return View(model);
        }
        public IActionResult TypePartial()
        {
            Product model = new Product();
            return PartialView(model);

        }
        public IActionResult ChooseCategory()
        {
            dynamic model = new ExpandoObject();
            model.Type = _context.DeviceType.ToList();
            model.Category = _context.Category.ToList();

            return View(model);
        }
        [HttpPost]
        public IActionResult Descrip(List<string> items)
        {
           
            return Redirect("ChooseCategory");
        }
 
        public IActionResult Basket(int page,string userId,int productId)
        {
           
            dynamic model = new ExpandoObject();
			model.Product = null;
            model.Product = _productRepo.RedirectToBasket(userId);
            return View(model);
        }
	    [HttpPost]
		public IActionResult DeleteProductFromBaket(int productId){
			
            _productRepo.DeleteProductFromBaket(productId);
            return Redirect("Basket");
		}
        [HttpPost]
        public IActionResult AddProduct(string UserId,int ProductId)
        {
			_context.Basket.FirstOrDefault(b=>b.ProductId==ProductId);
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
             _productRepo.MakeOrder(order, sum);
			return Redirect("Index");
		}
		public IActionResult OrderRequest(string UserId)
        {
            dynamic model = new ExpandoObject();
           model = _productRepo.OrderRequest(UserId);
		   return View(model);
		}
		public IActionResult UserOrders(string UserId)
        {
			dynamic model = new ExpandoObject();
			model = _productRepo.UserOrders(UserId);
            return View(model);
		}
		public IActionResult ProductPage(int productId){
			dynamic model = new ExpandoObject();
			model = _productRepo.ProductPage(productId);
            return View(model);
		}
		public IActionResult Characteristics(int productId){
			dynamic model = new ExpandoObject();
	         model = _productRepo.Characteristics(productId);
			 
            return View(model);
		}
		[HttpPost]
		public IActionResult CreateRequest(Requests request){
            request.PostTime = DateTime.Now;
            _context.Requests.Add(request);
            _context.SaveChanges();
			return Redirect($"Requests?productId={request.ProductId}");
		}
		public IActionResult Requests(int productId){
		    dynamic model = new ExpandoObject();
		    model = _productRepo.Requests(productId);
			return View(model);
		}
       public IActionResult Search(string request){
		   dynamic model = new ExpandoObject();
		   model=null;
           model = _productRepo.Search(request);
		   return View(model);
	   }
	   public IActionResult Catalog(){
		   var model = _context.Category.ToList();
		   return View(model);
	   }
	   [HttpPost]
	   public IActionResult AddPhoto(List<IFormFile> photos){
		   var category = _context.Category.ToList();
		   int i=0;
		   foreach(var item in category){
			    byte[] imageData = null;
                using (var binaryReader = new BinaryReader(photos[i].OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)photos[i].Length);
                }
                item.Img=imageData;
                _context.SaveChanges();
				i++;
		   }
		   return Redirect("Index");
	   }
	   [HttpPost]
	   public IActionResult CreateComplain(Complain complain){
		   _context.Complains.Add(complain);
		   _context.SaveChanges();
		   
		   return Redirect("Requests");
	   }
        public IActionResult ToComplete(int ProductId)
        {
            var result = _context.UserOreders.FirstOrDefault(o=>o.ProductId==ProductId);
			result.Status=true;
			_context.SaveChanges();
            return Redirect("Index");
        }
  
        public IActionResult CompletedoReders()
        {
            

            return View();
        }
  
       

            [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}