using System.ComponentModel.DataAnnotations;

namespace BackendFinalProjectEduHome.Areas.Admin.ViewModels
{
    public class SettingsUpdateViewModel
    {
        //public int Id { get; set; }
        public string? HeaderLogo { get; set; }
        public IFormFile? HeaderLogoImage { get; set; }
        public string? FooterLogo { get; set; }
        public IFormFile? FooterLogoImage { get; set; }
        [Phone]
        public string Phone { get; set; }
        [Phone]
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
        public string GoogleMapCode { get; set; }
        public string FooterDescription { get; set; }
        [DataType(DataType.Url)]
        public string WebSite { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [EmailAddress]
        public string? SecondEmail { get; set; }
        public string GreetingText { get; set; }
    }
}
