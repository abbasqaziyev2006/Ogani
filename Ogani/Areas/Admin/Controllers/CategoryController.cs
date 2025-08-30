using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ogani.DataContext.Entities;
using Ogani.DataContext;
using Ogani.Areas.Admin.Data;
using Ogani.Areas.Admin.Models;


namespace Ogani.Areas.Admin.Controllers
{
    public class CategoryController : AdminController
    {
        private readonly AppDbContext _dbContext;

        public CategoryController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _dbContext.Categories.Include(c => c.Products).ToListAsync();

            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateViewModel categoryModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (!categoryModel.ImageFile.IsImage())
            {
                ModelState.AddModelError("ImageUrl", "No image selected.");

                return View(categoryModel);
            }

            if (!categoryModel.ImageFile.IsAllowedSize(1))
            {
                ModelState.AddModelError("ImageUrl", "The measurement is not correct.");

                return View(categoryModel);
            }
            var unicalCoverName = await categoryModel.ImageFile.GenerateFileAsync(PathConstants.CategoryPath);

            var existedCategory = await _dbContext.Categories
                .AnyAsync(x => x.Name.ToLower() == categoryModel.Name.ToLower());

            if (existedCategory)
            {
                ModelState.AddModelError("Name", "This category already exists!");
                return View();
            }

            var category = new Category
            {
                ImageUrl = unicalCoverName,
                Name = categoryModel.Name,
            };

            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                return NotFound();

            var categoryUpdateViewModel = new CategoryUpdateViewModel
            {
                Id = category.Id,
                Name = category.Name,
                ImageUrl = category.ImageUrl,
            };

            return View(categoryUpdateViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(CategoryUpdateViewModel categoryModel, int id)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                return View(categoryModel);
            }

            if (categoryModel.ImageFile != null)
            {
                if (!categoryModel.ImageFile.IsImage())
                {
                    ModelState.AddModelError("ImageFile", "No image selected.");

                    return View(categoryModel);
                }

                if (!categoryModel.ImageFile.IsAllowedSize(1))
                {
                    ModelState.AddModelError("ImageFile", "The measurement is not correct.");

                    return View(categoryModel);
                }

                var oldPath = Path.Combine(PathConstants.CategoryPath, category.ImageUrl);
                if (System.IO.File.Exists(oldPath))
                    System.IO.File.Delete(oldPath);

                var unicalCoverName = await categoryModel.ImageFile.GenerateFileAsync(PathConstants.CategoryPath);
                category.ImageUrl = unicalCoverName;
            }

            category.Name = categoryModel.Name;

            _dbContext.Categories.Update(category);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var category = _dbContext.Categories.FirstOrDefault(x => x.Id == id);

            if (category == null)
                return NotFound();

            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}