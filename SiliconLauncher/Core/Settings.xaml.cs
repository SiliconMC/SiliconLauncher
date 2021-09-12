using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
