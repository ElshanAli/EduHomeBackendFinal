using System.ComponentModel.DataAnnotations;

namespace BackendFinalProjectEduHome.Areas.Admin.ViewModels
{
    public class NoticeBoardCreateViewModel
    {
        public string NoticeTitle { get; set; }
        public string NoticeDescription { get; set; }
        [DataType(DataType.Url)]
        public string VideoUrl { get; set; }
    }
}
