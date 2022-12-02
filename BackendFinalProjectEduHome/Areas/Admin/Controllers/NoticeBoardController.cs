using BackendFinalProjectEduHome.Areas.Admin.ViewModels;
using BackendFinalProjectEduHome.DAL;
using BackendFinalProjectEduHome.DAL.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendFinalProjectEduHome.Areas.Admin.Controllers
{
    public class NoticeBoardController : BaseController
    {
        private readonly EduHomeDbContext _dbContext;

        public NoticeBoardController(EduHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<NoticeBoard> dbNoticeBoard = await _dbContext.NoticeBoards
                .Where(w => !w.IsDeleted)
                .OrderByDescending(w => w.Id)
                .ToListAsync();

            return View(dbNoticeBoard);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NoticeBoardCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var noticeBoards = new NoticeBoard
            {
                NoticeTitle = model.NoticeTitle,
                NoticeDescription = model.NoticeDescription,
                VideoUrl = model.VideoUrl,
            };

            await _dbContext.NoticeBoards.AddAsync(noticeBoards);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return BadRequest();

            var dbNoticeBoard = await _dbContext.NoticeBoards
               .Where(nb => nb.Id == id)
               .FirstOrDefaultAsync();

            if (dbNoticeBoard is null) return NotFound();

            var noticeBoardUpdateViewModel = new NoticeBoardUpdateViewModel
            {
                Id = dbNoticeBoard.Id,
                NoticeTitle = dbNoticeBoard.NoticeTitle,
                VideoUrl = dbNoticeBoard.VideoUrl,
                NoticeDescription = dbNoticeBoard.NoticeDescription,
            };

            return View(noticeBoardUpdateViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, NoticeBoardUpdateViewModel model)
        {
            if (id is null) return BadRequest();

            var dbNoticeBoard = await _dbContext.NoticeBoards
               .Where(nb => nb.Id == id)
               .FirstOrDefaultAsync();

            if (dbNoticeBoard is null) return NotFound(); 

            dbNoticeBoard.NoticeTitle = model.NoticeTitle;  
            dbNoticeBoard.VideoUrl = model.VideoUrl;
            dbNoticeBoard.NoticeDescription = model.NoticeDescription;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var dbNoticeBoard = await _dbContext.NoticeBoards.FirstOrDefaultAsync(nb => nb.Id == id);

            if (dbNoticeBoard == null) return NotFound();

            if (dbNoticeBoard.Id != id) return BadRequest();

            _dbContext.NoticeBoards.Remove(dbNoticeBoard);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
