
namespace BackendFinalProjectEduHome.DAL.Entity
{
    public class Feature : Entity
    {
        public DateTime StartDate { get; set; }    
        public string Duration { get; set; }
        public string ClassDuration { get; set; }
        public string SkillLevel { get; set; }
        public string Language { get; set; }
        public int StudentCount { get; set; }
        public string Assesments { get; set; }
        public int CourseFee { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
