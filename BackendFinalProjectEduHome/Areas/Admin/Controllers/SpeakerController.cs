using BackendFinalProjectEduHome.Areas.Admin.ViewModels;
using BackendFinalProjectEduHome.DAL.Entity;
using Microsoft.AspNetCore.Mvc;

namespace BackendFinalProjectEduHome.Areas.Admin.Controllers
{
    public class SpeakerController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SpeakerCreateViewModel model)
        {
            return View();
        }
    }
}
