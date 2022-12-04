using BackendFinalProjectEduHome.Areas.Admin.ViewModels;
using BackendFinalProjectEduHome.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendFinalProjectEduHome.Areas.Admin.ViewComponents
{
    public class ContactMessageViewComponent : ViewComponent
    {
        private readonly EduHomeDbContext _dbContext;

        public ContactMessageViewComponent(EduHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var message = await _dbContext.ContactMessages.ToListAsync();

            var isAllReadMessage = message.All(x => x.IsRead);

            return View(new ContactMessageReadViewModel
            {
                ContactMessages = message,
                IsAllReadMessage = isAllReadMessage
            });
        }
    }
}
