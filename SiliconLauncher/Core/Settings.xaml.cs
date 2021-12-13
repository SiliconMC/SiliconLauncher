using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using SiliconLauncher.Helpers;

namespace SiliconLauncher
{
    public partial class Settings
    {
        public Settings()
        {
            if (!File.Exists(Globals.SiliconData + "\\Silicon\\launcher_settings.json"))
            {
                MessageBox.Show("No config file found. Generating new config.");
                SiliconHelper.Relaunch();
            }

            Launcher_Settings settings = JsonConvert.DeserializeObject<Launcher_Settings>(File.ReadAllText(Globals.SiliconData + "\\Silicon\\launcher_settings.json"));
            InitializeComponent();
            var versionInfo = FileVersionInfo.GetVersionInfo(System.Windows.Application.ResourceAssembly.Location);
            string version = versionInfo.FileVersion;
            Loaded += (s, e) =>
            {
                SiliconVersion.Content = "You are using SiliconLauncher " + version;
                MemoryBox.Text = settings.memoryMax.ToString();
                JavaLocationBox.Text = settings.javaDirectory;
                JavaBypass_CheckBox.IsChecked = settings.developer_Settings.bypassJavaWarning;
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

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var fbd = new FolderBrowserDialog())
                {
                    DialogResult result = fbd.ShowDialog();

                    if (!string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        JavaLocationBox.Text = fbd.SelectedPath + "\\javaw.exe";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while setting download path. Exception caught: " + ex.ToString());
            }
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(JavaLocationBox.Text) || JavaLocationBox.Text == "Please try again.")
            {
                JavaLocationBox.Text = JsonConvert.DeserializeObject<Launcher_Settings>(File.ReadAllText(Globals.SiliconData + "\\Silicon\\launcher_settings.json")).javaDirectory;
            }

            if (!File.Exists(JavaLocationBox.Text))
            {
                MessageBox.Show("Choose a Java installation directory - should be selected to bin folder and contain the executable 'javaw.exe'");
                return;
            }

            Developer_Settings developer_Settings = new Developer_Settings
            {
                bypassJavaWarning = (bool)JavaBypass_CheckBox.IsChecked,
            };

            Launcher_Settings settings = new Launcher_Settings
            {
                memoryMax = Convert.ToInt32(MemoryBox.Text),
                javaDirectory = JavaLocationBox.Text,
                developer_Settings = developer_Settings
            };

            SiliconHelper.SaveSettings(settings);

            if ((bool)GameInstallReset_CheckBox.IsChecked)
            {
                SiliconHelper.DeleteDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\deps");
            }
            ApplyButton.Content = "SUCCESSFULLY SAVED";
            ReturnButton.Content = "RETURN";
        }
    }
}
