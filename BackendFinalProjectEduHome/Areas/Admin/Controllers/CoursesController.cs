using BackendFinalProjectEduHome.Areas.Admin.Data;
using BackendFinalProjectEduHome.Areas.Admin.ViewModels;
using BackendFinalProjectEduHome.DAL;
using BackendFinalProjectEduHome.DAL.Entity;
using BackendFinalProjectEduHome.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Execution;
using Microsoft.EntityFrameworkCore;
using System.IO;

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
            IEnumerable<Course> dbCourse = await _dbContext.Courses.Include(c => c.Category).OrderByDescending(c=>c.Id).ToListAsync();

            return View(dbCourse);
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _dbContext.Categories.Where(c => !c.IsDeleted).ToListAsync();

            var categoryListItem = new List<SelectListItem>
            {
                new SelectListItem("Choose category...", "0")
            };

            categories.ForEach(c => categoryListItem.Add(new SelectListItem(c.Name, c.Id.ToString())));

            var model = new CourseCreateViewModel
            {
                Categories = categoryListItem
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var categories = await _dbContext.Categories
                .Where(c => !c.IsDeleted)
                .Include(c => c.Courses)
                .ToListAsync();

            var categoryListItem = new List<SelectListItem>
            {
                new SelectListItem("Choose category...", "0")
            };

            categories.ForEach(c => categoryListItem.Add(new SelectListItem(c.Name, c.Id.ToString())));

            var courseViewModel = new CourseCreateViewModel
            {
                Categories = categoryListItem
            };

            var createdCourse = new Course();

            if (DateTime.Compare(DateTime.UtcNow.AddHours(4), model.StartDate) >= 0)
            {
                ModelState.AddModelError("StartDate", "Start Date must be future");
                return View(model);
            }

            if (model.StartDate.ToString("yyyy-MM-dd 07:00") != model.StartDate.ToString("yyyy-MM-dd 20:00"))
            {
                ModelState.AddModelError("", "You must select this time slot From: 07:00 To: 20:00");
                return View(model);
            }

            //if (DateTime.Compare(DateTime.UtcNow.AddHours(4), model.EndDate) >= 0)
            //{
            //    ModelState.AddModelError("EndDate", "End Date must be future and after Start Date");
            //    return View(model);
            //}

            //if (DateTime.Compare(model.StartDate, model.EndDate) >= 0)
            //{
            //    ModelState.AddModelError("", "Start Date must be earlier than End Date");
            //    return View(model);
            //}

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

            var unicalName = await model.Image.Generatefile(Constants.CoursePath);

            if (model.CategoryId == 0)
            {
                ModelState.AddModelError("", "Must be select category");
                return View(model);
            }

            createdCourse.ImageUrl = unicalName;
            createdCourse.Title = model.Title;
            createdCourse.Description = model.Description;
            createdCourse.About = model.About;
            createdCourse.HowToApply = model.HowToApply;
            createdCourse.Certification = model.Certification;
            createdCourse.Duration = model.Duration;
            createdCourse.StartDate = model.StartDate;
            createdCourse.EndDate = model.EndDate;
            createdCourse.ClassDuration = model.ClassDuration;
            createdCourse.SkillLevel = model.SkillLevel;
            createdCourse.Language = model.Language;
            createdCourse.StudentCount = model.StudentCount;
            createdCourse.Assesments = model.Assesments;
            createdCourse.CourseFee = model.CourseFee;
            createdCourse.CategoryId = model.CategoryId;

            await _dbContext.Courses.AddAsync(createdCourse);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();

            var dbCategories = await _dbContext.Categories
                .Where(c => !c.IsDeleted)
                .ToListAsync();

            if (dbCategories is null) return NotFound();

            var dbCourse = await _dbContext.Courses
                .Where(dc => !dc.IsDeleted && dc.Id == id).Include(c => c.Category)
                .FirstOrDefaultAsync();

            if (dbCourse is null) return NotFound();

            var selectedCategory = new List<SelectListItem>();

            dbCategories.ForEach(c => selectedCategory.Add(new SelectListItem(c.Name, c.Id.ToString())));

            var courseUpdateViewModel = new CourseUpdateViewModel
            {
                Id = dbCourse.Id,
                ImageUrl = dbCourse.ImageUrl,
                Title = dbCourse.Title,
                Description = dbCourse.Description,
                About = dbCourse.About,
                HowToApply = dbCourse.HowToApply,
                Certification = dbCourse.Certification,
                Duration = dbCourse.Duration,
                StartDate = dbCourse.StartDate,
                EndDate = dbCourse.EndDate,
                ClassDuration = dbCourse.ClassDuration,
                SkillLevel = dbCourse.SkillLevel,
                Language = dbCourse.Language,
                StudentCount = dbCourse.StudentCount,
                Assesments = dbCourse.Assesments,
                CourseFee = dbCourse.CourseFee,
                Categories = selectedCategory,
                CategoryId = dbCourse.CategoryId
            };

            return View(courseUpdateViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, CourseUpdateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (id is null) return BadRequest();
            var dbCategories = await _dbContext.Categories
                .Where(c => !c.IsDeleted)
                .Include(c => c.Courses)
                .ToListAsync();

            if (dbCategories is null) return NotFound();

            var dbCourse = await _dbContext.Courses
                .Where(dc => !dc.IsDeleted && dc.Id == id).Include(c => c.Category)
                .FirstOrDefaultAsync();

            if (dbCourse is null) return NotFound();



            if (DateTime.Compare(DateTime.UtcNow.AddHours(4), model.StartDate) >= 0)
            {
                ModelState.AddModelError("StartDate", "Start Date must be future and earlier than End Date");
                return View(model);
            }

            if (DateTime.Compare(DateTime.UtcNow.AddHours(4), model.EndDate) >= 0)
            {
                ModelState.AddModelError("EndDate", "End Date must be future and after Start Date");
                return View(model);
            }

            if (DateTime.Compare(model.StartDate, model.EndDate) >= 0)
            {
                ModelState.AddModelError("", "Start Date must be earlier than End Date");
                return View(model);
            }

            if (model.Image is not null)
            {

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

                if (dbCourse.ImageUrl is null) return NotFound();

                var courseImagePath = Path.Combine(Constants.RootPath, "assets", "img", "course", dbCourse.ImageUrl);

                if (System.IO.File.Exists(courseImagePath))
                    System.IO.File.Delete(courseImagePath);

                var unicalName = await model.Image.Generatefile(Constants.CoursePath);

                dbCourse.ImageUrl = unicalName;
            }

            var selectedCategoryUpdate = new CourseUpdateViewModel
            {
                CategoryId = model.CategoryId
            };

            dbCourse.Title = model.Title;
            dbCourse.Description = model.Description;
            dbCourse.Duration = model.Duration;
            dbCourse.StartDate = model.StartDate;
            dbCourse.EndDate = model.EndDate;
            dbCourse.ClassDuration = model.ClassDuration;
            dbCourse.CourseFee = model.CourseFee;
            dbCourse.Assesments = model.Assesments;
            dbCourse.About = model.About;
            dbCourse.Certification = model.Certification;
            dbCourse.HowToApply = model.HowToApply;
            dbCourse.SkillLevel = model.SkillLevel;
            dbCourse.Language = model.Language;
            dbCourse.StudentCount = model.StudentCount;
            dbCourse.CategoryId = model.CategoryId;

            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();

            var dbCourse = await _dbContext.Courses.FindAsync(id);

            if (dbCourse is null) return NotFound();

            if (dbCourse.Id != id) return BadRequest();

            var courseImagePath = Path.Combine(Constants.RootPath, "assets", "img", "speaker", dbCourse.ImageUrl);

            if (System.IO.File.Exists(courseImagePath))
                System.IO.File.Delete(courseImagePath);

            _dbContext.Courses.Remove(dbCourse);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
