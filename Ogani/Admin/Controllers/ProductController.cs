using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ogani.Admin.Controllers;
using Ogani.Admin.Data;
using Ogani.Admin.Models;
using Ogani.DataContext.Entities;
using Ogani.DataContext;


namespace OganiAdminPanelTask.Areas.Admin.Controllers
{
    public class ProductController : AdminController
    {
        private readonly AppDbContext _dbContext;

        public ProductController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _dbContext.Products.Include(p => p.Category).ToListAsync();

            return View(products);
        }

        public async Task<IActionResult> Create()
        {
            var categorySelectListItems = await GetCategoryListItems();

            var productCreateViewModel = new ProductCreateViewModel
            {
                CategorySelectListItems = categorySelectListItems,
            };

            return View(productCreateViewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateViewModel productModel)
        {
            var existedProduct = await _dbContext.Products.AnyAsync(x => x.Name.ToLower() == productModel.Name.ToLower());
            if (existedProduct)
            {
                ModelState.AddModelError("Name", "This product exists");

                return View(productModel);
            }

            if (!ModelState.IsValid)
            {
                productModel.CategorySelectListItems = await GetCategoryListItems();

                return View(productModel);
            }

            if (!productModel.CoverImageFile.IsImage())
            {
                productModel.CategorySelectListItems = await GetCategoryListItems();
                ModelState.AddModelError("CoverImageFile", "No image selected.");

                return View(productModel);
            }

            if (!productModel.CoverImageFile.IsAllowedSize(1))
            {
                productModel.CategorySelectListItems = await GetCategoryListItems();
                ModelState.AddModelError("CoverImageFile", "The picture size is large.");

                return View(productModel);
            }

            foreach (var imageFile in productModel.ImageFiles)
            {
                if (!imageFile.IsImage())
                {
                    productModel.CategorySelectListItems = await GetCategoryListItems();
                    ModelState.AddModelError("ImageFiles", "No image selected.");

                    return View(productModel);
                }

                if (!imageFile.IsAllowedSize(1))
                {
                    productModel.CategorySelectListItems = await GetCategoryListItems();
                    ModelState.AddModelError("ImageFiles", "The picture size is large.");

                    return View(productModel);
                }
            }

            var productImages = new List<ProductImage>();

            foreach (var imageFile in productModel.ImageFiles)
            {
                var imageUnicalName = await imageFile.GenerateFileAsync(PathConstants.ProductPath);
                productImages.Add(new ProductImage
                {
                    Name = imageUnicalName,
                });
            }

            var coverUnicalName = await productModel.CoverImageFile.GenerateFileAsync(PathConstants.ProductPath);

            var product = new Product
            {
                Name = productModel.Name,
                Price = productModel.Price,
                ImageUrl = coverUnicalName,
                ProductImages = productImages,
                CategoryId = productModel.CategoryId,
            };

            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var product = await _dbContext.Products.Include(x => x.ProductImages)
                .Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);

            if (product == null)
                return NotFound();

            var categoryListItems = await GetCategoryListItems();

            var productUpdateViewModel = new ProductUpdateViewModel
            {
                Name = product.Name,
                Price = product.Price,
                CoverImageUrl = product.ImageUrl,
                Images = product.ProductImages,
                CategoryId = product.CategoryId,
                CategorySelectListItems = categoryListItems,
            };

            return View(productUpdateViewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Update(ProductUpdateViewModel productModel, int id)
        {
            var product = await _dbContext.Products.Include(x => x.ProductImages)
                .Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);

            if (product == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                productModel = await GetProductUpdateViewModel(product);

                return View(productModel);
            }

            if (productModel.CoverImageFile != null)
            {
                if (!productModel.CoverImageFile.IsImage())
                {
                    productModel = await GetProductUpdateViewModel(product);
                    ModelState.AddModelError("CoverImageFile", "No image selected.");

                    return View(productModel);
                }

                if (!productModel.CoverImageFile.IsAllowedSize(1))
                {
                    productModel = await GetProductUpdateViewModel(product);
                    ModelState.AddModelError("CoverImageFile", "The picture size is large.");

                    return View(productModel);
                }

                var oldPath = Path.Combine(PathConstants.ProductPath, product.ImageUrl);

                if (System.IO.File.Exists(oldPath))
                    System.IO.File.Delete(oldPath);

                var coverUnicalName = await productModel.CoverImageFile.GenerateFileAsync(PathConstants.ProductPath);
                product.ImageUrl = coverUnicalName;
            }

            var productImages = new List<ProductImage>();

            if (productModel.ImageFiles != null)
            {
                foreach (var file in productModel.ImageFiles)
                {
                    if (!file.IsImage())
                    {
                        productModel = await GetProductUpdateViewModel(product);
                        ModelState.AddModelError("ImageFiles", "No image selected.");

                        return View(productModel);
                    }

                    if (!file.IsAllowedSize(1))
                    {
                        productModel = await GetProductUpdateViewModel(product);
                        ModelState.AddModelError("ImageFiles", "The picture size is large.");

                        return View(productModel);
                    }
                }

                foreach (var file in productModel.ImageFiles)
                {
                    var imageUnicalName = await file.GenerateFileAsync(PathConstants.ProductPath);
                    productImages.Add(new ProductImage
                    {
                        Name = imageUnicalName,
                    });
                }
            }

            product.Name = productModel.Name;
            product.Price = productModel.Price;
            product.CategoryId = productModel.CategoryId;
            product.ProductImages.AddRange(productImages);

            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProductImage(int id)
        {
            var productImage = await _dbContext.ProductImages.FirstOrDefaultAsync(x => x.Id == id);

            if (productImage == null)
                return NotFound();

            _dbContext.ProductImages.Remove(productImage);
            await _dbContext.SaveChangesAsync();

            return Json(new { productImage.Id, productImage.Name });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (product == null)
                return NotFound();

            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();

            return Json(new { product.Id, product.Name });
        }

        public async Task<ProductUpdateViewModel> GetProductUpdateViewModel(Product product)
        {
            ProductUpdateViewModel productModel = new();

            var categoryListItems = await GetCategoryListItems();

            productModel.CategorySelectListItems = categoryListItems;
            productModel.CoverImageUrl = product.ImageUrl;
            productModel.Images = product.ProductImages;

            return productModel;
        }
        public async Task<List<SelectListItem>> GetCategoryListItems()
        {
            var categories = await _dbContext.Categories.ToListAsync();
            var categoryListItems = new List<SelectListItem>();

            foreach (var category in categories)
            {
                categoryListItems.Add(new SelectListItem(category.Name, category.Id.ToString()));
            }

            return categoryListItems;
        }
    }
}