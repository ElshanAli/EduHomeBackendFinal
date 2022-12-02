using BackendFinalProjectEduHome.Areas.Admin.Data;
using BackendFinalProjectEduHome.Areas.Admin.ViewModels;
using BackendFinalProjectEduHome.DAL;
using BackendFinalProjectEduHome.DAL.Entity;
using BackendFinalProjectEduHome.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendFinalProjectEduHome.Areas.Admin.Controllers
{
    public class CoursesController : BaseController
    {
        private readonly EduHomeDbContext _dbContext;

        public CoursesController(EduHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            CourseFeatureViewModel courseFeatureViewModel = new CourseFeatureViewModel
            {
                Courses = await _dbContext.Courses.Include(c => c.Features).ToListAsync()
            };

            return View(courseFeatureViewModel);
        }

        public IActionResult Create()
        {
            ViewBag.CategoryIds = _dbContext.Categories.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseFeatureViewModel model)
        {
            //if(!ModelState.IsValid) return View(model);
            if (model.Course.CategoryId == 0)
            {
                model.Course.CategoryId = null;
            }


             ViewBag.CategoryIds = await _dbContext.Categories.ToListAsync();

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
            model.Course.ImageUrl = await model.Image.Generatefile(Constants.CoursePath);
            model.Course.Features = model.Feature;
            model.Feature.Course = model.Course;
            model.Feature.CourseId = model.Course.Id;

            await _dbContext.Courses.AddAsync(model.Course);
            await _dbContext.Features.AddAsync(model.Feature);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
