using BackendFinalProjectEduHome.Areas.Admin.Data;
using BackendFinalProjectEduHome.Areas.Admin.ViewModels;
using BackendFinalProjectEduHome.DAL;
using BackendFinalProjectEduHome.DAL.Entity;
using BackendFinalProjectEduHome.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendFinalProjectEduHome.Areas.Admin.Controllers
{
    public class SettingsController : BaseController
    {
        private readonly EduHomeDbContext _dbContext;

        public SettingsController(EduHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var settings = await _dbContext.Settings.FirstOrDefaultAsync();

            return View(settings);
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();

            var dbSetting = await _dbContext.Settings.FirstOrDefaultAsync(x => x.Id == id);

            if (dbSetting == null) return NotFound();

            var settingsUpdateViewModel = new SettingsUpdateViewModel 
            {
                HeaderLogo = dbSetting.HeaderLogo,
                FooterLogo = dbSetting.FooterLogo,
                Phone = dbSetting.Phone,
                SecondPhone = dbSetting.SecondPhone,
                FacebookLink = dbSetting.FacebookLink,
                PinterestLink = dbSetting.PinterestLink,
                TwitterLink = dbSetting.TwitterLink,
                VimeoLink = dbSetting.VimeoLink,
                Address = dbSetting.Address,
                GoogleMapCode = dbSetting.GoogleMapCode,
                FooterDescription = dbSetting.FooterDescription,
                WebSite = dbSetting.WebSite,
                Email = dbSetting.Email,
                SecondEmail = dbSetting.SecondEmail,
                GreetingText = dbSetting.GreetingText,
            };

            return View(settingsUpdateViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, SettingsUpdateViewModel model)
        {
            if (id == null) return BadRequest();

            var dbSetting = await _dbContext.Settings.FirstOrDefaultAsync(x => x.Id == id);

            if (dbSetting == null) return NotFound();

            if (!ModelState.IsValid) return View(model);

            if (model.HeaderLogoImage != null)
            {
                if (!model.HeaderLogoImage.IsImage())
                {
                    ModelState.AddModelError("", "Must be selected image");
                    return View(model);
                }

                if (!model.HeaderLogoImage.IsAllowedSize(7))
                {
                    ModelState.AddModelError("", "Image size can be max 7 mb");
                    return View(model);
                }

                if (dbSetting.HeaderLogo is null) return NotFound();

                var headerLogoPath = Path.Combine(Constants.RootPath, "assets", "img", "setting", dbSetting.HeaderLogo);

                if (System.IO.File.Exists(headerLogoPath))
                    System.IO.File.Delete(headerLogoPath);

                var unicalName = await model.HeaderLogoImage.Generatefile(Constants.SettingsPath);
                dbSetting.HeaderLogo = unicalName;
            }

            if (model.FooterLogoImage != null)
            {
                if (!model.FooterLogoImage.IsImage())
                {
                    ModelState.AddModelError("", "Must be selected image");
                    return View(model);
                }

                if (!model.FooterLogoImage.IsAllowedSize(7))
                {
                    ModelState.AddModelError("", "Image size can be max 7 mb");
                    return View(model);
                }

                if (dbSetting.FooterLogo is null) return NotFound();

                var footerLogoPath = Path.Combine(Constants.RootPath, "assets", "img", "setting", dbSetting.FooterLogo);

                if (System.IO.File.Exists(footerLogoPath))
                    System.IO.File.Delete(footerLogoPath);

                var unicalName = await model.FooterLogoImage.Generatefile(Constants.SettingsPath);
                dbSetting.FooterLogo = unicalName;
            }

            dbSetting.Phone = model.Phone;
            dbSetting.SecondPhone = model.SecondPhone;
            dbSetting.FacebookLink = model.FacebookLink;
            dbSetting.PinterestLink = model.PinterestLink;
            dbSetting.TwitterLink = model.TwitterLink;
            dbSetting.VimeoLink = model.VimeoLink;
            dbSetting.Address = model.Address;
            dbSetting.GoogleMapCode = model.GoogleMapCode;
            dbSetting.FooterDescription = model.FooterDescription;
            dbSetting.WebSite = model.WebSite;
            dbSetting.Email = model.Email;
            dbSetting.SecondEmail = model.SecondEmail;
            dbSetting.GreetingText = model.GreetingText;
                
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
