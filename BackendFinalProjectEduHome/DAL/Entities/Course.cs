
namespace BackendFinalProjectEduHome.DAL.Entity
{
    public class Course : Entity
    {
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string About { get; set; }
        public string Certification { get; set; }
        public string HowToApply { get; set; }
        public Feature Features { get; set; }
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
