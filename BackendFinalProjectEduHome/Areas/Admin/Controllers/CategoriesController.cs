using BackendFinalProjectEduHome.Areas.Admin.ViewModels;
using BackendFinalProjectEduHome.DAL;
using BackendFinalProjectEduHome.DAL.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendFinalProjectEduHome.Areas.Admin.Controllers
{
    public class CategoriesController : BaseController
    {
        private readonly EduHomeDbContext _dbContext;

        public CategoriesController(EduHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _dbContext.Categories
                .Where(c => !c.IsDeleted)
                .Include(c => c.Courses)
                .OrderByDescending(c => c.Id)
                .ToListAsync();

            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoriesCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var categories = await _dbContext.Categories
                .FirstOrDefaultAsync(c => c.Name.ToLower().Trim() == model.Name.ToLower().Trim());

            if (categories != null)
            {
                ModelState.AddModelError("", "This category name already exists");
                return View(model);
            }
            else
            {
                var category = new Category
                {
                    Name = model.Name
                };

                await _dbContext.Categories.AddAsync(category);
                await _dbContext.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return BadRequest();

            var categories = await _dbContext.Categories
                .Where(c => !c.IsDeleted && c.Id == id).FirstOrDefaultAsync();

            if (categories is null) return NotFound();

            var categoryViewModel = new CategoriesUpdateViewModel
            {
                CategoryId = categories.Id,
                Name = categories.Name
            };

            return View(categoryViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, CategoriesUpdateViewModel model)
        {
            if (id is null) return BadRequest();

            if (!ModelState.IsValid) return View(model);

            var categories = await _dbContext.Categories.FindAsync(id);

            if (categories is null) return NotFound();

            var existCategories = await _dbContext.Categories
                .FirstOrDefaultAsync(c => c.Id != id && c.Name.ToLower().Trim() == model.Name.ToLower().Trim());

            if (existCategories is not null)
            {
                ModelState.AddModelError("", "This category name already exists");
                return View(model);

            }
            categories.Name = model.Name;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();

            var categories = await _dbContext.Categories.FindAsync(id);

            if (categories is null) return NotFound();

            if (categories.Id != id) return BadRequest();

            _dbContext.Categories.Remove(categories);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
