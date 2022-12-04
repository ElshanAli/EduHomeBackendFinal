using Microsoft.AspNetCore.Mvc;

namespace BackendFinalProjectEduHome.Controllers
{
    public class CourseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
