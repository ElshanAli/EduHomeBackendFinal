using System.ComponentModel.DataAnnotations;

namespace BackendFinalProjectEduHome.Areas.ViewModels
{
    public class SliderCreateViewModel
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string BtnText { get; set; }
        [DataType(DataType.Url)]
        public string BtnSrc { get; set; }
        public IFormFile Image { get; set; }

    }
}
