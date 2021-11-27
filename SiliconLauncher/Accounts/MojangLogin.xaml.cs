using System;
using System.IO;
using System.Threading;
using System.Windows;

namespace SiliconLauncher
{
    public partial class MojangLogin
    {
        public MojangLogin()
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


        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var username = EmailBox.Text;
            var password = PassBox.Password;

            if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password) == true)
            {
                MessageBox.Show("Fill in your Mojang credentials.");
            }

            if (MojangAccounts.Authenticate(username, password) == true)
            {
                MainWindow mainWin = new MainWindow();
                mainWin.Show();
                Close();
            }

        }

        internal static MojangLogin main;

    }
}
