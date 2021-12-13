using System;
using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Media.Imaging;

namespace SiliconLauncher
{
    class SiliconHelper
    {

        public static string FindJava()
        {
            var path = Environment.GetEnvironmentVariable("PATH");
            var directories = path.Split(';');

            foreach (var dir in directories)
            {
                var fullpath = Path.Combine(dir, "javaw.exe");
                if (File.Exists(fullpath)) return fullpath;
            }

            return null;
        }

        public static void DeleteDirectory(string target_dir)
        {
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(target_dir, false);
        }

        public static void Relaunch()
        {
            Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        public static void Quit()
        {
            Application.Current.Shutdown();
        }

        public static bool CheckInternetConnection()
        {
            bool result = false;
            Ping check = new Ping();
            try
            {
                PingReply reply = check.Send("1.1.1.1", 3000);
                if (reply.Status == IPStatus.Success)
                    return true;
            }
            catch (Exception)
            {
                return false;
            }
            return result;
        }

        public static void LoggingOut(string subreason)
        {
            MainWindow.mainWin.WelcomeBackLabel.Content = "Logged out.";
            MainWindow.mainWin.LoggedInAsLabel.Content = "Restart Silicon to relogin.";
            MainWindow.mainWin.VersionText.Text = subreason;
            MainWindow.mainWin.PLAYText.Content = "RESTART";
            MainWindow.mainWin.AvatarImage.Source = new BitmapImage(new Uri(@"../Assets/account-notsignedin.png", UriKind.Relative));
        }
    }
}
