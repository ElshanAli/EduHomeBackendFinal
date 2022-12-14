

namespace BackendFinalProjectEduHome.DAL.Entity
{
    public class Teacher : Entity
    {
        public string Name { get; set; }
        public string Profession { get; set; }
        public string ImageUrl { get; set; }
        public string AboutTeacher { get; set; }
        public string Degree { get; set; }
        public string Experience { get; set; }
        public string Hobby { get; set; }
        public string Faculty { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string SkypeAddress { get; set; }
        public byte LanguageSkill { get; set; }
        public byte DesignSkill { get; set; }
        public byte TeamLeaderSkill { get; set; }
        public byte InnovationSkill { get; set; }
        public byte DevelopmentSkill { get; set; }
        public byte CommunicationSkill { get; set; }
    }
}
