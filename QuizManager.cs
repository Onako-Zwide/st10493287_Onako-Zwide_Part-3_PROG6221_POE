using System;
using System.Collections.Generic;
using System.Text;

namespace CyberSecurityChatbot_PART_2
{
    internal class QuizManager
    {
        private List<QuizQuestion> _questions;
        private int _index = -1;
        private int _score = 0;

        // Tracks wrong answers as tuples: (questionIndex, chosenIndex)
        private List<(int QuestionIndex, int ChosenIndex)> _wrongAnswers = new();

        public QuizManager()
        {
            _questions = new List<QuizQuestion>();
            LoadDefaultQuestions();
        }

        private void LoadDefaultQuestions()
        {
            // Add more than 10 questions covering cybersecurity topics
            _questions.Add(new QuizQuestion { QuestionText = "What is phishing?", Choices = { "A fishing technique", "A social engineering attack", "A password manager", "An encryption method" }, CorrectIndex = 1 });
            _questions.Add(new QuizQuestion { QuestionText = "Which practice helps secure passwords?", Choices = { "Reuse passwords", "Use simple words", "Use a password manager", "Share passwords" }, CorrectIndex = 2 });
            _questions.Add(new QuizQuestion { QuestionText = "What does VPN do?", Choices = { "Speeds up internet", "Hides browsing activity", "Deletes cookies", "Scans for viruses" }, CorrectIndex = 1 });
            _questions.Add(new QuizQuestion { QuestionText = "What is ransomware?", Choices = { "Type of antivirus", "Type of malware that encrypts files", "A secure backup", "A network protocol" }, CorrectIndex = 1 });
            _questions.Add(new QuizQuestion { QuestionText = "Which is a sign of scam?", Choices = { "Unexpected request for money", "Email from friend", "Normal system update", "Bank login page" }, CorrectIndex = 0 });
            _questions.Add(new QuizQuestion { QuestionText = "Two-factor authentication (2FA) provides:", Choices = { "Weaker security", "An extra layer of security", "Faster login", "No benefit" }, CorrectIndex = 1 });
            _questions.Add(new QuizQuestion { QuestionText = "Best way to verify a link is safe:", Choices = { "Click it immediately", "Hover to check URL", "Trust any email", "Reply with your password" }, CorrectIndex = 1 });
            _questions.Add(new QuizQuestion { QuestionText = "Public Wi-Fi risk includes:", Choices = { "Improved privacy", "Potential eavesdropping", "Guaranteed encryption", "Faster downloads" }, CorrectIndex = 1 });
            _questions.Add(new QuizQuestion { QuestionText = "Malware can be delivered via:", Choices = { "Trusted app stores only", "Email attachments and downloads", "Only physical devices", "None of these" }, CorrectIndex = 1 });
            _questions.Add(new QuizQuestion { QuestionText = "Regular software updates help by:", Choices = { "Adding bugs", "Fixing security vulnerabilities", "Slowing device", "Removing features" }, CorrectIndex = 1 });
            _questions.Add(new QuizQuestion { QuestionText = "A strong password should:", Choices = { "Use your birthday", "Be short", "Contain letters, numbers, symbols", "Be common words" }, CorrectIndex = 2 });
        }

        public void StartQuiz()
        {
            _index = 0;
            _score = 0;
            _wrongAnswers.Clear();
        }

        public QuizQuestion GetCurrentQuestion()
        {
            if (_index >= 0 && _index < _questions.Count)
                return _questions[_index];

            return null;
        }

        public bool AnswerCurrent(int choiceIndex)
        {
            var q = GetCurrentQuestion();

            if (q == null)
                return false;

            bool correct = choiceIndex == q.CorrectIndex;

            if (correct) _score++;
            else _wrongAnswers.Add((_index, choiceIndex));

            return correct;
        }

        public void NextQuestion()
        {
            if (_index < _questions.Count - 1)
                _index++;
        }

        public bool IsLastQuestion()
        {
            return _index >= _questions.Count - 1;
        }

        public string FinishQuiz()
        {
            string msg = $"Quiz complete. Score: {_score} / {_questions.Count}. ";

            if (_score == _questions.Count)
                msg += "Perfect! Great job.";
            else if (_score >= _questions.Count * 0.7)
                msg += "Well done!";
            else
                msg += "Keep learning and practicing.";

            ActivityLogger.Log($"Quiz finished: {_score}/{_questions.Count}");

            return msg;
        }

        public bool IsQuizActive()
        {
            return _index >= 0 && _index < _questions.Count;
        }

        public bool IsLast()
        {
            return IsLastQuestion();
        }

        public int GetScore() => _score;
        public int GetTotal() => _questions.Count;

        // Returns a human-readable summary of wrong answers.
        public string GetWrongSummary()
        {
            if (_wrongAnswers.Count == 0)
                return "All answers correct.";

            var sb = new StringBuilder();
            sb.AppendLine("Questions you got wrong:");

            foreach (var (qIndex, chosen) in _wrongAnswers)
            {
                var q = _questions[qIndex];
                sb.AppendLine($"- Q{qIndex + 1}: {q.QuestionText}");
                sb.AppendLine($"    Your answer: {(chosen >= 0 && chosen < q.Choices.Count ? q.Choices[chosen] : "N/A")}");
                sb.AppendLine($"    Correct answer: {q.Choices[q.CorrectIndex]}");
            }

            return sb.ToString();
        }
    }
}
