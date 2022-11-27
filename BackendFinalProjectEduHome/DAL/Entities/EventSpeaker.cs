namespace BackendFinalProjectEduHome.DAL.Entity
{
    public class EventSpeaker : Entity
    {
        public int EventId { get; set; }
        public Event Event { get; set; }
        public int SpeakerId { get; set; }
        public Speaker Speaker { get; set; }

    }
}
