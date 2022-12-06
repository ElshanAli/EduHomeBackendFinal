using BackendFinalProjectEduHome.DAL.Entity;

namespace BackendFinalProjectEduHome.ViewModels
{
    public class HomeViewModel
    {
        public List<Slider> Sliders { get; set; } = new List<Slider>();
        public List<Course> Courses { get; set; } = new List<Course>();
    }
}
