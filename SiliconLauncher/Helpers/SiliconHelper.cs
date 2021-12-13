using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Media.Imaging;

namespace SiliconLauncher.Helpers
{
    class SiliconHelper
    {

        public static void LaunchGame(string accessToken, string uuid, string username)
        {
            Launcher_Settings settings = JsonConvert.DeserializeObject<Launcher_Settings>(File.ReadAllText(Globals.SiliconData + "\\Silicon\\launcher_settings.json"));
            var minecraftDirectory =
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.minecraft";
            var installDirectory = Environment.CurrentDirectory;
            var clientResources = installDirectory + "\\deps";
            var javaRuntime = settings.javaDirectory;
            var memoryMax = settings.memoryMax;

            string arguments =
                $@"-Djava.library.path={clientResources} -Dminecraft.client.jar={clientResources}\SiliconClient.jar -cp {clientResources}\libraries\com\mojang\netty\1.6\netty-1.6.jar;{clientResources}\libraries\java3d\vecmath\1.5.2\vecmath-1.5.2.jar;{clientResources}\libraries\net\sf\trove4j\trove4j\3.0.3\trove4j-3.0.3.jar;{clientResources}\libraries\com\ibm\icu\icu4j-core-mojang\51.2\icu4j-core-mojang-51.2.jar;{clientResources}\libraries\net\sf\jopt-simple\jopt-simple\4.6\jopt-simple-4.6.jar;{clientResources}\libraries\com\paulscode\codecjorbis\20101023\codecjorbis-20101023.jar;{clientResources}\libraries\com\paulscode\codecwav\20101023\codecwav-20101023.jar;{clientResources}\libraries\com\paulscode\libraryjavasound\20101123\libraryjavasound-20101123.jar;{clientResources}\libraries\com\paulscode\librarylwjglopenal\20100824\librarylwjglopenal-20100824.jar;{clientResources}\libraries\com\paulscode\soundsystem\20120107\soundsystem-20120107.jar;{clientResources}\libraries\io\netty\netty-all\4.0.15.Final\netty-all-4.0.15.Final.jar;{clientResources}\libraries\com\google\guava\guava\17.0\guava-17.0.jar;{clientResources}\libraries\org\apache\commons\commons-lang3\3.3.2\commons-lang3-3.3.2.jar;{clientResources}\libraries\commons-io\commons-io\2.4\commons-io-2.4.jar;{clientResources}\libraries\commons-codec\commons-codec\1.9\commons-codec-1.9.jar;{clientResources}\libraries\net\java\jinput\jinput\2.0.5\jinput-2.0.5.jar;{clientResources}\libraries\net\java\jutils\jutils\1.0.0\jutils-1.0.0.jar;{clientResources}\libraries\com\google\code\gson\gson\2.2.4\gson-2.2.4.jar;{clientResources}\libraries\com\mojang\authlib\1.5.21\authlib-1.5.21.jar;{clientResources}\libraries\com\mojang\realms\1.6.1\realms-1.6.1.jar;{clientResources}\libraries\com\mojang\netty\1.6\netty-1.6.jar;{clientResources}\libraries\org\apache\commons\commons-compress\1.8.1\commons-compress-1.8.1.jar;{clientResources}\libraries\org\apache\httpcomponents\httpclient\4.3.3\httpclient-4.3.3.jar;{clientResources}\libraries\commons-logging\commons-logging\1.1.3\commons-logging-1.1.3.jar;{clientResources}\libraries\org\apache\httpcomponents\httpcore\4.3.2\httpcore-4.3.2.jar;{clientResources}\libraries\org\apache\logging\log4j\log4j-api\2.0-beta9\log4j-api-2.0-beta9.jar;{clientResources}\libraries\org\apache\logging\log4j\log4j-core\2.0-beta9\log4j-core-2.0-beta9.jar;{clientResources}\libraries\org\lwjgl\lwjgl\lwjgl\2.9.1\lwjgl-2.9.1.jar;{clientResources}\libraries\org\lwjgl\lwjgl\lwjgl_util\2.9.1\lwjgl_util-2.9.1.jar;{clientResources}\libraries\tv\twitch\twitch\6.5\twitch-6.5.jar;{clientResources}\SiliconClient.jar -Xmx{memoryMax}m net.minecraft.client.main.Main --username {username} --version SiliconClient --gameDir {minecraftDirectory} --assetsDir {minecraftDirectory}\assets --assetIndex 1.8 --uuid {uuid} --accessToken {accessToken} --userProperties" +
                "{}" + "--userType mojang";
            try
            {
                Process.Start(javaRuntime, arguments);
            } catch (Exception ex)
            {
                MessageBox.Show("Something went wrong while launching... Exception: " + ex);
            }
        }

        public static void SaveSettings(Launcher_Settings settings)
        {
            try
            {
                
                Directory.CreateDirectory(Globals.SiliconData + "\\Silicon");
                File.WriteAllText(Globals.SiliconData + "\\Silicon\\launcher_settings.json", JsonConvert.SerializeObject(settings));
            } catch (Exception e)
            {
                MessageBox.Show("Something went wrong while saving your settings. Exception: " + e);
            }
        }

        public static string FindJava()
        {
            var path = Environment.GetEnvironmentVariable("PATH");
            var directories = path.Split(';');

            foreach (var dir in directories)
            {
                var fullpath = Path.Combine(dir, "javaw.exe");
                if (File.Exists(fullpath)) return fullpath;
            }

            return null;
        }

        public static void DeleteDirectory(string targetDir)
        {
            string[] files = Directory.GetFiles(targetDir);
            string[] dirs = Directory.GetDirectories(targetDir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(targetDir, false);
        }

        public static void Relaunch()
        {
            Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        public static void Quit()
        {
            Application.Current.Shutdown();
        }

        public static bool CheckInternetConnection()
        {
            bool result = false;
            Ping check = new Ping();
            try
            {
                PingReply reply = check.Send("1.1.1.1", 3000);
                if (reply.Status == IPStatus.Success)
                    return true;
            }
            catch (Exception e)
            {
                return false;
            }
            return result;
        }


        public static void LoggingOut(string subreason)
        {
            MainWindow.Main.WelcomeBackLabel.Content = "Logged out.";
            MainWindow.Main.LoggedInAsLabel.Content = "Restart Silicon to relogin.";
            MainWindow.Main.StatusText.Content = subreason;
            MainWindow.Main.LaunchButton.Content = "RESTART";
            MainWindow.Main.SettingsButton.IsEnabled = false;
            MainWindow.Main.LogOutButton.IsEnabled = false;
            MainWindow.Main.AvatarImage.Source = new BitmapImage(new Uri(@"../Assets/account-notsignedin.png", UriKind.Relative));
        }
    }
}
