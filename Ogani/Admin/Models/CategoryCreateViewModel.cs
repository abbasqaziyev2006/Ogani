namespace Ogani.Admin.Models
{
    public class CategoryCreateViewModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public IFormFile ImageFile { get; set; } = null!;
    }
}
