using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using appmvcnet.Models;
using appmvcnet.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace appmvcnet.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context;

    public HomeController(ILogger<HomeController> logger, AppDbContext context)
    {
        _context = context;
        _logger = logger;
    }

    public IActionResult Index()
    {
        var products = _context.Products
                            .Include(p => p.Author)
                            .Include(p => p.Photos)
                            .Include(p => p.ProductCategoryProducts)
                            .ThenInclude(p => p.Category)
                            .AsQueryable();

        products = products.OrderByDescending(p => p.DateUpdated).Take(4);

        var posts = _context.Posts
                            .Include(p => p.Author)
                            .Include(p => p.PostCategories)
                            .ThenInclude(p => p.Category)
                            .AsQueryable();

        posts = posts.OrderByDescending(p => p.DateUpdated).Take(3);


        ViewBag.products = products;
        ViewBag.posts = posts;

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
