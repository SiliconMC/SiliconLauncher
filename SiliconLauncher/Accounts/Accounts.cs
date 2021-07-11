using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;
using Newtonsoft.Json;
using RestSharp;
using System.Threading;
using System.Windows.Media.Imaging;
using System.Diagnostics;

namespace SiliconLauncher
{
    class MojangAccounts
    {
        public static bool Authenticate(string username, string password)
        {
            var client = new RestClient("https://authserver.mojang.com");
            var request = new RestRequest("authenticate", Method.POST);
            string json = JsonConvert.SerializeObject(new
            {
                agent = new
                {
                    name = "Minecraft",
                    version = 1
                },
                username = username,
                password = password
            }, Formatting.Indented);

            request.AddParameter("application/json", json, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                MessageBox.Show("Incorrect credentials.");
                return false;
            } else
            {
                
                var SiliconData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
                Directory.CreateDirectory(SiliconData + "\\Silicon");
                File.WriteAllText(SiliconData + "\\Silicon\\account.json", response.Content);
                Thread.Sleep(1500);
                return true;
            }
        }

        public static bool Validate(string accessToken, string clientToken)
        {
            var client = new RestClient("https://authserver.mojang.com");
            var request = new RestRequest("validate", Method.POST);
            string json = JsonConvert.SerializeObject(new
            {
                accessToken = accessToken,
                clientToken = clientToken
            }, Formatting.Indented);

            request.AddParameter("application/json", json, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            if (response.StatusCode != HttpStatusCode.NoContent)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static void Logout(string accessToken, string clientToken)
        {
            var client = new RestClient("https://authserver.mojang.com");
            var request = new RestRequest("invalidate", Method.POST);
            string json = JsonConvert.SerializeObject(new
            {
                accessToken = accessToken,
                clientToken = clientToken
            }, Formatting.Indented);

            request.AddParameter("application/json", json, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            var SiliconData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            File.Delete(SiliconData + "\\Silicon\\account.json");
            MainWindow.mainWin.WelcomeBackLabel.Content = "Logged out.";
            MainWindow.mainWin.LoggedInAsLabel.Content = "Restart Silicon to relogin.";
            MainWindow.mainWin.LaunchButton.IsEnabled = false;
            MainWindow.mainWin.SettingsButton.IsEnabled = false;
            MainWindow.mainWin.LogOutButton.IsEnabled = false;
            MainWindow.mainWin.AvatarImage.Source = new BitmapImage(new Uri("https://minecraftfaces.com/wp-content/bigfaces/big-steve-face.png"));
        }
    }

    class MicrosoftAccounts
    {
        public static void Authenticate(string token)
        {
            var client = new RestClient("https://login.live.com");
            var request = new RestRequest("oauth20_token.srf", Method.POST);

            request.AddParameter("client_id", "e562da4a-5002-4efd-8667-e74618f2d85d", ParameterType.GetOrPost);
            request.AddParameter("redirect_uri", "https://silicon.jacksta.dev/api/auth/microsoft/success", ParameterType.GetOrPost);
            request.AddParameter("grant_type", "authorization_code", ParameterType.GetOrPost);
            request.AddParameter("client_secret", "0~H3~xil-kQW74rAp6RFBQgl.dF9gYt1uA", ParameterType.GetOrPost);
            request.AddParameter("code", token, ParameterType.GetOrPost);
            request.AddParameter("application/x-www-form-urlencoded", "", ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            PostTokenJSON json = JsonConvert.DeserializeObject<PostTokenJSON>(response.Content);

            var process = Process.Start(AppDomain.CurrentDomain.BaseDirectory + @"\sxbla.exe", json.access_token);
            process.WaitForExit();
            SiliconHelper.Relaunch();
        }

        public class PostTokenJSON
        {
            public string access_token { get; set; }
            public string refresh_token { get; set; }
        }

        public static void Validate()
        {
            
        }

        public static void Logout()
        {
            var SiliconData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            File.Delete(SiliconData + "\\Silicon\\microsoft_account.json");
        }

    }
}
