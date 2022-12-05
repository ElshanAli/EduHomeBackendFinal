using BackendFinalProjectEduHome.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendFinalProjectEduHome.Controllers
{
    public class TeacherController : Controller
    {
        private readonly EduHomeDbContext _dbContext;

        public TeacherController(EduHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var dbTeachers = await _dbContext.Teachers.Where(t => !t.IsDeleted).ToListAsync();

            return View(dbTeachers);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if(id is null) return NotFound();

            var dbTeachers = await _dbContext.Teachers.Where(t => t.Id == id && !t.IsDeleted).FirstOrDefaultAsync();

            return View(dbTeachers);
        }
    }
}
