using BackendFinalProjectEduHome.DAL;
using BackendFinalProjectEduHome.DAL.Entities;
using BackendFinalProjectEduHome.DAL.Entity;
using BackendFinalProjectEduHome.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var contactSettings = await _dbContext.Settings.Where(s => !s.IsDeleted).FirstOrDefaultAsync();

            var settings = new ContactSettingsViewModel
            {
                Phone = contactSettings.Phone,
                PhoneImage = contactSettings.PhoneImage,
                Address = contactSettings.Address,
                AdressImage = contactSettings.AdressImage,
                WebSite = contactSettings.WebSite,
                WebsiteImage = contactSettings.WebsiteImage,
            };


            var model = new ContactViewModel
            {
                ContactSettings = settings,
                ContactMessage = new(),
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessage(ContactMessageViewModel contactMessage)
        {
            if (!ModelState.IsValid) return View(viewName: nameof(Index), contactMessage);
            var message = new ContactMessage
            {
                Name = contactMessage.Name,
                Email = contactMessage.Email,
                Subject = contactMessage.Subject,
                Message = contactMessage.Message,
                IsRead = false
            };

            await _dbContext.ContactMessages.AddAsync(message);

            await _dbContext.SaveChangesAsync();  
            
            return RedirectToAction(nameof(Index));
        }
    }
}
