using BackendFinalProjectEduHome.DAL.Entity;
using Microsoft.EntityFrameworkCore;

namespace BackendFinalProjectEduHome.DAL
{
    public class EduHomeDbContext : DbContext
    {
        public EduHomeDbContext(DbContextOptions<EduHomeDbContext> options) : base(options)
        {

        }

        public DbSet<Slider> Sliders { get; set; }
    }
}
