using System.ComponentModel.DataAnnotations;

namespace BackendFinalProjectEduHome.Areas.Admin.ViewModels
{
    public class SettingsUpdateViewModel
    {
        public string? HeaderLogo { get; set; }
        public IFormFile? HeaderLogoImage { get; set; }
        public string? FooterLogo { get; set; }
        public IFormFile? FooterLogoImage { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        public IFormFile? FormPhoneImage { get; set; }
        public string? PhoneImage { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string? SecondPhone { get; set; }
        [DataType(DataType.Url)]
        public string? FacebookLink { get; set; }
        [DataType(DataType.Url)]
        public string? PinterestLink { get; set; }
        [DataType(DataType.Url)]
        public string? TwitterLink { get; set; }
        [DataType(DataType.Url)]
        public string? VimeoLink { get; set; }
        public string Address { get; set; }
        public string? SecondAddress { get; set; }
        public IFormFile? FormAddressImage { get; set; }
        public string? AddressImage { get; set; }
        public string GoogleMapCode { get; set; }
        public string FooterDescription { get; set; }
        [DataType(DataType.Url)]
        public string WebSite { get; set; }
        [DataType(DataType.Url)]
        public string? SecondWebsite { get; set; }
        public IFormFile? FormWebsiteImage { get; set; }
        public string? WebSiteImage { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [EmailAddress]
        public string? SecondEmail { get; set; }
        public string GreetingText { get; set; }
    }
}
