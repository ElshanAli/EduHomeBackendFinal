using Microsoft.AspNetCore.Mvc;

namespace BackendFinalProjectEduHome.Areas.Admin.Controllers
{
    public class DashboardController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
