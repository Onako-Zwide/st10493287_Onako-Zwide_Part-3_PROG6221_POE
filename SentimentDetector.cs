using System;

namespace CyberSecurityChatbot_PART_2
{
    // This enum stores the different emotions
    // that the chatbot can recognise.
    public enum Sentiment
    {
        Neutral,
        Worried,
        Curious,
        Frustrated,
        Happy
    }

    // This class is responsible for
    // detecting the user's emotion
    // based on the words they type.
    public class SentimentDetector
    {
        // This method checks the user's message
        // and identifies the emotion/sentiment.
        public Sentiment Detect(string input)
        {
            // Converts input to lowercase
            // to make checking easier and case insensitive.
            string text = input.ToLower();

            // Detects worried emotions
            // if the user types certain keywords.
            if (text.Contains("worried") || text.Contains("scared") || text.Contains("afraid"))
                return Sentiment.Worried;

            // Detects curiosity
            // when the user asks questions.
            if (text.Contains("how") || text.Contains("why") || text.Contains("what"))
                return Sentiment.Curious;

            // Detects frustration
            // if the user sounds upset or annoyed.
            if (text.Contains("angry") || text.Contains("frustrated") || text.Contains("annoyed"))
                return Sentiment.Frustrated;

            // Detects positive emotions
            // when users say good or thankful words.
            if (text.Contains("thank") || text.Contains("good") || text.Contains("great"))
                return Sentiment.Happy;

            // If no emotion is detected,
            // the chatbot stays neutral.
            return Sentiment.Neutral;
        }

        // This method returns an emotional response
        // depending on the detected sentiment.
        public string GetSentimentResponse(Sentiment sentiment)
        {
            // Switch statement checks
            // which emotion was detected.
            switch (sentiment)
            {
                // Response for worried users
                case Sentiment.Worried:
                    return "I understand your concern, but do not worry, we'll go through this carefully. ";

                // Response for curious users
                case Sentiment.Curious:
                    return "Great question, here’s a simple explanation. ";

                // Response for frustrated users
                case Sentiment.Frustrated:
                    return "No worries, I’ll make this easy to understand. ";

                // Response for happy users
                case Sentiment.Happy:
                    return "Glad you’re engaging with this! ";

                // Default response if no emotion exists
                default:
                    return "";
            }
        }
    }
}