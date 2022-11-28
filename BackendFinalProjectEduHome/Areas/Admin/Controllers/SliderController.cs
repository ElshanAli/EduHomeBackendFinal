using BackendFinalProjectEduHome.Areas.Admin.Data;
using BackendFinalProjectEduHome.Areas.ViewModels;
using BackendFinalProjectEduHome.DAL;
using BackendFinalProjectEduHome.DAL.Entity;
using BackendFinalProjectEduHome.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace BackendFinalProjectEduHome.Areas.Admin.Controllers
{
    public class SliderController : BaseController
    {
        private readonly EduHomeDbContext _dbContext;
        public SliderController(EduHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Slider> sliders = await _dbContext.Sliders
                .Where(sl => !sl.IsDeleted)
                .OrderByDescending(e => e.Id)
                .ToListAsync();

            return View(sliders);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderCreateViewModel model)
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

            var unicalName = await model.Image.Generatefile(Constants.SliderPath);

            var slider = new Slider
            {
                Title = model.Title,
                Subtitle = model.Subtitle,
                BtnSrc = model.BtnSrc,
                BtnText = model.BtnText,
                ImageUrl = unicalName,
            };
            await _dbContext.Sliders.AddAsync(slider);

            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return BadRequest();

            var slider = await _dbContext.Sliders
                .Where(sl => sl.Id == id)
                .FirstOrDefaultAsync();

            if (slider is null) return NotFound();

            var sliderViewModel = new SliderUpdateViewModel
            {
                Id = slider.Id,
                Title = slider.Title,
                Subtitle = slider.Subtitle,
                BtnSrc=slider.BtnSrc,
                BtnText=slider.BtnText,
                ImageUrl = slider.ImageUrl
            };

            return View(sliderViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, SliderUpdateViewModel model)
        {
            if(id is null) return BadRequest();

            var slider = await _dbContext.Sliders
                .Where(sl => sl.Id == id)
                .FirstOrDefaultAsync();

            if(slider is null) return NotFound();         

            if (!ModelState.IsValid) return View(new SliderUpdateViewModel
            {   
                ImageUrl = slider.ImageUrl    
            });

            if (model.Image != null)
            {
                if (!model.Image.IsImage())
                {
                    ModelState.AddModelError("", "Must be selected image");
                    return View(new SliderUpdateViewModel
                    {                       
                        ImageUrl = slider.ImageUrl,                       
                    });
                }

                if (!model.Image.IsAllowedSize(7))
                {
                    ModelState.AddModelError("", "Image size can be max 7 mb");
                    return View(model);
                }

                if (slider.ImageUrl is null) return NotFound();

                var sliderImagePath = Path.Combine(Constants.RootPath, "assets", "img", "slider", slider.ImageUrl);

                if (System.IO.File.Exists(sliderImagePath))
                    System.IO.File.Delete(sliderImagePath);

                var unicalName = await model.Image.Generatefile(Constants.SliderPath);
                slider.ImageUrl = unicalName;
                
            }

            slider.Title = model.Title;           
            slider.Subtitle = model.Subtitle;
            slider.BtnText = model.BtnText;
            slider.BtnSrc = model.BtnSrc;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var slider = await _dbContext.Sliders.FirstOrDefaultAsync(s => s.Id == id);

            if (slider == null) return NotFound();

            if (slider.ImageUrl == null) return NotFound();

            if (slider.Id != id) return BadRequest();

            var sliderImagePath = Path.Combine(Constants.RootPath, "assets", "img", "slider", slider.ImageUrl);

            if (System.IO.File.Exists(sliderImagePath))
                System.IO.File.Delete(sliderImagePath);

            _dbContext.Sliders.Remove(slider);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }

  
}
