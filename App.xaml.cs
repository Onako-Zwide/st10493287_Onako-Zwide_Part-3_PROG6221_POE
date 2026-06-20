using System;
using System.Threading.Tasks;
using System.Windows;

namespace CyberSecurityChatbot_PART_2
{
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Show splash screen first
            SplashScreen splash = new SplashScreen();
            splash.Show();

            // Wait 3 seconds 
            await Task.Delay(3000);

            // Open main chatbot window
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            // Close splash screen
            splash.Close();
        }
    }
}
