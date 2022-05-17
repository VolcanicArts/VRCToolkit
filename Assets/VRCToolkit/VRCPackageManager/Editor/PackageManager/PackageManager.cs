using System;
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

        private static string PackageDataLocation => $"{Application.dataPath}/VRCToolkit/VRCPackageManager/Editor/Resources/VRCPackages.json";

        public static void LoadDataFromFile(bool reload)
        {
            if (pages != null && !reload) return;
            var packageDataJson = File.ReadAllText(PackageDataLocation);
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

            packages = new Dictionary<int, Package>();
            foreach (var page in pages)
            foreach (var package in page.packages)
                packages.Add(package.id, package);
        }

        public static string[] GetPageTitles()
        {
            return pages == null ? Array.Empty<string>() : pages.Select(page => page.title).ToArray();
        }
    }
}