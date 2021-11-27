using System;
using System.Diagnostics;
using System.Windows;

namespace SiliconLauncher
{
    public partial class Settings
    {
        public Settings()
        {
            InitializeComponent();
            var versionInfo = FileVersionInfo.GetVersionInfo(Application.ResourceAssembly.Location);
            string version = versionInfo.FileVersion;
            Loaded += (s, e) =>
            {
                SiliconVersion.Text = "You are using SiliconLauncher " + version;
            };
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AdonisWindow_Closed(object sender, EventArgs e)
        {
            MainWindow mainWin = new MainWindow();
            mainWin.Show();
            Close();
        }
    }
}
