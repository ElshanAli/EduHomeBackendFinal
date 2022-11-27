using BackendFinalProjectEduHome.DAL.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BackendFinalProjectEduHome.Areas.Admin.ViewModels
{
    public class EventCreateViewModel
    {

        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public string Address { get; set; }

        public List<SelectList> Speakers { get; set; }
        //public List<SelectList> SpeakerList { get; set; }
    }
}
