using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ogani.DataContext;
using Ogani.Models;
using System.Diagnostics;

namespace Ogani.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _dbContext;

        public HomeController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var products = _dbContext.Products.ToList();
            var categories = _dbContext.Categories.ToList();

            var homeViewModel = new HomeViewModel
            {
                Products = products,
                Categories = categories,
            };

            return View(homeViewModel);
        }
    }
}
