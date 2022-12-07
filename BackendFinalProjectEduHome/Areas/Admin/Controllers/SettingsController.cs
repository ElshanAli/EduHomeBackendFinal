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
                PhoneImage = dbSetting.PhoneImage,
                SecondPhone = dbSetting.SecondPhone,
                FacebookLink = dbSetting.FacebookLink,
                PinterestLink = dbSetting.PinterestLink,
                TwitterLink = dbSetting.TwitterLink,
                VimeoLink = dbSetting.VimeoLink,
                Address = dbSetting.Address,
                SecondAddress = dbSetting.SecondAddress,
                AddressImage = dbSetting.AdressImage,
                GoogleMapCode = dbSetting.GoogleMapCode,
                FooterDescription = dbSetting.FooterDescription,
                WebSite = dbSetting.WebSite,
                SecondWebsite = dbSetting.SecondWebsite,
                WebSiteImage = dbSetting.WebsiteImage,
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

            if (model.FormPhoneImage != null)
            {
                if (!model.FormPhoneImage.IsImage())
                {
                    ModelState.AddModelError("", "Must be selected image");
                    return View(model);
                }

                if (!model.FormPhoneImage.IsAllowedSize(7))
                {
                    ModelState.AddModelError("", "Image size can be max 7 mb");
                    return View(model);
                }

                if (dbSetting.PhoneImage is null) return NotFound();

                var phonePath = Path.Combine(Constants.RootPath, "assets", "img", "setting", dbSetting.PhoneImage);

                if (System.IO.File.Exists(phonePath))
                    System.IO.File.Delete(phonePath);

                var unicalName = await model.FormPhoneImage.Generatefile(Constants.SettingsPath);
                dbSetting.PhoneImage = unicalName;
            }

            if (model.FormAddressImage != null)
            {
                if (!model.FormAddressImage.IsImage())
                {
                    ModelState.AddModelError("", "Must be selected image");
                    return View(model);
                }

                if (!model.FormAddressImage.IsAllowedSize(7))
                {
                    ModelState.AddModelError("", "Image size can be max 7 mb");
                    return View(model);
                }

                if (dbSetting.AdressImage is null) return NotFound();

                var addressPath = Path.Combine(Constants.RootPath, "assets", "img", "setting", dbSetting.AdressImage);

                if (System.IO.File.Exists(addressPath))
                    System.IO.File.Delete(addressPath);

                var unicalName = await model.FormAddressImage.Generatefile(Constants.SettingsPath);
                dbSetting.AdressImage = unicalName;
            }

            if (model.FormWebsiteImage != null)
            {
                if (!model.FormWebsiteImage.IsImage())
                {
                    ModelState.AddModelError("", "Must be selected image");
                    return View(model);
                }

                if (!model.FormWebsiteImage.IsAllowedSize(7))
                {
                    ModelState.AddModelError("", "Image size can be max 7 mb");
                    return View(model);
                }

                if (dbSetting.WebsiteImage is null) return NotFound();

                var websitePath = Path.Combine(Constants.RootPath, "assets", "img", "setting", dbSetting.WebsiteImage);

                if (System.IO.File.Exists(websitePath))
                    System.IO.File.Delete(websitePath);

                var unicalName = await model.FormWebsiteImage.Generatefile(Constants.SettingsPath);
                dbSetting.WebsiteImage = unicalName;
            }

            dbSetting.Phone = model.Phone;
            dbSetting.SecondPhone = model.SecondPhone;
            dbSetting.FacebookLink = model.FacebookLink;
            dbSetting.PinterestLink = model.PinterestLink;
            dbSetting.TwitterLink = model.TwitterLink;
            dbSetting.VimeoLink = model.VimeoLink;
            dbSetting.Address = model.Address;
            dbSetting.SecondAddress = model.SecondAddress;
            dbSetting.GoogleMapCode = model.GoogleMapCode;
            dbSetting.FooterDescription = model.FooterDescription;
            dbSetting.WebSite = model.WebSite;
            dbSetting.SecondWebsite = model.SecondWebsite;
            dbSetting.Email = model.Email;
            dbSetting.SecondEmail = model.SecondEmail;
            dbSetting.GreetingText = model.GreetingText;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
