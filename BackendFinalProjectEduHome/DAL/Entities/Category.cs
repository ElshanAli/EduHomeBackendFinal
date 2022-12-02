
namespace BackendFinalProjectEduHome.DAL.Entity
{
    public class Category : Entity
    {
        public string Name { get; set; }
        public List<Course> Courses { get; set; }
    }
}
