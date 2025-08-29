using Azure;
using Microsoft.EntityFrameworkCore;
using Ogani.DataContext.Entities;

namespace Ogani.Models
{
    public class ShopViewModel
    {
        public List<Product> Products { get; set; } = new List<Product>();
        public List<Category> Categories { get; set; } = new List<Category>();
    }
}
