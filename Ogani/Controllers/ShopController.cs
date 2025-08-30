using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ogani.DataContext;
using Ogani.Models;

namespace Ogani.Controllers
{

    public class ShopController : Controller
    {
        private readonly AppDbContext _dbContext;

        public ShopController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _dbContext.Products.ToListAsync();
            var categories = await _dbContext.Categories.ToListAsync();

            var shopViewModel = new ShopViewModel
            {
                Products = products,
                Categories = categories
            };

            return View(shopViewModel);
        }
    }
}

