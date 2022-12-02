using BackendFinalProjectEduHome.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BackendFinalProjectEduHome.Controllers
{
    public class ContactMessageController : Controller
    {
        public IActionResult Index()
        {
            var model = new ContactViewModel();

            model.ContactMessage = new ContactMessageViewModel
            {
                Name = model.ContactMessage.Name,
                Email = model.ContactMessage.Email,
                Subject = model.ContactMessage.Subject,
                Message = model.ContactMessage.Message,
            };

            return View(model);
        }
    }
}
