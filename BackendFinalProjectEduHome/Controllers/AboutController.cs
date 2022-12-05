using BackendFinalProjectEduHome.DAL;
using BackendFinalProjectEduHome.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendFinalProjectEduHome.Controllers
{
    public class AboutController : Controller
    {
        private readonly EduHomeDbContext _dbContext;

        public AboutController(EduHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {


            var dbNoticeBoard = await _dbContext.NoticeBoards.Where(n => !n.IsDeleted).FirstOrDefaultAsync();
            var dbWelcomeEdu = await _dbContext.WelcomeEdu.Where(w => !w.IsDeleted).FirstOrDefaultAsync();
            AboutViewModel model = new AboutViewModel()
            {
                NoticeBoards = dbNoticeBoard,
                WelcomeEdu = dbWelcomeEdu
            };


            return View(model);
        }
    }
}
