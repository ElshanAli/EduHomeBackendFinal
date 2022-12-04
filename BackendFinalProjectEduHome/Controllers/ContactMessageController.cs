using BackendFinalProjectEduHome.DAL;
using BackendFinalProjectEduHome.DAL.Entities;
using BackendFinalProjectEduHome.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BackendFinalProjectEduHome.Controllers
{
    public class ContactMessageController : Controller
    {
        private readonly EduHomeDbContext _dbContext;
        private readonly UserManager<User> _userManager;

        public ContactMessageController(EduHomeDbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var model = new ContactViewModel();

            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);

                model.ContactMessage = new ContactMessageViewModel
                {
                    Name = model.ContactMessage.Name,
                    Email = model.ContactMessage.Email,
                    Subject = model.ContactMessage.Subject,
                    Message = model.ContactMessage.Message,
                };
            }

            return View(model);
        }
    }
}
