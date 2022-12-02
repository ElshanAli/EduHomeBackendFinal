using BackendFinalProjectEduHome.Areas.Admin.Data;
using BackendFinalProjectEduHome.Areas.Admin.ViewModels;
using BackendFinalProjectEduHome.DAL;
using BackendFinalProjectEduHome.DAL.Entity;
using BackendFinalProjectEduHome.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendFinalProjectEduHome.Areas.Admin.Controllers
{
    public class WelcomeEduController : BaseController
    {
        private readonly EduHomeDbContext _dbContext;

        public WelcomeEduController(EduHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<WelcomeEdu> dbWelcomeEdu = await _dbContext.WelcomeEdu
                .Where(w => !w.IsDeleted)
                .OrderByDescending(w => w.Id)
                .ToListAsync();

            return View(dbWelcomeEdu);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WelcomeEduCreateViewModel model)
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

            var unicalName = await model.Image.Generatefile(Constants.WelcomeEduPath);

            var welcomeEdu = new WelcomeEdu
            {
                Title = model.Title,
                Description = model.Description,
                ImageUrl = unicalName
            };

            await _dbContext.WelcomeEdu.AddAsync(welcomeEdu);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return BadRequest();

            var welcomeEdus = await _dbContext.WelcomeEdu
                .Where(we => we.Id == id)
                .FirstOrDefaultAsync();

            if (welcomeEdus is null) return NotFound();

            var welcomeEduUpdateViewModel = new WelcomeEduUpdateViewModel
            {
                Id = welcomeEdus.Id,
                Title = welcomeEdus.Title,
                ImageUrl = welcomeEdus.ImageUrl,
                Description = welcomeEdus.Description,
            };

            return View(welcomeEduUpdateViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, WelcomeEduUpdateViewModel model)
        {
            if (id is null) return BadRequest();

            var welcomeEdus = await _dbContext.WelcomeEdu
               .Where(we => we.Id == id)
               .FirstOrDefaultAsync();

            if (welcomeEdus is null) return NotFound();

            if (!ModelState.IsValid) return View(new WelcomeEduUpdateViewModel
            {
                ImageUrl = welcomeEdus.ImageUrl
            });

            if (model.Image != null)
            {
                if (!model.Image.IsImage())
                {
                    ModelState.AddModelError("", "Must be selected image");
                    return View(new WelcomeEduUpdateViewModel
                    {
                        ImageUrl = welcomeEdus.ImageUrl,
                    });
                }

                if (!model.Image.IsAllowedSize(7))
                {
                    ModelState.AddModelError("", "Image size can be max 7 mb");
                    return View(model);
                }
                if (welcomeEdus.ImageUrl is null) return NotFound();

                var welcomeEduPath = Path.Combine(Constants.RootPath, "assets", "img", "welcomeedu", welcomeEdus.ImageUrl);

                if (System.IO.File.Exists(welcomeEduPath))
                    System.IO.File.Delete(welcomeEduPath) ;

                var unicalName = await model.Image.Generatefile(Constants.WelcomeEduPath);
                welcomeEdus.ImageUrl = unicalName;
            }

            welcomeEdus.Title = model.Title;
            welcomeEdus.Description = model.Description;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var dbWelcomeEdus = await _dbContext.WelcomeEdu.FirstOrDefaultAsync(we => we.Id == id);

            if (dbWelcomeEdus == null) return NotFound();

            if (dbWelcomeEdus.ImageUrl == null) return NotFound();

            if (dbWelcomeEdus.Id != id) return BadRequest();

            var welcomeEduImagePath = Path.Combine(Constants.RootPath, "assets", "img", "welcomeedu", dbWelcomeEdus.ImageUrl);

            if (System.IO.File.Exists(welcomeEduImagePath))
                System.IO.File.Delete(welcomeEduImagePath);

            _dbContext.WelcomeEdu.Remove(dbWelcomeEdus);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
