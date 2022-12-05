using BackendFinalProjectEduHome.DAL.Entity;

namespace BackendFinalProjectEduHome.ViewModels
{
    public class AboutViewModel
    {
        public List<NoticeBoard> NoticeBoards { get; set; } = new List<NoticeBoard>();
        public WelcomeEdu WelcomeEdu { get; set; } = new WelcomeEdu();
    }
}
