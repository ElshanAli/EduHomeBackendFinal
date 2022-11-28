using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BackendFinalProjectEduHome.Areas.Admin.ViewModels
{
    public class EventUpdateViewModel
    {
        public int Id { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? Image { get; set; }
        public string Address { get; set; }
        public List<SelectListItem>? Speakers { get; set; }
        public List<int> SpeakerIds { get; set; }
    }
}
