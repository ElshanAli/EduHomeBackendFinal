using BackendFinalProjectEduHome.DAL;
using BackendFinalProjectEduHome.ViewModels;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index()
        {
            var slider = _dbContext.Sliders.ToList();
            var homeViewModel = new HomeViewModel
            {
                Sliders = slider,
            };
            return View(homeViewModel);
        }
    }
}