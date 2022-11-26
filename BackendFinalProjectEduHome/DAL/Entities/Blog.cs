

namespace BackendFinalProjectEduHome.DAL.Entity
{
    public class Blog : Entity
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }  
        public DateTime CreatedDate { get; set; }
        public string Description { get; set; }
    }
}
