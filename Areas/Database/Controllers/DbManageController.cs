using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using appmvcnet.DatabaseContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace appmvcnet.Areas.Database.Controllers
{
    [Area("Database")]
    [Route("/database-manage/[action]")]
    public class DbManageController : Controller
    {
        private readonly AppDbContext _dbContext;
        
        [TempData]
        public string StatusMessage { get; set; }

        public DbManageController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult DeleteDb()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDbAsync()
        {
            var success = await _dbContext.Database.EnsureDeletedAsync();
            StatusMessage = success ? "Removing Db succeeded" : "Failed to remove Db";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Migrate()
        {
            await _dbContext.Database.MigrateAsync();
            StatusMessage = "Update Database succeeded";
            return RedirectToAction(nameof(Index));
        }
    }
}