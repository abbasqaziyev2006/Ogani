using Microsoft.AspNetCore.Mvc.Rendering;
using Ogani.DataContext.Entities;

namespace Ogani.Admin.Models
{
    public class ProductUpdateViewModel
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string CoverImageUrl { get; set; } = null!;
        public IFormFile? CoverImageFile { get; set; }
        public List<ProductImage> Images { get; set; } = [];
        public IFormFile[]? ImageFiles { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public List<SelectListItem> CategorySelectListItems { get; set; } = [];
    }
}
