using Microsoft.EntityFrameworkCore;
using Ogani.DataContext.Entities;
using System.Collections.Generic;

namespace Ogani.DataContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}

