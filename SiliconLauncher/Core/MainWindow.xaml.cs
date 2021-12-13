using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json;
using SiliconLauncher.Helpers;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Windows;
using System.Windows.Media.Imaging;

namespace SiliconLauncher
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            mainWin = this;

            Loaded += (s, e) =>
            {
                MinimizeButton.IsEnabled = true;
                MaximizeRestoreButton.IsEnabled = false;
                CloseButton.IsEnabled = true;
                LoggedInAsLabel.Content = "Silicon game launching is currently disabled.";
                VersionText.Text = "Click above to learn more.";

                MicrosoftAccounts.Logout();

                if (Globals.isConnected)
                {
#if !DEBUG
                    if (Updater.checkLauncher())
                    {
                        PLAYText.Content = "UPDATE SILICON";
                        LaunchButton.Click -= Error_Click;
                        PLAYText.IsEnabled = true;
                        LaunchButton.IsEnabled = true;
                        VersionText.Text = "Launcher update required.";
                        LaunchButton.Click += new RoutedEventHandler(LaunchButton_UpdateLauncher);
                    }
#endif
                }
                else
                {
                    AvatarImage.Source = new BitmapImage(new Uri(@"../Assets/account-notsignedin.png", UriKind.Relative));
                }
            };
        }

        private void LaunchButton_UpdateLauncher(object sender, RoutedEventArgs e)
        {
            LoggedInAsLabel.Content = "Downloading SiliconLauncher update.";
            VersionText.Text = "Please wait...";
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadFileAsync(new Uri("https://github.com/SiliconMC/siliconlauncher/releases/latest/download/SiliconInstaller.exe"), AppDomain.CurrentDomain.BaseDirectory + @"\siliconinstaller-latest.exe");
                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(FinishDownload_Launcher);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured. Please uninstall SiliconLauncher via Programs and Features. Exception: " + ex);
            }
        }

        private void FinishDownload_Launcher(object sender, AsyncCompletedEventArgs e)
        {
            MessageBox.Show("Silicon has downloaded the installer. After installation you may need to manually start Silicon.");
            Process.Start(AppDomain.CurrentDomain.BaseDirectory + @"\siliconinstaller-latest.exe");
            SiliconHelper.Quit();
        }

        private void Error_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Silicon game launching is currently disabled until v1.7.0.0. We strongly recommend to use the native Minecraft launcher to protect against the recent log4j exploits. When an update is available make sure you have an internet connection.", "Silicon");
        }

        private void LaunchButton_Relaunch(object sender, RoutedEventArgs e)
        {
            SiliconHelper.Relaunch();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MicrosoftAccounts.Logout();
            SiliconHelper.LoggingOut("User initiated log out.");
            LaunchButton.IsEnabled = true;
            LaunchButton.Click += new RoutedEventHandler(LaunchButton_Relaunch);
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            SiliconHelper.Quit();
        }

        public class Account
        {
            public string AccessToken { get; set; }
            public string Username { get; set; }
            public string Uuid { get; set; }
            public bool IsMsft { get; set; }
        }

        internal static MainWindow mainWin;

        private void AdonisWindow_Closed(object sender, EventArgs e)
        {
            ToastNotificationManagerCompat.Uninstall();
            Environment.Exit(1);
        }
    }
}