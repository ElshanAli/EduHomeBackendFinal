using BackendFinalProjectEduHome.Areas.Admin.Data;
using BackendFinalProjectEduHome.Areas.Admin.ViewModels;
using BackendFinalProjectEduHome.DAL;
using BackendFinalProjectEduHome.DAL.Entity;
using BackendFinalProjectEduHome.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendFinalProjectEduHome.Areas.Admin.Controllers
{
    public class EventController : BaseController
    {
        private readonly EduHomeDbContext _dbContext;

        public EventController(EduHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Event> events = await _dbContext.Events
                .Include(e => e.EventSpeakers).ThenInclude(es => es.Speaker)
                .Where(e => !e.IsDeleted)
                .ToListAsync();

            return View(events);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (DateTime.Compare(DateTime.UtcNow.AddHours(4), model.StartDate) >= 0)
            {
                ModelState.AddModelError("StartDate", "Error");
                return View(model);
            }

            if (DateTime.Compare(DateTime.UtcNow.AddHours(4), model.EndDate) >= 0)
            {
                ModelState.AddModelError("EndDate", "Error");
                return View(model);
            }

            if (DateTime.Compare(model.StartDate, model.EndDate) >= 0)
            {
                ModelState.AddModelError("", "Error");
                return View(model);
            }

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

            var unicalName = await model.Image.Generatefile(Constants.EventPath);

            var events = new Event
            {
                Title = model.Title,
                Description = model.Description,
                ImageUrl = unicalName,
                Address = model.Address,
                EndDate = model.EndDate,
                StartDate = model.StartDate,
                EventSpeakers = new List<EventSpeaker>()
            };

            await _dbContext.Events.AddAsync(events); 
            await _dbContext.SaveChangesAsync();

            return View();
        }
    }
}
