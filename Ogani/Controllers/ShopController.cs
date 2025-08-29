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

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details(int id)
        {
            var product = _dbContext.Products.Include(p => p.Category!).Include(p => p.Images).FirstOrDefault(p => p.Id == id);

            if (product == null) return NotFound();

            var relatedProducts = _dbContext.Products.Where(p => p.CategoryId == product.CategoryId && p.Id != product.Id).Take(4).ToList();

            var productDetailsViewModel = new ProductDetailsViewModel
            {
                Product = product,
                RelatedProducts = relatedProducts
            };

            return View(productDetailsViewModel);
        }
    }
}

