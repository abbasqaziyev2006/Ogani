using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ogani.Admin.Models
{
    public class ProductCreateViewModel
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }

        public IFormFile CoverImageFile { get; set; } = null!;
        public IFormFile[] ImageFiles { get; set; } = [];
        public int CategoryId { get; set; }
        public List<SelectListItem> CategorySelectListItems { get; set; } = new();
        public List<SelectListItem> TagSelectListItems { get; set; } = [];
    }
}
