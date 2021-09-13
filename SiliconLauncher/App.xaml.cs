using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Windows.Foundation.Collections;

namespace SiliconLauncher
{
    public static class Globals {
        public static bool isConnected;
    }
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            if (SiliconHelper.CheckInternetConnection())
            {
                Globals.isConnected = true;
            } else
            {
                Globals.isConnected = false;
                MessageBox.Show("Silicon has detected no internet connection. Try basic troubleshooting steps to restore launcher functionality.");
            }
            ToastNotificationManagerCompat.OnActivated += toastArgs =>
            {
                // Obtain the arguments from the notification
                ToastArguments args = ToastArguments.Parse(toastArgs.Argument);

                // Need to dispatch to UI thread if performing UI operations
                Current.Dispatcher.Invoke(delegate
                {

                });
            };
        }
    }
}
