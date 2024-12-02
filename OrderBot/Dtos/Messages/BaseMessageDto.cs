namespace OrderBot.Dtos
{
    public class BaseMessageDto
    {
        public string Type { get; set; }
    }
    public class Mentionee
    {
        public string Type { get; set; }
        public string UserId { get; set; }
    }

    public class User
    {
        public string Type { get; set; }
        public Mentionee Mentionee { get; set; }
    }

    public class SubstitutionDto
    {
        public User User1 { get; set; }
    }
}
