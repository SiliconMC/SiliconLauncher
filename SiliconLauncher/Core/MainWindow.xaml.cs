using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Net.Http;
using System.Threading;
using Newtonsoft.Json;
using System.Net;
using System.Diagnostics;
using System.IO.Compression;
using System.ComponentModel;
using Microsoft.Toolkit.Uwp.Notifications;

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
                var accountJson = "\\Silicon\\account.json";

                var SiliconData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
                Account account = JsonConvert.DeserializeObject<Account>(File.ReadAllText(SiliconData + accountJson));
                MinimizeButton.IsEnabled = true;
                MaximizeRestoreButton.IsEnabled = false;
                CloseButton.IsEnabled = true;
                LoggedInAsLabel.Content = "Logged in as " + account.username;
                VersionText.Text = "Ready to start.";
                AvatarImage.Source = new BitmapImage(new Uri("https://crafatar.com/avatars/" + account.uuid + ".png"));

                SiliconHelper.checkUpdate();

                if(!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\deps") && !File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\deps\SiliconClient.jar") && !Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\deps\libraries"))
                {
                    PLAYText.Content = "DOWNLOAD";
                    LaunchButton.Click -= LaunchButton_Click;
                    ProgressBar.Opacity = 100;
                    LaunchButton.IsEnabled = true;
                    VersionText.Text = "";
                    LaunchButton.Click += new RoutedEventHandler(LaunchButton_Download);
                }
            };
        }

        private void LaunchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = "java.exe";
                psi.Arguments = " -version";
                psi.RedirectStandardError = true;
                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;

                Process pr = Process.Start(psi);
                string strOutput = pr.StandardError.ReadLine().Split(' ')[2].Replace("\"", "");

                if (!strOutput.Contains("1.8"))
                {
                    MessageBox.Show("You have the wrong version of Java installed. We will try launch the game, however unexpected errors and bugs may occur. You can disable this warning in the Settings.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("You need to install Java 8. Opening your browser now. Download Windows Offline (64-bit) for optimal performance. If you have Java installed, you may want to change the Java directory in the launcher settings. Exception: " + ex.Message + ".");
                Process.Start("https://www.java.com/en/download/manual.jsp");
            }

            var accountJson = "\\Silicon\\account.json";

            var SiliconData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            Account account = JsonConvert.DeserializeObject<Account>(File.ReadAllText(SiliconData + accountJson));

            SiliconHelper.LaunchGame(account.accessToken, account.uuid, account.username);
        }

        private void LaunchButton_Relaunch(object sender, RoutedEventArgs e)
        {
            var accountJson = "\\Silicon\\account.json";
            var SiliconData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));

            File.Delete(SiliconData + accountJson);
            SiliconHelper.Relaunch();
        }

        private void LaunchButton_Download(object sender, RoutedEventArgs e)
        {
            LaunchButton.IsEnabled = false;
            SettingsButton.IsEnabled = false;
            LogOutButton.IsEnabled = false;
            using (WebClient client = new WebClient())
            {
                client.DownloadFileAsync(new Uri("https://s3.ap-southeast-2.amazonaws.com/cdn.jacksta.dev/ShareX/2021/09/SiliconClient.zip"), AppDomain.CurrentDomain.BaseDirectory + @"\client.zip");
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(UpdateProgress);
                client.DownloadFileCompleted += new AsyncCompletedEventHandler(FinishDownload);
            }
        }

        private void UpdateProgress(object sender, DownloadProgressChangedEventArgs e)
        {
            ProgressBar.Value = e.ProgressPercentage;
        }

        private void FinishDownload(object sender, AsyncCompletedEventArgs e)
        {
            VersionText.Text = "Finishing up.";
            ZipFile.ExtractToDirectory(AppDomain.CurrentDomain.BaseDirectory + @"\client.zip", AppDomain.CurrentDomain.BaseDirectory + "\\deps");
            LaunchButton.Click -= LaunchButton_Download;
            ProgressBar.Opacity = 0;
            LaunchButton.Click += new RoutedEventHandler(LaunchButton_Click);
            PLAYText.Content = "PLAY";
            VersionText.Text = "Ready to start.";
            LaunchButton.IsEnabled = true;
            LogOutButton.IsEnabled = true;
            SettingsButton.IsEnabled = true;
            File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"\client.zip");

        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            var accountJson = "\\Silicon\\account.json";
            bool isMicrosoft = false;

            var SiliconData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            Account account = JsonConvert.DeserializeObject<Account>(File.ReadAllText(SiliconData + accountJson));

            if (isMicrosoft)
            {
                MicrosoftAccounts.Logout();
            }
            else
            {
                MojangAccounts.Logout(account.accessToken);
            }
            SiliconHelper.LoggingOut("User initiated log out.");
            LaunchButton.Click -= LaunchButton_Click;
            LaunchButton.IsEnabled = true;
            LaunchButton.Click += new RoutedEventHandler(LaunchButton_Relaunch);
        }

        public class Account
        {
            public string accessToken { get; set; }
            public string username { get; set; }
            public string uuid { get; set; }
            public bool isMsft { get; set; }
        }

        internal static MainWindow mainWin;

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Settings settingsWin = new Settings();
            settingsWin.Show();
            Hide();
        }

        private void AdonisWindow_Closed(object sender, EventArgs e)
        {
            ToastNotificationManagerCompat.Uninstall();
            Environment.Exit(1);
        }
    }
}