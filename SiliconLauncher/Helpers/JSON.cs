using System;
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

    public class Root
    {
        public Silicon silicon { get; set; }
        public string currentClientVersion { get; set; }
        public string currentLauncherVersion { get; set; }
        public string currentServerVersion { get; set; }
        public string version { get; set; }
    }
}
