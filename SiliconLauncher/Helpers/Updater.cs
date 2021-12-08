using Newtonsoft.Json;
using RestSharp;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace SiliconLauncher.Helpers
{
    class Updater
    {
        public static bool CheckLauncher()
        {
            var client = new RestClient("https://status.jacksta.dev/json");
            var request = new RestRequest("authenticate", Method.GET);

            IRestResponse response = client.Execute(request);
            Root body = JsonConvert.DeserializeObject<Root>(response.Content);

            var versionInfo = FileVersionInfo.GetVersionInfo(Application.ResourceAssembly.Location);
            string clientVersion = versionInfo.FileVersion;

            try
            {
                if (body == null) return false;

                if (body.silicon.launcher.prod.version != clientVersion) return true;

                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occured. Exception: " + e);
                return false;
            }
        }

        public static bool CheckClient()
        {
            var client = new RestClient("https://status.jacksta.dev/json");
            var request = new RestRequest("authenticate", Method.GET);

            IRestResponse response = client.Execute(request);
            Root body = JsonConvert.DeserializeObject<Root>(response.Content);

            var versionInfo = AppDomain.CurrentDomain.BaseDirectory;

            var accountJson = "\\deps\\information.json";

            Root version = JsonConvert.DeserializeObject<Root>(File.ReadAllText(versionInfo + accountJson));

            try
            {
                if (body == null) return false;

                if (body.silicon.client.prod.version != version.version) return true;

                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occured. Exception: " + e);
                return false;
            }

        }
    }
}
