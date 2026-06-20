using System;

namespace CyberSecurityChatbot_PART_2
{
    // This class controls the chatbot's behaviour.
    // It processes user input and returns responses.
    public class ChatBot
    {
        private KeywordResponder _keywords;
        private SentimentDetector _sentiment;
        private MemoryStore _memory;
        private TaskManager _taskManager;
        private QuizManager _quizManager;

        // Checks if the chatbot is still waiting for the user to enter their name.
        private bool _awaitingName = true;

        // Stores the previous topic discussed so the chatbot can answer follow-up questions.
        private string _lastTopic = "";

        // Constructor
        public ChatBot()
        {
            _keywords = new KeywordResponder();
            _sentiment = new SentimentDetector();
            _memory = new MemoryStore();
            _taskManager = new TaskManager();
        }

        // Initial greeting
        public string GetGreeting()
        {
            return "Please enter your name. ";
        }

        // Main input processor
        public string ProcessInput(string input)
        {
            string text = input.ToLower();

            // If a quiz is active, handle numeric answers first
            if (_quizManager != null && _quizManager.IsQuizActive())
            {
                if (int.TryParse(text.Trim(), out int num))
                {
                    var current = _quizManager.GetCurrentQuestion();
                    int choiceIndex = num - 1;

                    if (current == null)
                        return "There was an error retrieving the question.";

                    if (choiceIndex < 0 || choiceIndex >= current.Choices.Count)
                        return "Please enter a valid option number.";

                    bool correct = _quizManager.AnswerCurrent(choiceIndex);
                    string resp = correct ? "Correct!" : $"Incorrect. Correct answer: {current.Choices[current.CorrectIndex]}";

                    if (_quizManager.IsLast())
                    {
                        resp += "\n" + _quizManager.FinishQuiz();
                        _quizManager = null;
                    }
                    else
                    {
                        _quizManager.NextQuestion();
                        var next = _quizManager.GetCurrentQuestion();
                        resp += "\nNext question:\n" + FormatQuestion(next);
                    }

                    return resp;
                }

                return "Please answer the current quiz question by typing the option number (e.g., 1).";
            }

            // Activity log request
            if (text.Contains("show activity log") || text.Contains("show log") || text.Contains("what have you done") || text.Contains("recent actions"))
            {
                return ActivityLogger.GetRecentLog();
            }

            // Add task intent
            if (text.Contains("add task") || text.Contains("add a task") || text.Contains("create task"))
            {
                string taskTitle = input;

                taskTitle = taskTitle.Replace("add task", "").Replace("add a task", "").Replace("create task", "").Trim();

                if (string.IsNullOrWhiteSpace(taskTitle))
                {
                    return "Please tell me the task you would like to add.";
                }

                _taskManager.AddTask(taskTitle, "Cybersecurity task", "");
                return $"Task added: '{taskTitle}'. Would you like a reminder?";
            }

            // If awaiting name
            if (_awaitingName)
            {
                _memory.UserName = input;
                _awaitingName = false;

                if (_memory.IsReturningUser(input))
                {
                    string lastTopic = _memory.GetLastTopic(input);
                    return $"Welcome back {_memory.UserName}! Last time we spoke about {lastTopic}. Feel free to ask me anything cybersecurity related. Type 'exit' to quit.";
                }

                return $"Greetings {_memory.UserName}! I'm Uniqua, your cybersecurity awareness assistant. Ask me about phishing, scams, password safety, or type 'quiz' to take a short quiz. Type 'exit' to quit.";
            }

            // Start quiz intent
            if (text.Contains("quiz") || text.Contains("start quiz") || text.Contains("take quiz"))
            {
                _quizManager = new QuizManager();
                _quizManager.StartQuiz();
                var first = _quizManager.GetCurrentQuestion();
                return "Starting quiz. Answer by typing the option number (e.g., 1).\n" + FormatQuestion(first);
            }

            // Exit
            if (text == "exit")
            {
                return "It was great working with you! Stay safe online. Goodbye!";
            }

            // More info about last topic
            if (text.Contains("tell me more") || text.Contains("explain more"))
            {
                if (_lastTopic == "phishing")
                    return "These messages often create urgency to make victims click malicious links or download harmful attachments. Always verify the sender and avoid suspicious links.";

                if (_lastTopic == "password")
                    return "Your password must be hard to guess and should not include personal information such as birthdays or names.";

                if (_lastTopic == "scam")
                    return "Scammers may pretend to be trusted companies or even family members. Always verify before sharing information.";

                if (_lastTopic == "safe")
                    return "Staying safe online also means updating your apps and avoiding public Wi-Fi when sharing sensitive information.";

                return "Please tell me which topic you would like to know more about.";
            }

            // Detect sentiment
            Sentiment sentiment = _sentiment.Detect(input);
            string emotionResponse = _sentiment.GetSentimentResponse(sentiment);

            // Keyword responses
            if (text == "1" || text.Contains("phishing"))
            {
                _lastTopic = "phishing";
                _memory.StoreTopic("phishing");
                return emotionResponse + "Phishing is a cyberattack where criminals pretend to be trusted organizations in emails, texts, phone calls, or fake websites to trick people into revealing sensitive information. Always verify the sender and avoid suspicious links.";
            }

            if (text == "2" || text.Contains("password"))
            {
                _lastTopic = "password";
                _memory.StoreTopic("password");
                return emotionResponse + "Passwords protect accounts and personal information. Use strong, unique passwords and consider a password manager and two-factor authentication.";
            }

            if (text == "3" || text.Contains("scam"))
            {
                _lastTopic = "scam";
                _memory.StoreTopic("scam");
                return emotionResponse + "A scam is a dishonest scheme designed to steal money or personal information. Be cautious of requests for money or personal details.";
            }

            if (text == "4" || text.Contains("safe"))
            {
                _lastTopic = "safe";
                _memory.StoreTopic("safe");
                return emotionResponse + "Staying safe online means using strong passwords, avoiding unknown links, and keeping software updated.";
            }

            if (text.Contains("malware"))
            {
                _lastTopic = "malware";
                _memory.StoreTopic("malware");
                return emotionResponse + "Malware is harmful software that can damage devices or steal information. Avoid suspicious downloads and keep devices updated.";
            }

            if (text.Contains("virus"))
            {
                _lastTopic = "virus";
                _memory.StoreTopic("virus");
                return emotionResponse + "A computer virus is a harmful program that can damage files or slow performance. Be careful with attachments and downloads.";
            }

            if (text.Contains("ransomware"))
            {
                _lastTopic = "ransomware";
                _memory.StoreTopic("ransomware");
                return emotionResponse + "Ransomware locks files until payment is made. Avoid suspicious attachments and back up important files.";
            }

            if (text.Contains("hacker") || text.Contains("hacking"))
            {
                _lastTopic = "hacking";
                _memory.StoreTopic("hacking");
                return emotionResponse + "Hackers try to gain unauthorized access to systems. Some aim to help security, others to cause harm.";
            }

            if (text.Contains("social engineering"))
            {
                _lastTopic = "social engineering";
                _memory.StoreTopic("social engineering");
                return emotionResponse + "Social engineering tricks people into revealing information by pretending to be trustworthy.";
            }

            if (text.Contains("cyberbullying") || text.Contains("bullying"))
            {
                _lastTopic = "cyberbullying";
                _memory.StoreTopic("cyberbullying");
                return emotionResponse + "Cyberbullying uses technology to harass or embarrass. Report and block harmful behaviour.";
            }

            if (text.Contains("email"))
            {
                _lastTopic = "email";
                _memory.StoreTopic("email");
                return emotionResponse + "Be careful with suspicious emails, avoid unknown attachments, and never share personal info via untrusted messages.";
            }

            if (text.Contains("suspicious link") || text.Contains("unknown link") || text.Contains("click link"))
            {
                _lastTopic = "suspicious links";
                _memory.StoreTopic("suspicious links");
                return emotionResponse + "Suspicious links may lead to fake sites or install malware. Check URLs before clicking.";
            }

            if (text.Contains("public wifi") || text.Contains("public wi-fi") || text.Contains("wifi safety"))
            {
                _lastTopic = "public wifi";
                _memory.StoreTopic("public wifi");
                return emotionResponse + "Public Wi-Fi can be unsafe; avoid logging into sensitive accounts on public networks.";
            }

            if (text.Contains("privacy") || text.Contains("data privacy"))
            {
                _lastTopic = "privacy";
                _memory.StoreTopic("privacy");
                return emotionResponse + "Data privacy means protecting your personal information and controlling access to it.";
            }

            if (text.Contains("vpn"))
            {
                _lastTopic = "vpn";
                _memory.StoreTopic("vpn");
                return emotionResponse + "A VPN encrypts your connection and can help improve online privacy.";
            }

            if (text.Contains("hello") || text.Contains("hi"))
            {
                return "Hello! How can I assist you with cybersecurity today?";
            }

            if (text.Contains("how are you"))
            {
                return "As a bot I have no feelings, but I'm here to help. How may I assist you?";
            }

            if (text.Contains("purpose"))
            {
                return "My purpose is to help you stay safe online and understand cybersecurity.";
            }

            return "I didn't understand that. Try asking about phishing, passwords, scams, start a 'quiz', or type 'exit'.";
        }

        private string FormatQuestion(QuizQuestion q)
        {
            if (q == null) return "No question available.";

            string s = q.QuestionText + "\n";
            for (int i = 0; i < q.Choices.Count; i++)
            {
                s += $"{i + 1}. {q.Choices[i]}\n";
            }

            return s.TrimEnd();
        }
    }
}
