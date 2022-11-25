using System.ComponentModel.DataAnnotations;

namespace BackendFinalProjectEduHome.Areas.ViewModels
{
    public class SliderUpdateViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string BtnText { get; set; }
        [DataType(DataType.Url)]
        public string BtnSrc { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? Image { get; set; }

    }
}
