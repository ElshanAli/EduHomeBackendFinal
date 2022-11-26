using Microsoft.AspNetCore.Mvc;

namespace BackendFinalProjectEduHome.Controllers
{
    public class TeacherController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
