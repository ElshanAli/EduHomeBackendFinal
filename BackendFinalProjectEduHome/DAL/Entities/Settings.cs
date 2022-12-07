
namespace BackendFinalProjectEduHome.DAL.Entity
{
    public class Settings : Entity
    {
        public string? HeaderLogo { get; set; }
        public string? FooterLogo { get; set; }
        public string Phone { get; set; }
        public string PhoneImage { get; set; }
        public string? SecondPhone { get; set; }
        public string? FacebookLink { get; set; }
        public string? PinterestLink { get; set; }
        public string? TwitterLink { get; set; }
        public string? VimeoLink { get; set; }
        public string Address { get; set; }
        public string? SecondAddress { get; set; }
        public string AdressImage { get; set; }
        public string GoogleMapCode { get; set; }
        public string FooterDescription { get; set; }
        public string WebSite { get; set; }
        public string? SecondWebsite { get; set; }
        public string WebsiteImage { get; set; }
        public string Email { get; set; }
        public string? SecondEmail { get; set; }
        public string GreetingText { get; set; }
    }
}
