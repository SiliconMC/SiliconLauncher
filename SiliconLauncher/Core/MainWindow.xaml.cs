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
                bool isMicrosoft = false;

                var SiliconData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
                if (!File.Exists(SiliconData + "\\Silicon\\account.json"))
                {
                    accountJson = "\\Silicon\\microsoft_account.json";
                    isMicrosoft = true;
                }
                Account userInfo = JsonConvert.DeserializeObject<Account>(File.ReadAllText(SiliconData + accountJson));
                MinimizeButton.IsEnabled = true;
                MaximizeRestoreButton.IsEnabled = false;
                CloseButton.IsEnabled = true;
                LoggedInAsLabel.Content = "Logged in as " + userInfo.SelectedProfile.Name;
                AvatarImage.Source = new BitmapImage(new Uri("https://crafatar.com/avatars/" + userInfo.SelectedProfile.Id + ".png"));
            };
        }

        private void LaunchButton_Click(object sender, RoutedEventArgs e)
        {
            var accountJson = "\\Silicon\\account.json";
            bool isMicrosoft = false;

            var SiliconData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            if (!File.Exists(SiliconData + "\\Silicon\\account.json"))
            {
                accountJson = "\\Silicon\\microsoft_account.json";
                isMicrosoft = true;
            }
            Account userInfo = JsonConvert.DeserializeObject<Account>(File.ReadAllText(SiliconData + accountJson));

            SiliconHelper.LaunchGame(userInfo.AccessToken, userInfo.SelectedProfile.Id, userInfo.SelectedProfile.Name);
        }

        private void LaunchButton_Relaunch(object sender, RoutedEventArgs e)
        {
            var accountJson = "\\Silicon\\account.json";
            var SiliconData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));

            if (!File.Exists(SiliconData + "\\Silicon\\account.json"))
            {
                accountJson = "\\Silicon\\microsoft_account.json";
            }

            File.Delete(SiliconData + accountJson);
            SiliconHelper.Relaunch();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            var accountJson = "\\Silicon\\account.json";
            bool isMicrosoft = false;

            var SiliconData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            if (!File.Exists(SiliconData + "\\Silicon\\account.json"))
            {
                accountJson = "\\Silicon\\microsoft_account.json";
                isMicrosoft = true;
            }
            Account userInfo = JsonConvert.DeserializeObject<Account>(File.ReadAllText(SiliconData + accountJson));

            if (isMicrosoft)
            {
                MicrosoftAccounts.Logout();
            } else
            {
                MojangAccounts.Logout(userInfo.AccessToken, userInfo.ClientToken);
            }
            SiliconHelper.LoggingOut("User initiated log out.");
            LaunchButton.Click -= LaunchButton_Click;
            LaunchButton.IsEnabled = true;
            LaunchButton.Click += new RoutedEventHandler(LaunchButton_Relaunch);
        }

        public class SelectedProfile
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("id")]
            public string Id { get; set; }
        }

        public class AvailableProfile
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("id")]
            public string Id { get; set; }
        }

        public class Account
        {
            [JsonProperty("clientToken")]
            public string ClientToken { get; set; }

            [JsonProperty("accessToken")]
            public string AccessToken { get; set; }

            [JsonProperty("selectedProfile")]
            public SelectedProfile SelectedProfile { get; set; }

            [JsonProperty("availableProfiles")]
            public List<AvailableProfile> AvailableProfiles { get; set; }
        }

        internal static MainWindow mainWin;

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Settings settingsWin = new Settings();
            settingsWin.Show();
            Hide();
        }
    }
}