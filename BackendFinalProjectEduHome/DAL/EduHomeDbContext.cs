using BackendFinalProjectEduHome.DAL.Entity;
using Microsoft.EntityFrameworkCore;

namespace BackendFinalProjectEduHome.DAL
{
    public class EduHomeDbContext : DbContext
    {
        public EduHomeDbContext(DbContextOptions<EduHomeDbContext> options) : base(options) { }

        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Speaker> Speakers { get; set; }
        public DbSet<EventSpeaker> EventSpeakers { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<WelcomeEdu> WelcomeEdu { get; set; }
        public DbSet<NoticeBoard> NoticeBoards { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
    }
}
