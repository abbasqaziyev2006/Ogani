using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ogani.Admin.Models
{
    public class ProductUpdateViewModel
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }

        public string? CoverImageUrl { get; set; }
        public IFormFile? CoverImageFile { get; set; }

        public int CategoryId { get; set; }
        public List<SelectListItem> CategorySelectListItems { get; set; } = new();
    }
}
