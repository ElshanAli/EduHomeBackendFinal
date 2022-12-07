using BackendFinalProjectEduHome.DAL;
using BackendFinalProjectEduHome.DAL.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendFinalProjectEduHome.Controllers
{
    public class BlogController : Controller
    {
        private readonly EduHomeDbContext _dbContext;
        private int _blogCount;
        public BlogController(EduHomeDbContext dbContext)
        {
            _dbContext = dbContext;
            _blogCount = _dbContext.Blogs.Count();
        }

        public async Task<IActionResult> Index()
        {

            ViewBag.blogCount = _blogCount;
            List<Blog> dbBlogs = await _dbContext.Blogs.Take(3).ToListAsync();

            return View(dbBlogs);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var dbBlogs = await _dbContext.Blogs.Where(x => x.Id == id && !x.IsDeleted).FirstOrDefaultAsync();
            if (dbBlogs == null) return NotFound();

            return View(dbBlogs);
        }

        public async Task<IActionResult> Partial(int toPass)
        {
            if (toPass >= _blogCount)
                return BadRequest();
            var dbBlogs = await _dbContext.Blogs.Skip(toPass).Take(3).ToListAsync();

            return PartialView("_BlogPartialView", dbBlogs);
        }
    }
}
