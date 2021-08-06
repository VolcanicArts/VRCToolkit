using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace VRCToolkit.VRCPackageManager.Editor.Settings
{
    public static class SettingsManager
    {
        private static readonly string settingsFileLocation = $"{Application.dataPath}/VRCToolkit/VRCPackageManager/Editor/Resources/Settings.json";

        public static Settings settings;
        public static Dictionary<int, string> installedVersions;

        public static bool LoadSettings(bool reload)
        {
            if (settings != null && !reload) return false;
            
            installedVersions = new Dictionary<int, string>();
            if (!File.Exists(settingsFileLocation))
            {
                GenerateDefaultSettings();
                SaveSettings();
                return true;
            }

            var settingsJSON = File.ReadAllText(settingsFileLocation);
            settings = JsonUtility.FromJson<Settings>(settingsJSON);
            if (settings.installedPackages != null)
            {
                foreach (var packageVersion in settings.installedPackages)
                {
                    installedVersions.Add(packageVersion.packageID, packageVersion.installedVersion);
                }
            }

            return true;
        }

        public static void SaveSettings()
        {
            settings.installedPackages = new PackageVersion[installedVersions.Count];
            var index = 0;
            foreach (var entry in installedVersions)
            {
                settings.installedPackages[index] = new PackageVersion {packageID = entry.Key, installedVersion = entry.Value};
                index++;
            }

            File.WriteAllText(settingsFileLocation, JsonUtility.ToJson(settings, true));
        }

        public static void GenerateDefaultSettings()
        {
            settings = new Settings();
            installedVersions.Clear();
        }

        public static void SetAttributes()
        {
            if (!string.IsNullOrEmpty(settings.installedSDK)) VRCPackageManagerWindow.selectedScreen = 1;
        }
    }
}