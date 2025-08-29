using Ogani.DataContext.Entities;

namespace Ogani.Admin.Models
{
    public class ProductDetailViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string? CoverImageUrl { get; set; }
        public string? CategoryName { get; set; }
        public List<ProductImage> Images { get; set; } = [];
        public List<string> Tags { get; set; } = [];
    }
}
