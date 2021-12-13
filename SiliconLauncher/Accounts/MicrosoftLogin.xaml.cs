

using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json;
using SiliconLauncher.Helpers;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Windows;
using System.Windows.Media.Imaging;

namespace SiliconLauncher
{
    public partial class MicrosoftLogin
    {
        public MicrosoftLogin()
        {

            if (File.Exists(Globals.SiliconData + "\\Silicon\\account.json"))
            {
                MainWindow mainWin = new MainWindow();
                mainWin.Show();
                Close();
            }

            InitializeComponent();
            Main = this;

            Loaded += (s, e) =>
            {
                MinimizeButton.IsEnabled = true;
                MaximizeRestoreButton.IsEnabled = false;
                CloseButton.IsEnabled = true;

                Process.Start("https://login.live.com/oauth20_authorize.srf ?client_id=e562da4a-5002-4efd-8667-e74618f2d85d&response_type=code&redirect_uri=https://silicon-auth.jacksta.workers.dev/auth/microsoft&scope=XboxLive.signin offline_access");
            };

        }


        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var token = TokenBox.Password;

            if (String.IsNullOrEmpty(token))
            {
                MessageBox.Show("Fill in your Microsoft token.");
                return;
            }

            MicrosoftAccounts.AuthenticateXbl(token);
        }

        internal static MicrosoftLogin Main;

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            Close();
        }
    }
}

