using BackendFinalProjectEduHome.DAL;
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

        public IActionResult Index()
        {

            ViewBag.blogCount = _blogCount;
            var dbBlogs = _dbContext.Blogs.Take(3).ToList();

            return View(dbBlogs);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var dbBlogs = await _dbContext.Blogs.Where(x => x.Id == id && !x.IsDeleted).FirstOrDefaultAsync();
            if (dbBlogs == null) return NotFound();

            return View(dbBlogs);
        }

        public async Task<IActionResult> Partial(int skip)
        {
            if (skip >= _blogCount)
                return BadRequest();
            var dbBlogs = await _dbContext.Blogs.Skip(skip).Take(3).ToListAsync();

            return PartialView("_BlogPartialView", dbBlogs);
        }
    }
}
