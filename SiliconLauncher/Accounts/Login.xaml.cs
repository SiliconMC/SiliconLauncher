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
    public partial class Login
    {
        public Login()
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

            };
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var username = EmailBox.Text;
            var password = PassBox.Password;

            if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
            {
                MessageBox.Show("Fill in your Mojang credentials.");
                return;
            }

            if (MojangAccounts.Authenticate(username, password))
            {
                MainWindow mainWin = new MainWindow();
                mainWin.Show();
                Close();
            }

        }

        private void MicrosoftLoginButton_Click(object sender, RoutedEventArgs e)
        {
            MicrosoftLogin microsoftLogin = new MicrosoftLogin();
            microsoftLogin.Show();
            Close();
        }

        internal static Login Main;
    }
}





