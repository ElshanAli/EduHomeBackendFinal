using BackendFinalProjectEduHome.DAL.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BackendFinalProjectEduHome.Areas.Admin.ViewModels
{
    public class CourseCreateViewModel
    {
        public string? ImageUrl { get; set; }
        public IFormFile Image { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string About { get; set; }
        public string Certification { get; set; }
        public string HowToApply { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Duration { get; set; }
        public string ClassDuration { get; set; }
        public string SkillLevel { get; set; }
        public string Language { get; set; }
        public int StudentCount { get; set; }
        public string Assesments { get; set; }
        public int CourseFee { get; set; }
        public int? CategoryId { get; set; }
        public List<SelectListItem>? Categories { get; set; }
    }
}
