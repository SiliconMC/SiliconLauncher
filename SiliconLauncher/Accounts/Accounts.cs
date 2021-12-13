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

namespace SiliconLauncher
{
    class MicrosoftAccounts
    {
        public static void Logout()
        {
            try
            {
                var SiliconData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
                File.Delete(SiliconData + "\\Silicon\\account.json");
            } catch (Exception ex)
            {
                MessageBox.Show("Error while logging out. Exception: " + ex);
            } 
        }

    }
}
