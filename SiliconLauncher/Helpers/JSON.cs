﻿using System;
using System.Collections.Generic;

// Not sure if this is how I should handle JSON in Silicon and I am sure there is a better solution than this.
// If you are reading this and know a better solution you are more than welcome to create a PR with your changes. :)

namespace SiliconLauncher
{
    public class Prod
    {
        public string version { get; set; }
        public string download_link { get; set; }
    }

    public class Dev
    {
        public string version { get; set; }
    }

    public class Launcher
    {
        public Prod prod { get; set; }
        public Dev dev { get; set; }
    }

    public class Client
    {
        public Prod prod { get; set; }
        public Dev dev { get; set; }
    }

    public class Silicon
    {
        public Launcher launcher { get; set; }
        public Client client { get; set; }
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
        public string clientToken { get; set; }
        public string accessToken { get; set; }

        public Silicon silicon { get; set; }
        public SelectedProfile selectedProfile { get; set; }
        public List<AvailableProfile> availableProfiles { get; set; }
        public string currentClientVersion { get; set; }
        public string currentLauncherVersion { get; set; }
        public string currentServerVersion { get; set; }
        public string version { get; set; }
    }

    public class Config
    {
        public string accessToken { get; set; }
        public string username { get; set; }
        public string uuid { get; set; }
        public bool isMsft { get; set; }
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
}
