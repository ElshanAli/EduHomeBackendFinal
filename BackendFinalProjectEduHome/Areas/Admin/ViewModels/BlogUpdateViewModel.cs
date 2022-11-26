namespace BackendFinalProjectEduHome.Areas.Admin.ViewModels
{
    public class BlogUpdateViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? Image { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
