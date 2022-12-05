using BackendFinalProjectEduHome.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendFinalProjectEduHome.ViewComponents
{
    public class TeacherViewComponent : ViewComponent
    {
        private readonly EduHomeDbContext _dbContext;

        public TeacherViewComponent(EduHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync(int cut)
        {
            var dbTeachrs =  _dbContext.Teachers;

            if (cut == 0)
            {
                return View(await dbTeachrs.Where(t => !t.IsDeleted).ToListAsync());
            }

            return View(await dbTeachrs.Take(cut).ToListAsync());
        }
    }
}
