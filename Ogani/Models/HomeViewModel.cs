using Ogani.DataContext.Entities;

namespace Ogani.Models
{
    public class HomeViewModel
    {
        public List<Product> Products { get; set; } = new();
        public List<Category> Categories { get; set; } = new();
        public List<ProductImage>? ProductImages { get;  set; }
    }
}
