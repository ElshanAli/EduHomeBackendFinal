using BackendFinalProjectEduHome.DAL.Entity;

namespace BackendFinalProjectEduHome.ViewModels
{
    public class AboutViewModel
    {
        public NoticeBoard NoticeBoards { get; set; } = new NoticeBoard();
        public WelcomeEdu WelcomeEdu { get; set; } = new WelcomeEdu();
    }
}
