namespace Ogani.Admin.Models
{
    public class CategoryCreateViewModel
    {
        public required string Name { get; set; }
        public IFormFile ImageFile { get; set; } = null!;
    }
}
