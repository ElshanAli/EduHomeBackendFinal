namespace BackendFinalProjectEduHome.Areas.Admin.ViewModels
{
    public class WelcomeEduCreateViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile Image { get; set; }
    }
}
