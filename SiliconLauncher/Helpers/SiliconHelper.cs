using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media.Imaging;

namespace SiliconLauncher
{
    class SiliconHelper
    {
        public static void LaunchGame(string accessToken, string uuid, string username)
        {
            MessageBox.Show("Coming soon.");
        }

        public static void Relaunch()
        {
            Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        public static void LoggingOut(string subreason)
        {
            MainWindow.mainWin.WelcomeBackLabel.Content = "Logged out.";
            MainWindow.mainWin.LoggedInAsLabel.Content = "Restart Silicon to relogin.";
            MainWindow.mainWin.VersionText.Text = subreason;
            MainWindow.mainWin.LaunchButton.Content = "Relaunch";
            MainWindow.mainWin.SettingsButton.IsEnabled = false;
            MainWindow.mainWin.LogOutButton.IsEnabled = false;
            MainWindow.mainWin.AvatarImage.Source = new BitmapImage(new Uri(@"../Assets/account-notsignedin.png", UriKind.Relative));
        }
    }
}
