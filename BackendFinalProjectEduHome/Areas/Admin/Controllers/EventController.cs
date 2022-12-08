using BackendFinalProjectEduHome.Areas.Admin.Data;
using BackendFinalProjectEduHome.Areas.Admin.ViewModels;
using BackendFinalProjectEduHome.DAL;
using BackendFinalProjectEduHome.DAL.Entity;
using BackendFinalProjectEduHome.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Logging;

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
                .OrderByDescending(e => e.Id)
                .ToListAsync();

            return View(events);
        }

        public async Task<IActionResult> Create()
        {
            var speakers = await _dbContext.Speakers.ToListAsync();

            var eventSpeakersSelectList = new List<SelectListItem>();

            speakers.ForEach(c => eventSpeakersSelectList
            .Add(new SelectListItem(c.Firstname + " " + c.Lastname, c.Id.ToString())));

            var model = new EventCreateViewModel
            {
                Speakers = eventSpeakersSelectList
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (model.StartDate.ToString("yyyy") != model.EndDate.ToString("yyyy")
                || model.StartDate.ToString("MM") != model.EndDate.ToString("MM")
                || model.StartDate.ToString("dd") != model.EndDate.ToString("dd"))
            {
                ModelState.AddModelError("", "Start Date and End Date must be same day");
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
            };

            List<EventSpeaker> eventSpeakers = new List<EventSpeaker>();

            var speakers = await _dbContext.Speakers.Where(s => !s.IsDeleted).ToListAsync();

            var eventSpeakersSelectList = new List<SelectListItem>();

           
            speakers.ForEach(c => eventSpeakersSelectList
            .Add(new SelectListItem(c.Firstname + " " + c.Lastname, c.Id.ToString())));

            var viewModel = new EventCreateViewModel
            {

                Speakers=eventSpeakersSelectList,
                Image=model.Image
            };

            foreach (int speakerId in model.SpeakerIds)
            {
                if (!await _dbContext.Speakers.AnyAsync(s => s.Id == speakerId))
                {
                    ModelState.AddModelError("", "Incorect Speaker");
                    return View(viewModel);
                }

                eventSpeakers.Add(new EventSpeaker
                {
                    SpeakerId = speakerId,
                });

            }

            events.EventSpeakers = eventSpeakers;
            model.Speakers = eventSpeakersSelectList;

            await _dbContext.Events.AddAsync(events);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return BadRequest();

            var dbEvent = await _dbContext.Events
                .Where(e => !e.IsDeleted && e.Id == id)
                .Include(e => e.EventSpeakers).ThenInclude(es => es.Speaker)
                
                .FirstOrDefaultAsync();

            if (dbEvent is null) return NotFound();

            var speakers = await _dbContext.Speakers.Where(s => !s.IsDeleted).ToListAsync();

            var eventSpeakersSelectList = new List<SelectListItem>();

            speakers.ForEach(c => eventSpeakersSelectList
            .Add(new SelectListItem(c.Firstname + " " + c.Lastname, c.Id.ToString())));

            List<EventSpeaker> eventSpeakers = new List<EventSpeaker>();

            foreach (EventSpeaker eventSpeaker in dbEvent.EventSpeakers)
            {
                if (!await _dbContext.Speakers.AnyAsync(s => s.Id == eventSpeaker.SpeakerId))
                {
                    ModelState.AddModelError("", "Incorect Speaker Id");
                    return View();
                }

                eventSpeakers.Add(new EventSpeaker
                {
                    SpeakerId = eventSpeaker.SpeakerId
                });
            }

            var eventViewModel = new EventUpdateViewModel
            {
                Id = dbEvent.Id,
                StartDate = dbEvent.StartDate,
                EndDate = dbEvent.EndDate,
                Title = dbEvent.Title,
                Description = dbEvent.Description,
                ImageUrl = dbEvent.ImageUrl,
                Address = dbEvent.Address,
                Speakers = eventSpeakersSelectList,
                SpeakerIds = eventSpeakers.Select(s => s.SpeakerId).ToList()
            };

            return View(eventViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, EventUpdateViewModel model)
        {

            if (id is null) return BadRequest();

            var dbEvent = await _dbContext.Events
                .Include(e => e.EventSpeakers).ThenInclude(es => es.Speaker)
                .Where(e => !e.IsDeleted && e.Id == id)
                .FirstOrDefaultAsync();
            var eventSpeakersSelectList = new List<SelectListItem>();

            var speakers = await _dbContext.Speakers.Where(s => !s.IsDeleted).ToListAsync();

            var viewModel = new EventUpdateViewModel
            {
                ImageUrl = dbEvent.ImageUrl,
                Speakers = eventSpeakersSelectList,

            };
            speakers.ForEach(e => eventSpeakersSelectList
            .Add(new SelectListItem(e.Firstname+" "+e.Lastname, e.Id.ToString())));

            if (dbEvent is null) return NotFound();

            if (!ModelState.IsValid) return View(viewModel);            

            if (model.StartDate.ToString("yyyy") != model.EndDate.ToString("yyyy")
                || model.StartDate.ToString("MM") != model.EndDate.ToString("MM")
                || model.StartDate.ToString("dd") != model.EndDate.ToString("dd"))
            {
                ModelState.AddModelError("", "Start Date and End Date must be same day");
                return View(viewModel);
            }

            if (model.SpeakerIds.Count > 0)
            {
                foreach (int speakerId in model.SpeakerIds)
                {
                    if (!await _dbContext.Speakers.AnyAsync(c => c.Id == speakerId))
                    {
                        ModelState.AddModelError("", "Has been selected incorrect speaker.");
                        return View(viewModel);
                    }
                }

                _dbContext.EventSpeakers.RemoveRange(dbEvent.EventSpeakers);

                List<EventSpeaker> eventSpeakers = new List<EventSpeaker>();
                foreach (var item in model.SpeakerIds)
                {
                    EventSpeaker eventSpeaker = new EventSpeaker
                    {
                        SpeakerId = item
                    };

                    eventSpeakers.Add(eventSpeaker);
                }

                dbEvent.EventSpeakers = eventSpeakers;
            }
            else
            {
                ModelState.AddModelError("", "Select minimum 1 speaker");
                return View(viewModel);
            }

            if (model.Image != null)
            {
                if (!model.Image.IsImage())
                {
                    ModelState.AddModelError("", "Must be selected image");
                    return View(viewModel);
                }

                if (!model.Image.IsAllowedSize(7))
                {
                    ModelState.AddModelError("", "Image size can be max 7 mb");
                    return View(viewModel);
                }

                if (dbEvent.ImageUrl is null) return NotFound();
               

                var eventImagePath = Path.Combine(Constants.RootPath, "assets", "img", "event", dbEvent.ImageUrl);

                if (System.IO.File.Exists(eventImagePath))
                    System.IO.File.Delete(eventImagePath);

                var unicalName = await model.Image.Generatefile(Constants.EventPath);
                dbEvent.ImageUrl = unicalName;
            }

            dbEvent.StartDate = model.StartDate;
            dbEvent.EndDate = model.EndDate;
            dbEvent.Description = model.Description;
            dbEvent.Address = model.Address;
            dbEvent.Title = model.Title;

            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return NotFound();

            var events = await _dbContext.Events
                .Where(c => !c.IsDeleted && c.Id == id)
                .Include(cat => cat.EventSpeakers).ThenInclude(s=>s.Speaker)
                .FirstOrDefaultAsync();

            if (events is null) return NotFound();

            return View(events);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();

            var existevent = await _dbContext.Events
                  .Include(e => e.EventSpeakers).ThenInclude(es => es.Speaker)
                  .FirstOrDefaultAsync(e => e.Id == id);

            if (existevent is null) return NotFound();

            if (existevent.ImageUrl is null) return NotFound();

            if (existevent.Id != id) return BadRequest();

            var eventImagePath = Path.Combine(Constants.RootPath, "assets", "img", "event", existevent.ImageUrl);

            if (System.IO.File.Exists(eventImagePath))
                System.IO.File.Delete(eventImagePath);

            _dbContext.Events.Remove(existevent);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
