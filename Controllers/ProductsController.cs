using System.Diagnostics;
using appmvcnet.Models;
using appmvcnet.Services;
using Microsoft.AspNetCore.Mvc;

namespace appmvcnet.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly ProductService _productService;

        public ProductsController(ILogger<ProductsController> logger, ProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [TempData]
        public string StatusMessage { get; set; }

        public IActionResult ViewProduct(int? id)
        {
            var product = _productService.Where(p => p.Id == id).FirstOrDefault();
            if (product == null)
            {
                StatusMessage = "Product not found";
                return Redirect(Url.Action("Index", "Home"));
            }
            ViewData["Title"] = product.Name;

            ViewBag.product = product;
            
            return View(product);
        }
    }
}