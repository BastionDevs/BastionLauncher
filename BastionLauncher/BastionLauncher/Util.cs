using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TinyINIController;

namespace BastionLauncher
{
    internal class Util
    {

        public static string LauncherDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static string LauncherConfigFile = LauncherDir + @"\launcher.ini";

        public static IniFile file = new IniFile(LauncherConfigFile);

        public static JObject JREMasterList = JObject.Parse(new WebClient().DownloadString("https://launchermeta.mojang.com/v1/products/java-runtime/2ec0cc96c44e5a76b9c8b7c39df7210883d12871/all.json"));

        public static JObject LauncherUserProfiles = null;

        //Config
        //Launcher
        public static string MinecraftDir = "";
        //JRE
        public static string JRELocation = "";
        public static string JREAlphaInstalled = "";
        public static string JREGammaInstalled = "";
        public static string JREDeltaInstalled = "";
        public static string JRELegacyInstalled = "";
        public static string JRESelected = "";

        //Accounts
        public static string AccountSelected = "";

        public static void GetConfig()
        {
            MinecraftDir = file.Read("GameDir", "Launcher");

            JRELocation = file.Read("Location", "JRE");
            JREAlphaInstalled = file.Read("AlphaInstalled", "JRE");
            JREGammaInstalled = file.Read("GammaInstalled", "JRE");
            JREDeltaInstalled = file.Read("DeltaInstalled", "JRE");
            JRELegacyInstalled = file.Read("LegacyInstalled", "JRE");
            JRESelected = file.Read("SelectedJRE", "JRE");

            //user accounts
            LauncherUserProfiles = JObject.Parse(File.ReadAllText(LauncherDir + @"\blauncher_profiles.json"));
            AccountSelected = LauncherUserProfiles["SelectedUser"].ToString();

        }

        public static void SetConfig()
        {
            file.Write("GameDir", MinecraftDir, "Launcher");

            file.Write("Location", JRELocation, "JRE");
            file.Write("AlphaInstalled", JREAlphaInstalled, "JRE");
            file.Write("GammaInstalled", JREGammaInstalled, "JRE");
            file.Write("DeltaInstalled", JREDeltaInstalled, "JRE");
            file.Write("LegacyInstalled", JRELegacyInstalled, "JRE");
            file.Write("SelectedJRE", JRESelected, "JRE");

            LauncherUserProfiles["SelectedUser"] = AccountSelected;
            File.WriteAllText(LauncherDir + @"\blauncher_profiles.json", LauncherUserProfiles.ToString(Newtonsoft.Json.Formatting.Indented));
        }
    }
}
