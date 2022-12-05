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
    public class SpeakerController : BaseController
    {
        private readonly EduHomeDbContext _dbContext;

        public SpeakerController(EduHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Speaker> speakers = await _dbContext.Speakers.Include(s=>s.EventSpeakers).ThenInclude(s=>s.Event)
                .OrderByDescending(sp => sp.Id)
                .ToListAsync();

            return View(speakers);
        }

        public  IActionResult Create() 
        {          
            return View();        
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SpeakerCreateViewModel model)
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

            var unicalName = await model.Image.Generatefile(Constants.SpeakerPath);

            var speakers = new Speaker
            {
                Firstname = model.Firstname,
                Lastname = model.Lastname,
                ImageUrl = unicalName,
                Position = model.Position,
                Company = model.Company
            };

            await _dbContext.Speakers.AddAsync(speakers);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return BadRequest();

            var speakers = await _dbContext.Speakers
                .Where(sp => sp.Id == id)
                .FirstOrDefaultAsync();

            if (speakers == null) return NotFound();

            var speakerViewModel = new SpeakerUpdateViewModel
            {
                Id = speakers.Id,
                Firstname = speakers.Firstname,
                Lastname = speakers.Lastname,
                ImageUrl = speakers.ImageUrl,
                Position = speakers.Position,
                Company = speakers.Company
            };

            return View(speakerViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, SpeakerUpdateViewModel model)
        {
            if (id is null) return BadRequest();

            var speakers = await _dbContext.Speakers
                .Where(sp => sp.Id == id)               
                .FirstOrDefaultAsync();

            if (speakers == null) return NotFound();

            if (!ModelState.IsValid) return View(new SpeakerUpdateViewModel
            {
                ImageUrl = speakers.ImageUrl
            });

            if (model.Image != null)
            {
                if (!model.Image.IsImage())
                {
                    ModelState.AddModelError("", "Must be selected image");
                    return View(new BlogUpdateViewModel
                    {
                        ImageUrl = speakers.ImageUrl,
                    });
                }

                if (!model.Image.IsAllowedSize(7))
                {
                    ModelState.AddModelError("", "Image size can be max 7 mb");
                    return View(model);
                }

                if (speakers.ImageUrl is null) return NotFound();

                var speakerImagePath = Path.Combine(Constants.RootPath, "assets", "img", "speaker", speakers.ImageUrl);

                if (System.IO.File.Exists(speakerImagePath))
                    System.IO.File.Delete(speakerImagePath);

                var unicalName = await model.Image.Generatefile(Constants.SpeakerPath);
                speakers.ImageUrl = unicalName;               
            }

            speakers.Firstname = model.Firstname;
            speakers.Lastname = model.Lastname;
            speakers.Position = model.Position;
            speakers.Company = model.Company;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var speakers = await _dbContext.Speakers
                .FirstOrDefaultAsync(sp => sp.Id == id);

            if (speakers == null) return NotFound();

            if (speakers.ImageUrl == null) return NotFound();

            if (speakers.Id != id) return BadRequest();

            var speakerImagePath = Path.Combine(Constants.RootPath, "assets", "img", "speaker", speakers.ImageUrl);

            if (System.IO.File.Exists(speakerImagePath))
                System.IO.File.Delete(speakerImagePath);

            _dbContext.Speakers.Remove(speakers);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
