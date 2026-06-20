using System.Collections.Generic;

namespace CyberSecurityChatbot_PART_2
{
    internal class QuizQuestion
    {
        public string QuestionText { get; set; }

        public List<string> Choices { get; set; }

        public int CorrectIndex { get; set; }

        public QuizQuestion()
        {
            Choices = new List<string>();
        }
    }
}
