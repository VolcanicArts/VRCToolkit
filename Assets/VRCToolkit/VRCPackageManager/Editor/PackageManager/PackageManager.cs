using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace VRCToolkit.VRCPackageManager
{
    public static class PackageManager
    {
        public static PackagePage[] pages;
        public static Dictionary<int, Package> packages;

        public static void LoadDataFromFile(bool reload)
        {
            if (pages != null && !reload) return;
            var packageDataLocation = $"{Application.dataPath}/VRCToolkit/VRCPackageManager/Editor/Resources/VRCPackages.json";
            var packageDataJson = File.ReadAllText(packageDataLocation);
            var packageData = JsonUtility.FromJson<PackageData>(packageDataJson);

            SettingsManager.LoadSettings(false);
            switch (SettingsManager.settings.installedSDK)
            {
                case "SDK3World":
                    pages = packageData.SDK3World;
                    break;
                case "SDK3Avatar":
                    pages = packageData.SDK3Avatar;
                    break;
            }

            if (pages == null)
            {
                Debug.Log("No valid SDK found");
                return;
            }

            packages = new Dictionary<int, Package>();
            foreach (var page in pages)
            foreach (var package in page.packages)
                packages.Add(package.id, package);
        }

        public static string[] GetPageTitles()
        {
            if (pages == null) return new string[0];
            var pageTitles = new List<string>();
            pageTitles.AddRange(pages.Select(page => page.title));
            return pageTitles.ToArray();
        }
    }
}