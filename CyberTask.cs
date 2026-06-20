namespace CyberSecurityChatbot_PART_2
{
    // Represents one cybersecurity task in the system
    public class CyberTask
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Reminder { get; set; }

        public bool IsComplete { get; set; }

        public string CreatedAt { get; set; }
    }
}