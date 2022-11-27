using BackendFinalProjectEduHome.Areas.Admin.Data;
using BackendFinalProjectEduHome.Areas.Admin.ViewModels;
using BackendFinalProjectEduHome.Areas.ViewModels;
using BackendFinalProjectEduHome.DAL;
using BackendFinalProjectEduHome.DAL.Entity;
using BackendFinalProjectEduHome.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using System.Net.Mail;

namespace BackendFinalProjectEduHome.Areas.Admin.Controllers
{
    public class TeacherController : BaseController
    {
        private readonly EduHomeDbContext _dbContext;

        public TeacherController(EduHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Teacher> teachers = await _dbContext.Teachers
                .Where(t => !t.IsDeleted)
                .OrderByDescending(e => e.Id)
                .ToListAsync();

            return View(teachers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeacherCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (!model.Image.IsImage())
            {
                ModelState.AddModelError("", "Must be selected image");
                return View(model);
            }

            if (!model.Image.IsAllowedSize(7))
            {
                ModelState.AddModelError("", "Image size can be max 7 mb");
                return View(model);
            }

            var unicalName = await model.Image.Generatefile(Constants.TeacherPath);

            var teachers = new Teacher
            {
                Name = model.FullName,
                Profession = model.Profession,
                ImageUrl = unicalName,
                AboutTeacher = model.AboutTeacher,
                Degree = model.Degree,
                Experience = model.Experience,
                Hobby = model.Hobby,
                Faculty = model.Faculty,
                EmailAddress = model.EmailAddress,
                PhoneNumber = model.PhoneNumber,
                SkypeAddress = model.SkypeAddress,
                LanguageSkill = model.LanguageSkill,
                DesignSkill = model.DesignSkill,
                TeamLeaderSkill = model.TeamLeaderSkill,
                InnovationSkill = model.InnovationSkill,
                DevelopmentSkill = model.DevelopmentSkill,
                CommunicationSkill = model.CommunicationSkill
            };

            await _dbContext.Teachers.AddAsync(teachers);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();

            var teachers = await _dbContext.Teachers
                .Where(t => t.Id == id)
                .FirstOrDefaultAsync();

            if (teachers == null) return NotFound();

            var teacherViewModel = new TeacherUpdateViewModel
            {
                Id = teachers.Id,
                FullName = teachers.Name,
                Profession = teachers.Profession,
                ImageUrl = teachers.ImageUrl,
                AboutTeacher = teachers.AboutTeacher,
                Degree = teachers.Degree,
                Experience = teachers.Experience,
                Hobby = teachers.Hobby,
                Faculty = teachers.Faculty,
                EmailAddress = teachers.EmailAddress,
                PhoneNumber = teachers.PhoneNumber,
                SkypeAddress = teachers.SkypeAddress,
                LanguageSkill = teachers.LanguageSkill,
                DesignSkill = teachers.DesignSkill,
                TeamLeaderSkill = teachers.TeamLeaderSkill,
                InnovationSkill = teachers.InnovationSkill,
                DevelopmentSkill = teachers.DevelopmentSkill,
                CommunicationSkill = teachers.CommunicationSkill,

            };

            return View(teacherViewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, TeacherUpdateViewModel model)
        {
            if (id == null) return BadRequest();

            var teachers = await _dbContext.Teachers.FindAsync(id);
                

            if (teachers == null) return NotFound();

            if (!ModelState.IsValid) return View(new TeacherUpdateViewModel
            {
                ImageUrl = teachers.ImageUrl
            });

            if (model.Image != null)
            {
                if (!model.Image.IsImage())
                {
                    ModelState.AddModelError("", "Must be selected image");
                    return View(new TeacherUpdateViewModel
                    {
                        ImageUrl = teachers.ImageUrl,
                    });
                }

                if (!model.Image.IsAllowedSize(7))
                {
                    ModelState.AddModelError("", "Image size can be max 7 mb");
                    return View(model);
                }

                var unicalName = await model.Image.Generatefile(Constants.TeacherPath);

                teachers.ImageUrl = unicalName;
            }

            teachers.Name = model.FullName;
            teachers.Profession = model.Profession;          
            teachers.AboutTeacher = model.AboutTeacher;
            teachers.Degree = model.Degree;
            teachers.Experience = model.Experience;
            teachers.Hobby = model.Hobby;
            teachers.Faculty = model.Faculty;
            teachers.EmailAddress = model.EmailAddress;
            teachers.PhoneNumber = model.PhoneNumber;
            teachers.SkypeAddress = model.SkypeAddress;
            teachers.LanguageSkill = model.LanguageSkill;
            teachers.DesignSkill = model.DesignSkill;
            teachers.TeamLeaderSkill = model.TeamLeaderSkill;
            teachers.InnovationSkill = model.InnovationSkill;
            teachers.DevelopmentSkill = model.DevelopmentSkill;
            teachers.CommunicationSkill = model.CommunicationSkill;

            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));                
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var teachers = await _dbContext.Teachers
                .FirstOrDefaultAsync(t => t.Id == id);

            if (teachers == null) return NotFound();

            if (teachers.ImageUrl == null) return NotFound();
            if (teachers.Id != id) return BadRequest();

            var teacherImagePath = Path.Combine(Constants.RootPath, "assets", "img", "teacher", teachers.ImageUrl);

            if (System.IO.File.Exists(teacherImagePath))
                System.IO.File.Delete(teacherImagePath);

            _dbContext.Teachers.Remove(teachers);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
    }

}
