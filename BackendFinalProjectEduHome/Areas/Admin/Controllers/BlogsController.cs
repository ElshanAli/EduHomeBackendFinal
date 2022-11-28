using BackendFinalProjectEduHome.Areas.Admin.Data;
using BackendFinalProjectEduHome.Areas.Admin.ViewModels;
using BackendFinalProjectEduHome.Areas.ViewModels;
using BackendFinalProjectEduHome.DAL;
using BackendFinalProjectEduHome.DAL.Entity;
using BackendFinalProjectEduHome.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendFinalProjectEduHome.Areas.Admin.Controllers
{
    public class BlogsController : BaseController
    {
        private readonly EduHomeDbContext _dbContext;

        public BlogsController(EduHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Blog> blogs = await _dbContext.Blogs
                .Where(b => !b.IsDeleted)
                .OrderByDescending(e => e.Id)
                .ToListAsync();

            return View(blogs);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
         
                if (!model.Image.IsImage())
            {
                ModelState.AddModelError("", "Must be selected image");
                return View(model);
            }

            if (!model.Image.IsAllowedSize(7))
            {
                ModelState.AddModelError("", "Image size can be max 7 mb");
                return View(model);
            }

            var unicalName = await model.Image.Generatefile(Constants.BlogPath);

            var blogs = new Blog
            {
                ImageUrl = unicalName,
                Title = model.Title,
                Name = model.Name,
                CreatedDate = DateTime.Now,
                Description = model.Description,
            };

            await _dbContext.Blogs.AddAsync(blogs);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return BadRequest();

            var blogs = await _dbContext.Blogs
                .Where(bl => bl.Id == id)
                .FirstOrDefaultAsync();


            if (blogs is null) return NotFound();

            var blogViewModel = new BlogUpdateViewModel
            {
                Id = blogs.Id,
                Title = blogs.Title,
                Description = blogs.Description,
                Name = blogs.Name,
                ImageUrl = blogs.ImageUrl
            };

            return View(blogViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, BlogUpdateViewModel model)
        {
            if (id is null) return BadRequest();

            var blogs = await _dbContext.Blogs
                .Where(bl => bl.Id == id)
                .FirstOrDefaultAsync();

            if (blogs is null) return NotFound();

            if (!ModelState.IsValid) return View(new BlogUpdateViewModel
            {
                ImageUrl = blogs.ImageUrl
            });

            if (model.Image != null)
            {
                if (!model.Image.IsImage())
                {
                    ModelState.AddModelError("", "Must be selected image");
                    return View(new BlogUpdateViewModel
                    {
                        ImageUrl = blogs.ImageUrl,
                    });
                }

                if (!model.Image.IsAllowedSize(7))
                {
                    ModelState.AddModelError("", "Image size can be max 7 mb");
                    return View(model);
                }
                if (blogs.ImageUrl is null) return NotFound();
                
                var blogImagePath = Path.Combine(Constants.RootPath, "assets", "img", "blog", blogs.ImageUrl);

                if (System.IO.File.Exists(blogImagePath))
                    System.IO.File.Delete(blogImagePath);

                var unicalName = await model.Image.Generatefile(Constants.BlogPath);
                blogs.ImageUrl = unicalName;
            }

            blogs.Title = model.Title;
            blogs.Description = model.Description;
            blogs.Name = model.Name;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var blogs = await _dbContext.Blogs.FirstOrDefaultAsync(bl => bl.Id == id);

            if (blogs == null) return NotFound();

            if (blogs.ImageUrl == null) return NotFound();

            if (blogs.Id != id) return BadRequest();

            var blogImagePath = Path.Combine(Constants.RootPath, "assets", "img", "blog", blogs.ImageUrl);

            if (System.IO.File.Exists(blogImagePath))
                System.IO.File.Delete(blogImagePath);

            _dbContext.Blogs.Remove(blogs);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
