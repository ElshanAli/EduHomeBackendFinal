using BackendFinalProjectEduHome.DAL;
using BackendFinalProjectEduHome.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BackendFinalProjectEduHome.Controllers
{
    public class HomeController : Controller
    {
        private readonly EduHomeDbContext _dbContext;
        public HomeController(EduHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var slider = await _dbContext.Sliders.ToListAsync();
            var courses = await _dbContext.Courses.ToListAsync();
            var homeViewModel = new HomeViewModel
            {
                Sliders = slider,
                Courses = courses
            };
            return View(homeViewModel);
        }
    }
}