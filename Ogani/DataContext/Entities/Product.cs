using Microsoft.EntityFrameworkCore;

namespace Ogani.DataContext.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }

        [Precision(18, 2)]
        public decimal Price { get; set; }

        [Precision(18, 2)]
        public decimal Weight { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; } = new();
        public List<ProductImage> Images { get; set; } = [];
    }
}


