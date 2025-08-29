using Ogani.DataContext.Entities;

namespace Ogani.Models
{
    public class ProductDetailsViewModel
    {
        public Product? Product { get; set; }
        public List<Category> Categories { get; set; } = new();
        public List<Product> RelatedProducts { get; set; } = new();
    }
}
