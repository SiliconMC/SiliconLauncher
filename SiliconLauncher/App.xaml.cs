using System;
using System.IO;
using System.Windows;
using Microsoft.VisualBasic.Devices;
using Newtonsoft.Json;
using SiliconLauncher.Helpers;

namespace SiliconLauncher
{
    public static class Globals
    {
        public static bool IsConnected = true;
        public static string SiliconData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
    }
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            if (!SiliconHelper.CheckInternetConnection())
            {
                Globals.IsConnected = false;
                MessageBox.Show(
                    "Silicon has detected no internet connection. Try basic troubleshooting steps to restore launcher functionality.");
            }

            if(!File.Exists(Globals.SiliconData + "\\Silicon\\launcher_settings.json"))
            {
                Launcher_Settings export = new Launcher_Settings
                {
                    memoryMax = Convert.ToInt32((new ComputerInfo().TotalPhysicalMemory / (1024 * 1024) / 4 + 16)),
                    javaDirectory = SiliconHelper.FindJava(),
                    developer_Settings = new Developer_Settings
                    {
                        bypassJavaWarning = false,
                    }

                };

                Directory.CreateDirectory(Globals.SiliconData + "\\Silicon");
                File.WriteAllText(Globals.SiliconData + "\\Silicon\\launcher_settings.json", JsonConvert.SerializeObject(export));


            }
        }
    }
}
