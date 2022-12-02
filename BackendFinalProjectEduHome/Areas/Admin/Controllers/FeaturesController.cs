using BackendFinalProjectEduHome.DAL;
using Microsoft.AspNetCore.Mvc;

namespace BackendFinalProjectEduHome.Areas.Admin.Controllers
{
    public class FeaturesController : Controller
    {
        private readonly EduHomeDbContext _dbContext;

        public FeaturesController(EduHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
