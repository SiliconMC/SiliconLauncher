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
using RestSharp.Authenticators;

namespace SiliconLauncher
{
    class MojangAccounts
    {
        public static bool Authenticate(string username, string password)
        {
            var SiliconData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
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
            Root body = JsonConvert.DeserializeObject<Root>(response.Content);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                MessageBox.Show("Incorrect credentials.");
                return false;
            } else
            {
                Config config = new Config
                {
                    accessToken = body.accessToken,
                    username = body.selectedProfile.name,
                    uuid = body.selectedProfile.id,
                    isMsft = false

                };
                Directory.CreateDirectory(SiliconData + "\\Silicon");
                File.WriteAllText(SiliconData + "\\Silicon\\account.json", JsonConvert.SerializeObject(config));
                Thread.Sleep(1500);
                return true;
            }
        }

        /*public static bool Validate(string accessToken, string clientToken)
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
        }*/

        public static void Logout(string accessToken)
        {
            var SiliconData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            File.Delete(SiliconData + "\\Silicon\\account.json");
            MainWindow.mainWin.WelcomeBackLabel.Content = "Logged out.";
            MainWindow.mainWin.LoggedInAsLabel.Content = "Restart Silicon to relogin.";
            MainWindow.mainWin.LaunchButton.IsEnabled = false;
            MainWindow.mainWin.SettingsButton.IsEnabled = false;
            MainWindow.mainWin.LogOutButton.IsEnabled = false;
            MainWindow.mainWin.AvatarImage.Source = new BitmapImage(new Uri("https://minecraftfaces.com/wp-content/bigfaces/big-steve-face.png"));
        }

        public class SelectedProfile
        {
            public string name { get; set; }
            public string id { get; set; }
        }

        public class AvailableProfile
        {
            public string name { get; set; }
            public string id { get; set; }
        }

        public class Root
        {
            public string clientToken { get; set; }
            public string accessToken { get; set; }
            public SelectedProfile selectedProfile { get; set; }
            public List<AvailableProfile> availableProfiles { get; set; }
        }
    }

    class MicrosoftAccounts
    {
        public static void AuthenticateXBL(string token)
        {
            var client = new RestClient("https://user.auth.xboxlive.com/user");
            var request = new RestRequest("authenticate", Method.POST);
            string json = JsonConvert.SerializeObject(new
            {
                Properties = new
                {
                    AuthMethod = "RPS",
                    SiteName = "user.auth.xboxlive.com",
                    RpsTicket = "d=" + token

                },
                RelyingParty = "http://auth.xboxlive.com",
                TokenType = "JWT"
            }, Formatting.Indented);

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddParameter("application/json", json, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            Root body = JsonConvert.DeserializeObject<Root>(response.Content);

            if (response.StatusCode.ToString() != "OK")
            {
                MessageBox.Show("Something went wrong. Try again.");
                SiliconHelper.Relaunch();
            }
            else
            {
                AuthenticateXSTS(body.Token);
            }
        }

        public static void AuthenticateXSTS(string token)
        {
            var client = new RestClient("https://xsts.auth.xboxlive.com/xsts");
            var request = new RestRequest("authorize", Method.POST);
            string json = JsonConvert.SerializeObject(new
            {
                Properties = new
                {
                    SandboxId = "RETAIL",
                    UserTokens = new List<string> { token }

                },
                RelyingParty = "rp://api.minecraftservices.com/",
                TokenType = "JWT"
            }, Formatting.Indented);

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddParameter("application/json", json, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            Root body = JsonConvert.DeserializeObject<Root>(response.Content);

            AuthenticateMC(body.Token, body.DisplayClaims.xui[0].uhs);
        }

        public static void AuthenticateMC(string token, string userhash)
        {
            var client = new RestClient("https://api.minecraftservices.com/authentication");
            var request = new RestRequest("login_with_xbox", Method.POST);
            string json = JsonConvert.SerializeObject(new
            {
                identityToken = "XBL3.0 x=" + userhash + ";" + token + '"'
            }, Formatting.Indented);

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddParameter("application/json", json, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            Root body = JsonConvert.DeserializeObject<Root>(response.Content);

            GetProfile(body.access_token);

        }

        public static void GetProfile(string token)
        {
            var client = new RestClient("https://api.minecraftservices.com/minecraft");
            var request = new RestRequest("profile", Method.GET);
            client.Authenticator = new JwtAuthenticator(token);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");

            IRestResponse response = client.Execute(request);
            Root body = JsonConvert.DeserializeObject<Root>(response.Content);

            if (body.error == "NOT_FOUND" || body.error == "UnauthorizedOperationException")
            {
                MessageBox.Show("Error while signing in: " + body.error + ". You do not own Minecraft on this account.");
                SiliconHelper.Relaunch();
            }
            else if (response.StatusCode != HttpStatusCode.OK)
            {
                MessageBox.Show("Something went wrong. Please try again.");
                SiliconHelper.Relaunch();
            }
            else
            {
                Config config = new Config
                {
                    accessToken = token,
                    username = body.name,
                    uuid = body.id,
                    isMsft = true
                };

                var SiliconData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
                MessageBox.Show("7");
                Directory.CreateDirectory(SiliconData + "\\Silicon");
                MessageBox.Show("8");
                File.WriteAllText(SiliconData + "\\Silicon\\account.json", JsonConvert.SerializeObject(config));
                Thread.Sleep(1500);
                SiliconHelper.Relaunch();
            }

        }

        public class Xui
        {
            public string uhs { get; set; }
        }

        public class DisplayClaims
        {
            public List<Xui> xui { get; set; }
        }

        public class Root
        {
            public DateTime IssueInstant { get; set; }
            public DateTime NotAfter { get; set; }
            public string Token { get; set; }
            public DisplayClaims DisplayClaims { get; set; }
            public string access_token { get; set; }
            public string accesstoken { get; set; }
            public string id { get; set; }
            public string name { get; set; }
            public string path { get; set; }
            public string errorType { get; set; }
            public string error { get; set; }
            public string errorMessage { get; set; }
            public string developerMessage { get; set; }
        }

        public static void Logout()
        {
            var SiliconData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            File.Delete(SiliconData + "\\Silicon\\account.json");
        }

    }

    public class Config
    {
        public string accessToken { get; set; }
        public string username { get; set; }
        public string uuid { get; set; }
        public bool isMsft { get; set; }
    }
}
