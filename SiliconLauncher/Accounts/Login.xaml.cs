using System.IO;
using System.Threading;
using System.Windows;

namespace SiliconLauncher
{
    public partial class Login
    {
        public Login()
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

        internal static Login Main;

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
