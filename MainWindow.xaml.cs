using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CyberSecurityChatbot_PART_2
{
    // This is the main window of the chatbot application.
    // It controls the chatbot interface and handles user interaction.
    public partial class MainWindow : Window
    {
        // Creates an object of the ChatBot class
        // so that we can access chatbot responses.
        private ChatBot _chatBot;
        private QuizManager _quizManager;
        private int _activityDisplayCount = 5;

        // Constructor for MainWindow
        // This runs automatically when the window opens.
        public MainWindow()
        {
            // Loads everything designed in MainWindow.xaml
            InitializeComponent();

            // Creates a new chatbot object
            _chatBot = new ChatBot();
            _quizManager = new QuizManager();



            // Voice greeting
            // This tries to play the chatbot greeting audio
            // when the application starts.
            try
            {
                SoundPlayer player = new SoundPlayer("Chatbot Audio WAV.wav");
                player.Play();
            }
            catch
            {
                // If the audio file cannot be found or played,
                // a message box appears instead of crashing the app.
                MessageBox.Show("Voice greeting could not be played.");
            }

            // Displays the chatbot's first greeting message
            // when the app opens.
            AppendBotMessage(_chatBot.GetGreeting());

            // Load existing tasks into the UI
            try
            {
                RefreshTaskList();
            }
            catch
            {
                // If refresh fails, log and continue
                ActivityLogger.Log("Failed to refresh task list on startup.");
            }

            // Initialize activity log display
            try
            {
                ActivityLogText.Text = ActivityLogger.GetRecentLog(_activityDisplayCount);
            }
            catch { }
        }

        // This method runs when the Send button is clicked.
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        // This method allows the user to press Enter
        // on the keyboard instead of clicking Send.
        private void UserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage();
            }
        }

        // This method handles sending messages
        // between the user and the chatbot.
        private async void SendMessage()
        {
            // Stores what the user typed into a variable
            string input = UserInput.Text;

            // Prevents empty messages from being sent
            if (string.IsNullOrWhiteSpace(input))
                return;

            // Shows the user's message in the chat area
            AppendUserMessage(input);

            // Clears the textbox after sending the message
            UserInput.Clear();

            // Creates a typing indicator bubble
            // to make the chatbot feel more realistic.
            Border typingBubble = new Border
            {
                Background = System.Windows.Media.Brushes.DarkSlateBlue,
                CornerRadius = new CornerRadius(15),
                Padding = new Thickness(10),
                Margin = new Thickness(0, 5, 100, 5),
                HorizontalAlignment = HorizontalAlignment.Left,
                MaxWidth = 150
            };

            // Displays text to show that
            // the chatbot is preparing a response.
            TextBlock typingText = new TextBlock
            {
                Text = "Uniqua is typing...",
                Foreground = System.Windows.Media.Brushes.White
            };

            // Adds the typing bubble to the chat panel
            typingBubble.Child = typingText;
            ChatPanel.Children.Add(typingBubble);

            // Automatically scrolls to the newest message
            ChatScroll.ScrollToEnd();

            // Creates a short delay to simulate
            // the chatbot typing a response.
            await System.Threading.Tasks.Task.Delay(1200);

            // Removes the typing indicator
            // after the delay ends.
            ChatPanel.Children.Remove(typingBubble);

            // Sends the user's message to the chatbot
            // and stores the chatbot response.
            string response = _chatBot.ProcessInput(input);

            // Displays the chatbot response in the chat area.
            AppendBotMessage(response);
        }

        // This method creates and displays
        // the user's message bubble on the right side.
        private void AppendUserMessage(string message)
        {
            // Creates the message bubble design
            Border userBubble = new Border
            {
                Background = System.Windows.Media.Brushes.Magenta,
                CornerRadius = new CornerRadius(15),
                Padding = new Thickness(10),
                Margin = new Thickness(100, 5, 0, 5),
                HorizontalAlignment = HorizontalAlignment.Right,
                MaxWidth = 280
            };

            // Creates the text inside the message bubble
            TextBlock text = new TextBlock
            {
                Text = message,
                Foreground = System.Windows.Media.Brushes.White,
                TextWrapping = TextWrapping.Wrap
            };

            // Adds text into the message bubble
            userBubble.Child = text;

            // Adds the bubble to the chat panel
            ChatPanel.Children.Add(userBubble);

            // Scrolls automatically to newest message
            ChatScroll.ScrollToEnd();
        }

        // This method creates and displays
        // the chatbot's message bubble on the left side.
        private void AppendBotMessage(string message)
        {
            // Creates the chatbot bubble design
            Border botBubble = new Border
            {
                Background = System.Windows.Media.Brushes.DarkSlateBlue,
                CornerRadius = new CornerRadius(15),
                Padding = new Thickness(10),
                Margin = new Thickness(0, 5, 100, 5),
                HorizontalAlignment = HorizontalAlignment.Left,
                MaxWidth = 280

            };

            // Creates the chatbot message text
            TextBlock text = new TextBlock
            {
                // Adds "Uniqua:" before every chatbot message
                Text = "Uniqua: " + message,
                Foreground = System.Windows.Media.Brushes.White,
                TextWrapping = TextWrapping.Wrap
            };

            // Adds text into the chatbot bubble
            botBubble.Child = text;

            // Adds the chatbot bubble to the chat panel
            ChatPanel.Children.Add(botBubble);

            // Scrolls automatically to newest message
            ChatScroll.ScrollToEnd();
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            string title = TaskInput.Text?.Trim();
            string description = DescriptionInput.Text?.Trim();
            string reminder = ReminderInput.Text?.Trim();

            if (string.IsNullOrWhiteSpace(title))
            {
                MessageBox.Show("Please enter a title for the task.");
                return;
            }

            var manager = new TaskManager();

            if (AddTaskButtonTag.HasValue)
            {
                // Save existing edited task
                int editingId = AddTaskButtonTag.Value;
                manager.UpdateTask(editingId, title, description, reminder, false);

                // Reset edit state
                AddTaskButtonTag = null;
                AddTaskButton.Content = "Add Task";
            }
            else
            {
                manager.AddTask(title, description, reminder);
            }

            RefreshTaskList();

            TaskInput.Clear();
            DescriptionInput.Clear();
            ReminderInput.Clear();
        }

        private void TaskList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (TaskList.SelectedItem != null)
            {
                // Toggle completion using TaskManager
                var item = TaskList.SelectedItem as string;

                if (int.TryParse(item?.Split(':')[0], out int id))
                {
                    var manager = new TaskManager();
                    manager.ToggleComplete(id);

                    RefreshTaskList();
                }
            }
        }

        // New: update details panel when selection changes
        private void TaskList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TaskList.SelectedItem == null)
            {
                TaskDetailsText.Text = string.Empty;
                return;
            }

            var item = TaskList.SelectedItem as string;

            if (!int.TryParse(item?.Split(':')[0], out int id))
            {
                TaskDetailsText.Text = string.Empty;
                return;
            }

            var manager = new TaskManager();
            var task = manager.GetAllTasks().Find(t => t.Id == id);

            if (task != null)
            {
                TaskDetailsText.Text = $"Title: {task.Title}\n\nDescription: {task.Description}\n\nReminder: {task.Reminder}\n\nCreated: {task.CreatedAt}\n\nStatus: {(task.IsComplete ? "Complete" : "Incomplete")}";
            }
            else
            {
                TaskDetailsText.Text = string.Empty;
            }
        }

        // Mark selected task as complete
        private void MarkComplete_Click(object sender, RoutedEventArgs e)
        {
            if (TaskList.SelectedItem == null)
            {
                MessageBox.Show("Select a task first.");
                return;
            }

            var item = TaskList.SelectedItem as string;

            if (!int.TryParse(item?.Split(':')[0], out int id))
            {
                MessageBox.Show("Invalid selection.");
                return;
            }

            var manager = new TaskManager();
            manager.MarkAsComplete(id);
            RefreshTaskList();
        }

        // Edit Selected: populate inputs for editing (simple replace on add)
        private void EditSelected_Click(object sender, RoutedEventArgs e)
        {
            if (TaskList.SelectedItem == null)
            {
                MessageBox.Show("Select a task to edit.");
                return;
            }

            var item = TaskList.SelectedItem as string;

            if (!int.TryParse(item?.Split(':')[0], out int id))
            {
                MessageBox.Show("Invalid selection.");
                return;
            }

            var manager = new TaskManager();
            var task = manager.GetAllTasks().Find(t => t.Id == id);

            if (task != null)
            {
                // Populate inputs for editing and store id in Tag for Save
                TaskInput.Text = task.Title;
                DescriptionInput.Text = task.Description;
                ReminderInput.Text = task.Reminder;

                // Store id on Add Task button so Save can update instead of create
                AddTaskButtonTag = id;
                AddTaskButton.Content = "Save";
            }
        }

        // Temporary fields to support edit/save flow
        private int? AddTaskButtonTag = null;
        // We'll find the button by name at runtime

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (TaskList.SelectedItem != null)
            {
                var item = TaskList.SelectedItem as string;

                if (int.TryParse(item?.Split(':')[0], out int id))
                {
                    var manager = new TaskManager();
                    manager.DeleteTask(id);

                    RefreshTaskList();
                }
            }
            else
            {
                MessageBox.Show("Please select a task to delete.");
            }
        }

        private void SetReminder_Click(object sender, RoutedEventArgs e)
        {
            if (TaskList.SelectedItem != null)
            {
                var item = TaskList.SelectedItem as string;

                if (int.TryParse(item?.Split(':')[0], out int id))
                {
                    var reminder = ReminderInput.Text;

                    var manager = new TaskManager();
                    manager.SetReminder(id, reminder);

                    RefreshTaskList();
                }
            }
            else
            {
                MessageBox.Show("Please select a task to set a reminder for.");
            }
        }

        private void RefreshTaskList()
        {
            var manager = new TaskManager();
            var tasks = manager.GetAllTasks();

            TaskList.Items.Clear();

            foreach (var t in tasks)
            {
                string display = $"{t.Id}: {t.Title}";

                if (!string.IsNullOrEmpty(t.Reminder))
                    display += $" (Rem: {t.Reminder})";

                if (t.IsComplete)
                    display = "✔ " + display;

                TaskList.Items.Add(display);
            }
        }

        // Quiz UI handlers
        private void StartQuiz_Click(object sender, RoutedEventArgs e)
        {
            _quizManager.StartQuiz();
            QuizResultText.Text = string.Empty;
            QuizStatusText.Text = string.Empty;
            ShowCurrentQuizQuestion();
        }

        private void NextQuiz_Click(object sender, RoutedEventArgs e)
        {
            if (_quizManager.IsLastQuestion())
            {
                ShowQuizSummary();
            }
            else
            {
                _quizManager.NextQuestion();
                ShowCurrentQuizQuestion();
            }
        }

        // New Done button handler: finish quiz and show results immediately
        private void DoneQuiz_Click(object sender, RoutedEventArgs e)
        {
            if (!_quizManager.IsQuizActive())
            {
                QuizResultText.Text = "No active quiz. Press Start to begin.";
                return;
            }

            ShowQuizSummary();
        }

        private void ShowCurrentQuizQuestion()
        {
            var q = _quizManager.GetCurrentQuestion();

            if (q == null)
                return;
            // Show the question title separately and build a numbered list of choices
            // Ensure question appears both as a title and inside the choices text area
            QuizQuestionTitle.Text = q.QuestionText;

            var choicesText = "";
            for (int i = 0; i < q.Choices.Count; i++)
            {
                choicesText += $"{i + 1}. {q.Choices[i]}\n";
            }

            // We display answers as interactive buttons to avoid duplicated text
            QuizQuestionText.Text = string.Empty;
            QuizQuestionText.Visibility = Visibility.Collapsed;

            // Clear previous status and ensure buttons are enabled
            QuizStatusText.Text = string.Empty;

            QuizChoicesPanel.Children.Clear();

            // Create buttons for interactive answering — prefix button content with option number
            for (int i = 0; i < q.Choices.Count; i++)
            {
                var btn = new Button
                {
                    Content = $"{i + 1}. {q.Choices[i]}",
                    Margin = new Thickness(4),
                    Tag = i,
                    IsEnabled = true,
                    Style = (Style)FindResource("QuizChoiceButtonStyle")
                };

                btn.Click += (s, e) =>
                {
                    int idx = (int)((Button)s).Tag;
                    var currentQuestion = _quizManager.GetCurrentQuestion();
                    bool correct = _quizManager.AnswerCurrent(idx);

                    if (correct)
                        QuizStatusText.Text = "Correct!";
                    else
                        QuizStatusText.Text = $"Incorrect. Correct answer: {currentQuestion.Choices[currentQuestion.CorrectIndex]}";

                    ActivityLogger.Log($"Quiz answered: {(correct ? "correct" : "incorrect")}");

                    // Disable choice buttons after answering to prevent multiple submissions
                    foreach (var child in QuizChoicesPanel.Children)
                    {
                        if (child is Button b) b.IsEnabled = false;
                    }

                    // If this was the last question, show final score and clear choices
                    if (_quizManager.IsLastQuestion())
                    {
                        ShowQuizSummary();
                    }
                };

                QuizChoicesPanel.Children.Add(btn);
            }

        }

        // Shows final score and wrong-answer details
        private void ShowQuizSummary()
        {
            var scoreMsg = _quizManager.FinishQuiz();
            QuizQuestionTitle.Text = "Quiz Complete";
            QuizQuestionText.Visibility = Visibility.Visible;
            QuizQuestionText.Text = scoreMsg;

            // Fill detailed wrong answers list area
            QuizResultText.Text = _quizManager.GetWrongSummary();

            QuizChoicesPanel.Children.Clear();
            QuizStatusText.Text = string.Empty;
        }
        

        // Activity log handlers (connected from XAML)
        private void ShowRecentActivity_Click(object sender, RoutedEventArgs e)
        {
            _activityDisplayCount = 5;
            ActivityLogText.Text = ActivityLogger.GetRecentLog(_activityDisplayCount);
        }

        private void ShowMoreActivity_Click(object sender, RoutedEventArgs e)
        {
            _activityDisplayCount = System.Math.Min(ActivityLogger.GetCount(), _activityDisplayCount + 5);
            ActivityLogText.Text = ActivityLogger.GetRecentLog(_activityDisplayCount);
        }
    }
}