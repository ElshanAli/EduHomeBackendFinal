namespace BackendFinalProjectEduHome.DAL.Entity
{
    public class Speaker : Entity
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string? ImageUrl { get; set; }
        public string Position { get; set; }
        public string Company { get; set; }
        public ICollection<EventSpeaker> EventSpeakers { get; set; }
    }
}
