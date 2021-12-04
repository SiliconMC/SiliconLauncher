using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;

namespace SiliconLauncher
{
    public partial class MicrosoftLogin
    {
        public MicrosoftLogin()
        {
            string SiliconData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            Directory.CreateDirectory(SiliconData + "\\Silicon");
            Thread.Sleep(1500);
            if (File.Exists(SiliconData + "\\Silicon\\account.json"))
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
                Process.Start("https://login.live.com/oauth20_authorize.srf ?client_id=e562da4a-5002-4efd-8667-e74618f2d85d&response_type=code&redirect_uri=https://silicon-auth.jacksta.workers.dev/auth/microsoft&scope=XboxLive.signin offline_access");
            };

        }


        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var token = TokenBox.Text;

            if (String.IsNullOrEmpty(token))
            {
                MessageBox.Show("Fill in your Microsoft token.");
                return;
            }

            MicrosoftAccounts.AuthenticateXBL(token);

        }

        internal static MicrosoftLogin main;

    }
}
