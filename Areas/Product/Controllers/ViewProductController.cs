using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using appmvcnet.Areas.Product.Models;
using appmvcnet.Areas.Product.Services;
using appmvcnet.DatabaseContext;
using appmvcnet.Models;
using appmvcnet.Models.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace appmvcnet.Areas.Product.Controllers
{
    [Area("Product")]
    public class ViewProductController : Controller
    {
        private readonly ILogger<ViewProductController> _logger;
        private readonly AppDbContext _context;

        private readonly CartService _cartService;

        public ViewProductController(ILogger<ViewProductController> logger, AppDbContext context, CartService cartService)
        {
            _logger = logger;
            _context = context;
            _cartService = cartService;
        }

        // /post/
        // /post/{categoryslug?}
        [Route("/product/{categoryslug?}")]
        public IActionResult Index(string categoryslug, [FromQuery(Name = "p")] int currentPage, int pagesize)
        {
            var categories = GetCategories();

            ViewBag.categories = categories;
            ViewBag.categoryslug = categoryslug;

            CategoryProduct category = null;

            if (!string.IsNullOrEmpty(categoryslug))
            {
                category = _context.CategoryProducts.Where(c => c.Slug == categoryslug)
                                    .Include(c => c.CategoryChildren)
                                    .FirstOrDefault();

                if (category == null)
                {
                    return NotFound();
                }
            }

            var products = _context.Products
                                .Include(p => p.Author)
                                .Include(p => p.Photos)
                                .Include(p => p.ProductCategoryProducts)
                                .ThenInclude(p => p.Category)
                                .AsQueryable();

            products = products.OrderByDescending(p => p.DateUpdated);

            if (category != null)
            {
                var ids = new List<int>();
                category.ChildCategoryIDs(null, ids);
                ids.Add(category.Id);


                products = products.Where(p => p.ProductCategoryProducts.Where(pc => ids.Contains(pc.CategoryID)).Any());


            }

            int totalProducts = products.Count();
            if (pagesize <= 0) pagesize = 6;
            int countPages = (int)Math.Ceiling((double)totalProducts / pagesize);

            if (currentPage > countPages) currentPage = countPages;
            if (currentPage < 1) currentPage = 1;

            var pagingModel = new PagingModel()
            {
                countpages = countPages,
                currentpage = currentPage,
                generateUrl = (pageNumber) => Url.Action("Index", new
                {
                    p = pageNumber,
                    pagesize = pagesize
                })
            };

            var productsInPage = products.Skip((currentPage - 1) * pagesize)
                             .Take(pagesize);


            ViewBag.pagingModel = pagingModel;
            ViewBag.totalPosts = totalProducts;



            ViewBag.category = category;
            return View(productsInPage.ToList());
        }

        [Route("/product/{productslug}.html")]
        public IActionResult Detail(string productslug)
        {
            var categories = GetCategories();
            ViewBag.categories = categories;

            var product = _context.Products.Where(p => p.Slug == productslug)
                               .Include(p => p.Author)
                               .Include(p => p.Photos)
                               .Include(p => p.ProductCategoryProducts)
                               .ThenInclude(pc => pc.Category)
                               .FirstOrDefault();

            if (product == null)
            {
                return NotFound();
            }

            CategoryProduct category = product.ProductCategoryProducts.FirstOrDefault()?.Category;
            ViewBag.category = category;

            var otherProducts = _context.Products.Where(p => p.ProductCategoryProducts.Any(c => c.Category.Id == category.Id))
                                            .Where(p => p.ProductID != product.ProductID)
                                            .OrderByDescending(p => p.DateUpdated)
                                            .Take(5);
            ViewBag.otherProducts = otherProducts;

            return View(product);
        }

        private List<CategoryProduct> GetCategories()
        {
            var categories = _context.CategoryProducts
                            .Include(c => c.CategoryChildren)
                            .AsEnumerable()
                            .Where(c => c.ParentCategory == null)
                            .ToList();
            return categories;
        }

        [Route("addcart/{productid:int}", Name = "addcart")]
        public IActionResult AddToCart([FromRoute] int productid)
        {

            var product = _context.Products
                .Where(p => p.ProductID == productid)
                .FirstOrDefault();
            if (product == null)
                return NotFound();

            var cart = _cartService.GetCartItems();
            var cartitem = cart.Find(p => p.product.ProductID == productid);
            if (cartitem != null)
            {
                cartitem.quantity++;
            }
            else
            {
                cart.Add(new CartItem() { quantity = 1, product = product });
            }

            _cartService.SaveCartSession(cart);
            return RedirectToAction(nameof(Cart));
        }

        [Route("/cart", Name = "cart")]
        public IActionResult Cart()
        {
            return View(_cartService.GetCartItems());
        }

        [Route("/removecart/{productid:int}", Name = "removecart")]
        public IActionResult RemoveCart([FromRoute] int productid)
        {
            var cart = _cartService.GetCartItems();
            var cartitem = cart.Find(p => p.product.ProductID == productid);
            if (cartitem != null)
            {
                cart.Remove(cartitem);
            }

            _cartService.SaveCartSession(cart);
            return RedirectToAction(nameof(Cart));
        }

        [Route("/updatecart", Name = "updatecart")]
        [HttpPost]
        public IActionResult UpdateCart([FromForm] int productid, [FromForm] int quantity)
        {
            var cart = _cartService.GetCartItems();
            var cartitem = cart.Find(p => p.product.ProductID == productid);
            if (cartitem != null)
            {
                cartitem.quantity = quantity;
            }
            _cartService.SaveCartSession(cart);
            return Ok();
        }

        [Route("/checkout")]
        public IActionResult Checkout()
        {
            var cart = _cartService.GetCartItems();

            // ....
            _cartService.ClearCart();

            return Content("Checkout completed");

        }

    }
}