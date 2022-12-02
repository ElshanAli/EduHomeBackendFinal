namespace BackendFinalProjectEduHome.Areas.Admin.ViewModels
{
    public class WelcomeEduUpdateViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? Image { get; set; }

    }
}
