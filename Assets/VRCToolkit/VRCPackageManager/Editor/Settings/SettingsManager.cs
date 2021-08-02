using System.IO;
using UnityEngine;

namespace VRCToolkit.VRCPackageManager.Editor.Settings
{
    public class SettingsManager
    {
        private static readonly string settingsFileLocation = $"{Application.dataPath}/VRCToolkit/VRCPackageManager/Editor/Resources/Settings.json";

        public static Settings settings;

        public static bool LoadSettings(bool reload)
        {
            if (settings != null && !reload) return false;
            if (!File.Exists(settingsFileLocation))
            {
                settings = new Settings();
                return true;
            }

            var settingsJSON = File.ReadAllText(settingsFileLocation);
            settings = JsonUtility.FromJson<Settings>(settingsJSON);
            return true;
        }

        public static void SaveSettings()
        {
            File.WriteAllText(settingsFileLocation, JsonUtility.ToJson(settings, true));
        }
    }
}