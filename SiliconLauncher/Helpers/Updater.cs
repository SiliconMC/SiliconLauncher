using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using RestSharp;

namespace SiliconLauncher.Helpers
{
    class Updater
    {
        public static bool checkLauncher()
        {
            var client = new RestClient("https://silicon-api.jacksta.workers.dev/status");
            var request = new RestRequest("authenticate", Method.GET);

            IRestResponse response = client.Execute(request);
            Root body = JsonConvert.DeserializeObject<Root>(response.Content);

            var versionInfo = FileVersionInfo.GetVersionInfo(Application.ResourceAssembly.Location);
            string clientVersion = versionInfo.FileVersion;

            try
            {
                if (body == null) return false;

                if (body.currentLauncherVersion != clientVersion) return true;

                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occured. Exception: " + e);
                return false;
            }
        }

        public static bool checkClient()
        {
            var client = new RestClient("https://silicon-api.jacksta.workers.dev/status");
            var request = new RestRequest("authenticate", Method.GET);

            IRestResponse response = client.Execute(request);
            Root body = JsonConvert.DeserializeObject<Root>(response.Content);

            var versionInfo = AppDomain.CurrentDomain.BaseDirectory;

            var accountJson = "\\deps\\information.json";

            Root version = JsonConvert.DeserializeObject<Root>(File.ReadAllText(versionInfo + accountJson));

            try
            {
                if (body == null) return false;

                if (body.currentClientVersion != version.version) return true;

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
