namespace BackendFinalProjectEduHome.Areas.Admin.ViewModels
{
    public class SpeakerCreateViewModel
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public IFormFile Image { get; set; }
        public string Position { get; set; }
        public string Company { get; set; }
    }
}
