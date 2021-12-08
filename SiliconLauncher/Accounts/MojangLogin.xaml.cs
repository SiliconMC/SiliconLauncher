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
            Thread.Sleep(1500);
            if (File.Exists(Globals.SiliconData + "\\Silicon\\account.json"))
            {
                MainWindow mainWin = new MainWindow();
                mainWin.Show();
                Close();
            }

            InitializeComponent();
            Main = this;
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

            if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
            {
                MessageBox.Show("Fill in your Mojang credentials.");
                return;
            }

            if (MojangAccounts.Authenticate(username, password))
            {
                MainWindow mainWin = new MainWindow();
                mainWin.Show();
                Close();
            }

        }

        internal static MojangLogin Main;

    }
}
