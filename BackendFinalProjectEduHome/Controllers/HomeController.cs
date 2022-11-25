using BackendFinalProjectEduHome.DAL;
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
            return View();
        }
    }
}