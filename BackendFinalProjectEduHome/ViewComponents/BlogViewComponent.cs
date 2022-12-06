using BackendFinalProjectEduHome.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace BackendFinalProjectEduHome.ViewComponents
{
    public class BlogViewComponent : ViewComponent
    {
        private readonly EduHomeDbContext _dbContext;

        public BlogViewComponent(EduHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var dbBlog = await _dbContext.Blogs.Where(b => !b.IsDeleted).ToListAsync();
            return View(dbBlog);
        }
    }
}
