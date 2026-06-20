using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CyberSecurityChatbot_PART_2
{
    // This class stores user information
    // and remembers users even after
    // the application is closed.
    public class MemoryStore
    {
        // File used to save user memory
        private readonly string filePath = "memory.txt";

        // Stores the current user's name
        public string UserName { get; set; }

        // Stores the user's favourite topic
        public string FavouriteTopic { get; set; }

        // Dictionary stores multiple user
        private Dictionary<string, string> userTopics;

        // Constructor
        // Loads saved memory when app starts
        public MemoryStore()
        {
            userTopics = new Dictionary<string, string>();
            LoadMemory();
        }

        // Saves the topic linked to a user
        public void StoreTopic(string topic)
        {
            FavouriteTopic = topic;

            // Save topic for current user
            if (!string.IsNullOrEmpty(UserName))
            {
                userTopics[UserName] = topic;
                SaveMemory();
            }
        }

        // Checks if a user already exists
        public bool IsReturningUser(string name)
        {
            return userTopics.ContainsKey(name);
        }

        // Gets the last topic for a user
        public string GetLastTopic(string name)
        {
            if (userTopics.ContainsKey(name))
            {
                return userTopics[name];
            }

            return "";
        }

        // Saves memory to a text file
        private void SaveMemory()
        {
            List<string> lines = new List<string>();

            foreach (var user in userTopics)
            {
                lines.Add($"{user.Key}|{user.Value}");
            }

            File.WriteAllLines(filePath, lines);
        }

        // Loads memory from the text file
        private void LoadMemory()
        {
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);

                foreach (var line in lines)
                {
                    var parts = line.Split('|');

                    if (parts.Length == 2)
                    {
                        userTopics[parts[0]] = parts[1];
                    }
                }
            }
        }

        // Creates a personalised opener
        // based on saved topic
        public string GetPersonalisedOpener()
        {
            if (!string.IsNullOrEmpty(FavouriteTopic))
            {
                return $"Since you're interested in {FavouriteTopic}, here's something useful: ";
            }

            return "";
        }

        // Creates a personalised greeting
        public string GetNameGreeting()
        {
            if (!string.IsNullOrEmpty(UserName))
            {
                return $"Great to see you again, {UserName}! ";
            }

            return "";
        }
    }
}