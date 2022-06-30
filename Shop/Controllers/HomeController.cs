using Microsoft.AspNetCore.Mvc;
using Shop.Models;
using Shop.ModelHelpers;
using System.Diagnostics;
using Shop.Areas.Identity.Data;
using Shop.Paging;
using Shop.Interfaces;
using System.Dynamic;
using System.Linq;

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
 
        public IActionResult Basket(int page,string userId)
        {
            dynamic model = new ExpandoObject();
            model.Product = _context.Basket.Where(a => a.UserId == userId).Join(_context.Product,
                p => p.ProductId,
                c => c.Id,
                (p, c) => new
                {
                    UserId = p.UserId,
                    Name = c.Name,
                    Id = c.Id,
                    Price = c.Price
                });
            return View(model);
        }
        [HttpPost]
        public IActionResult AddProduct(string UserId,int ProductId)
        {
            Console.WriteLine($"{UserId}\n{ProductId}");
            _productRepo.AddProductInUserBasket(UserId, ProductId);
            return Redirect("Index");
        }

            [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}