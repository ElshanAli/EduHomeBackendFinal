using BackendFinalProjectEduHome.DAL.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BackendFinalProjectEduHome.Areas.Admin.ViewModels
{
    public class CourseFeatureViewModel
    {
        public List<Course> Courses { get; set; } = new();
        public List<Feature> Features { get; set; }
        public Course Course { get; set; }
        public Feature Feature { get; set; }
        public List<SelectListItem> Categories { get; set; } = new();   
        public IFormFile Image { get; set; }    
    }
}
