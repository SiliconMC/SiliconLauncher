using System;
using System.Windows;
using System.IO;
using System.Threading;

namespace SiliconLauncher
{
    public partial class Login
    {
        public Login()
        {
            var SiliconData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
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
            };

        }

        internal static Login main;

        private void MojangLoginButton_Click(object sender, RoutedEventArgs e)
        {
            MojangLogin mojangLogin = new MojangLogin();
            mojangLogin.Show();
            Close();
        }

        private void MicrosoftLoginButton_Click(object sender, RoutedEventArgs e)
        {
            MicrosoftLogin microsoftLogin = new MicrosoftLogin();
            microsoftLogin.Show();
            Close();
        }
    }
}
