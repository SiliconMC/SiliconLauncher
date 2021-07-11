using System;
using System.Windows;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Net;

namespace SiliconLauncher
{
    public partial class MicrosoftLogin
    {
        public MicrosoftLogin()
        {
            string SiliconData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            Directory.CreateDirectory(SiliconData + "\\Silicon");
            Thread.Sleep(1500);
            if (File.Exists(SiliconData + "\\Silicon\\microsoft_account.json"))
            {
                MainWindow mainWin = new MainWindow();
                mainWin.Show();
                Close();
            }


           
            InitializeComponent();


            main = this;
            Loaded += (s, e) =>
            {
                MinimizeButton.IsEnabled = true;
                MaximizeRestoreButton.IsEnabled = false;
                CloseButton.IsEnabled = true;

                if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\sxbla.exe"))
                {
                    using (WebClient webClient = new WebClient())
                    {
                        webClient.DownloadFile("https://github.com/SiliconMC/SXBLA/releases/latest/download/sxbla.exe", AppDomain.CurrentDomain.BaseDirectory + @"\sxbla.exe");
                        Process.Start("https://login.live.com/oauth20_authorize.srf ?client_id=e562da4a-5002-4efd-8667-e74618f2d85d&response_type=code&redirect_uri=https://silicon.jacksta.dev/api/auth/microsoft/success&scope=XboxLive.signin offline_access");
                    }

                    
                } else
                {
                    Process.Start("https://login.live.com/oauth20_authorize.srf ?client_id=e562da4a-5002-4efd-8667-e74618f2d85d&response_type=code&redirect_uri=https://silicon.jacksta.dev/api/auth/microsoft/success&scope=XboxLive.signin offline_access");
                }

            };

        }


        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var token = TokenBox.Text;

            if (String.IsNullOrEmpty(token) == true)
            {
                MessageBox.Show("Fill in your Microsoft token.");
            }

            MicrosoftAccounts.Authenticate(token);
            
        }

        internal static MicrosoftLogin main;

    }
}
