using BackendFinalProjectEduHome.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace BackendFinalProjectEduHome.ViewComponents
{
    public class EventForHomePageViewComponent : ViewComponent
    {
        private readonly EduHomeDbContext _dbContext;

        public EventForHomePageViewComponent(EduHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var dbEvent = await _dbContext.Events.Where(e => !e.IsDeleted).OrderByDescending(e => e.Id).ToListAsync();
            return View(dbEvent);
        }
    }
}
