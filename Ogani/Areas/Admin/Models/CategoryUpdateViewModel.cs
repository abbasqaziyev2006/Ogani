namespace Ogani.Areas.Admin.Models
{
    public class CategoryUpdateViewModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
