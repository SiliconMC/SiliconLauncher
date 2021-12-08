using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;
using SiliconLauncher.Helpers;

namespace SiliconLauncher
{
    class Accounts
    {
        public static bool AccountSet(string token, string username, string uuid, bool isMsft)
        {
            try
            {
                Config config = new Config
                {
                    accessToken = token,
                    username = username,
                    uuid = uuid,
                    isMsft = isMsft,

                };
                Directory.CreateDirectory(Globals.SiliconData + "\\Silicon");
                File.WriteAllText(Globals.SiliconData + "\\Silicon\\account.json", JsonConvert.SerializeObject(config));
                Thread.Sleep(1500);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Something went wrong when saving your profile. Reinstall the client and try again. Exception: " + e);
                return false;
            }
        }
    }



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
                username,
                password
            }, Formatting.Indented);

            request.AddParameter("application/json", json, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            Root body = JsonConvert.DeserializeObject<Root>(response.Content);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                MessageBox.Show("Incorrect credentials.");
                return false;
            }
            else
            {
                Accounts.AccountSet(body.accessToken, body.selectedProfile.name, body.selectedProfile.id, false);
                return true;
            }
        }

        public static void Logout(string accessToken)
        {
            File.Delete(Globals.SiliconData + "\\Silicon\\account.json");
            MainWindow.Main.WelcomeBackLabel.Content = "Logged out.";
            MainWindow.Main.LoggedInAsLabel.Content = "Restart Silicon to relogin.";
            MainWindow.Main.LaunchButton.IsEnabled = false;
            MainWindow.Main.SettingsButton.IsEnabled = false;
            MainWindow.Main.LogOutButton.IsEnabled = false;
            MainWindow.Main.AvatarImage.Source = new BitmapImage(new Uri("https://minecraftfaces.com/wp-content/bigfaces/big-steve-face.png"));
        }
    }

    class MicrosoftAccounts
    {
        public static void AuthenticateXbl(string token)
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
                AuthenticateXsts(body.Token);
            }
        }

        public static void AuthenticateXsts(string token)
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

            AuthenticateMinecraft(body.Token, body.DisplayClaims.xui[0].uhs);
        }

        public static void AuthenticateMinecraft(string token, string userhash)
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
                Accounts.AccountSet(token, body.name, body.id, true);
                SiliconHelper.Relaunch();

            }

        }



        public static void Logout()
        {
            File.Delete(Globals.SiliconData + "\\Silicon\\account.json");
        }

    }
}
