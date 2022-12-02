using BackendFinalProjectEduHome.Areas.Admin.Data;
using BackendFinalProjectEduHome.Areas.Admin.ViewModels;
using BackendFinalProjectEduHome.DAL;
using BackendFinalProjectEduHome.DAL.Entity;
using BackendFinalProjectEduHome.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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

            //if (model.StartDate.ToString("yyyy") != model.EndDate.ToString("yyyy")
            //    || model.StartDate.ToString("MM") != model.EndDate.ToString("MM")
            //    || model.StartDate.ToString("dd") != model.EndDate.ToString("dd"))
            //{
            //    ModelState.AddModelError("", "Start Date and End Date must be same day");
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

            foreach (int speakerId in model.SpeakerIds)
            {
                if (!await _dbContext.Speakers.AnyAsync(s => s.Id == speakerId))
                {
                    ModelState.AddModelError("", "Incorect Speaker");
                    return View();
                }

                eventSpeakers.Add(new EventSpeaker
                {
                    SpeakerId = speakerId,
                });

            }

            var speakers = await _dbContext.Speakers.Where(s => !s.IsDeleted).ToListAsync();

            var eventSpeakersSelectList = new List<SelectListItem>();
            speakers.ForEach(c => eventSpeakersSelectList
            .Add(new SelectListItem(c.Firstname + " " + c.Lastname, c.Id.ToString())));

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

            if (dbEvent is null) return NotFound();

            if (!ModelState.IsValid) return View(model);

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

            //if (model.StartDate.ToString("yyyy") != model.EndDate.ToString("yyyy")
            //    || model.StartDate.ToString("MM") != model.EndDate.ToString("MM")
            //    || model.StartDate.ToString("dd") != model.EndDate.ToString("dd"))
            //{
            //    ModelState.AddModelError("", "Start Date and End Date must be same day");
            //    return View(model);
            //}

            var speakers = await _dbContext.Speakers.Where(s => !s.IsDeleted).ToListAsync();

            var eventSpeakersSelectList = new List<SelectListItem>();

            if (model.SpeakerIds.Count > 0)
            {
                foreach (int speakerId in model.SpeakerIds)
                {
                    if (!await _dbContext.Speakers.AnyAsync(c => c.Id == speakerId))
                    {
                        ModelState.AddModelError("", "Has been selected incorrect speaker.");
                        return View(model);
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
                return View(model);
                //_dbContext.EventSpeakers.RemoveRange(dbEvent.EventSpeakers);
            }

            if (model.Image != null)
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
