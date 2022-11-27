using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendFinalProjectEduHome.DAL.Entity
{
    public class Event : Entity
    {
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        public string Address { get; set; }
        public ICollection<EventSpeaker> EventSpeakers { get; set; }
      
    }
}
