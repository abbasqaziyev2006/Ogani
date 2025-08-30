namespace Ogani.Admin.Models
{
    public class CategoryUpdateViewModel
    {
        public required string Name { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
