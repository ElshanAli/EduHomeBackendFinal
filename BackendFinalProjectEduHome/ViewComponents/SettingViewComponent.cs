using BackendFinalProjectEduHome.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendFinalProjectEduHome.ViewComponents
{
    public class SettingViewComponent : ViewComponent
    {
        private readonly EduHomeDbContext _dbContext;

        public SettingViewComponent(EduHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var dbSettings = await _dbContext.Settings.Where(s => !s.IsDeleted).FirstOrDefaultAsync();

            return View(dbSettings);
        }
    }
}
