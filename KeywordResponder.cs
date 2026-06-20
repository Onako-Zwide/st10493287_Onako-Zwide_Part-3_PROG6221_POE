using System;
using System.Collections.Generic;

namespace CyberSecurityChatbot_PART_2
{
    // This class handles keyword recognition.
    // It checks the user's message for keywords
    // and returns an appropriate response.
    public class KeywordResponder
    {
        // Dictionary stores keywords and multiple responses.
     
        private Dictionary<string, List<string>> _responses;

        // Random object is used to randomly select
        // different responses so the chatbot
        private Random _random = new Random();

        // Constructor
        // This runs automatically when the class is created.
        public KeywordResponder()
        {
            // Stores cybersecurity topics and responses
            _responses = new Dictionary<string, List<string>>()
            {
                // PHISHING RESPONSES
                {
                    "phishing", new List<string>()
                    {
                        "Phishing is when attackers pretend to be trusted sources to steal your information.",
                        "These attacks often come through emails or messages that look real but are fake.",
                        "Always check links carefully before clicking anything suspicious."
                    }
                },

                // PASSWORD SAFETY RESPONSES
                {
                    "password", new List<string>()
                    {
                        "A strong password should mix letters, numbers, and symbols.",
                        "Never reuse the same password across different accounts.",
                        "Avoid using personal information like your name or birthday."
                    }
                },

                // SCAM RESPONSES
                {
                    "scam", new List<string>()
                    {
                        "Scams are tricks designed to steal your money or personal details.",
                        "If something sounds too good to be true, it is usually a scam.",
                        "Always verify messages before trusting them."
                    }
                },

                // ONLINE SAFETY RESPONSES
                {
                    "safe", new List<string>()
                    {
                        "Stay safe by avoiding suspicious links and downloads.",
                        "Keep your apps and antivirus updated regularly.",
                        "Only share personal information on trusted websites."
                    }
                }
            };
        }

        // This method checks the user's input
        // for cybersecurity keywords and returns
        // a random response related to the topic.
        public string GetResponse(string input)
        {
            // Loops through all saved keywords
            foreach (var key in _responses.Keys)
            {
                // Checks if the user's message
                // contains a keyword
                if (input.ToLower().Contains(key))
                {
                    // Gets the list of responses
                    // linked to that keyword
                    var list = _responses[key];

                    // Returns a random response
                    // from the list
                    return list[_random.Next(list.Count)];
                }
            }

            // Returns an empty string
            // if no keyword is found
            return "";
        }

        // This method returns all keywords
        // stored in the dictionary.
        public List<string> GetAllKeywords()
        {
            return new List<string>(_responses.Keys);
        }
    }
}