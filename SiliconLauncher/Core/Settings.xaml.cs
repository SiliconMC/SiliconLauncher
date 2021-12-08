using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using Newtonsoft.Json;
using MessageBox = System.Windows.MessageBox;

namespace SiliconLauncher
{
    public partial class Settings
    {
        public Settings()
        {
            if(!File.Exists(Globals.SiliconData + "\\Silicon\\launcher_settings.json"))
            {
                MessageBox.Show("No config file found. Generating new config.");
                Helpers.SiliconHelper.Relaunch();
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
                        JavaLocationBox.Text = fbd.SelectedPath;
                    }
                    else
                    {
                        JavaLocationBox.Text = "Please try again.";
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
            
            try
            {
                Launcher_Settings oldSettings = JsonConvert.DeserializeObject<Launcher_Settings>(File.ReadAllText(Globals.SiliconData + "\\Silicon\\launcher_settings.json"));
                

                if (String.IsNullOrEmpty(JavaLocationBox.Text) || JavaLocationBox.Text == "Please try again.")
                {
                    JavaLocationBox.Text = JsonConvert.DeserializeObject<Launcher_Settings>(File.ReadAllText(Globals.SiliconData + "\\Silicon\\launcher_settings.json")).javaDirectory;

                }
                if (JavaLocationBox.Text != oldSettings.javaDirectory)
                {
                    JavaLocationBox.Text += "\\javaw.exe";

                }

                if (!File.Exists(JavaLocationBox.Text))
                {
                    MessageBox.Show("Choose a Java installation directory - should be selected to bin folder and contain 'javaw.exe'");
                    return;
                }
                Launcher_Settings settings = new Launcher_Settings
                {
                    memoryMax = Convert.ToInt32(MemoryBox.Text),
                    javaDirectory = JavaLocationBox.Text,
                    developer_Settings = new Developer_Settings
                    {
                        bypassJavaWarning = (bool)JavaBypass_CheckBox.IsChecked
                    }
                };
                Directory.CreateDirectory(Globals.SiliconData + "\\Silicon");
                File.WriteAllText(Globals.SiliconData + "\\Silicon\\launcher_settings.json", JsonConvert.SerializeObject(settings));

                if ((bool)GameInstallReset_CheckBox.IsChecked) 
                {
                    Helpers.SiliconHelper.DeleteDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\deps"); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong when saving your profile. Reinstall the client and try again. Exception: " + ex);
            }

            ApplyButton.Content = "SUCCESSFULLY SAVED";
            ReturnButton.Content = "RETURN";
        }
    }
}
