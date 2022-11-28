using BackendFinalProjectEduHome.DAL.Entity;

namespace BackendFinalProjectEduHome.Areas.Admin.ViewModels
{
    public class SpeakerUpdateViewModel
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? Image { get; set; }
        public string Position { get; set; }
        
    }
}
