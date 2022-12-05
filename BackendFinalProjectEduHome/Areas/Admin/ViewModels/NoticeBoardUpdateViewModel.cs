using System.ComponentModel.DataAnnotations;

namespace BackendFinalProjectEduHome.Areas.Admin.ViewModels
{
    public class NoticeBoardUpdateViewModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string NoticeDescription { get; set; }
        [DataType(DataType.Url)]
        public string VideoUrl { get; set; }
    }
}
