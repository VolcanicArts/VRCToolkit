using System.Linq;
using UnityEditor;
using UnityEngine;

namespace VRCToolkit.VRCPackageManager
{
    public class PackageScreen : VRCPackageManagerScreen
    {
        private Vector2 scrollPosition;
        private int selectedPage;
        private int selectedUpperPage;

        public override void OnGUI()
        {
            base.OnGUI();
            if (CheckIfPlaying()) return;

            selectedUpperPage = GUILayout.Toolbar(selectedUpperPage, new[] { "Install Packages", "View Installed Packages" });
            switch (selectedUpperPage)
            {
                case 0:
                    DrawInstallPackagesPage();
                    break;
                case 1:
                    DrawViewInstalledPackagedPage();
                    break;
            }
        }

        private void DrawInstallPackagesPage()
        {
            PackageManager.LoadDataFromFile(false);
            DrawPageTitles();
            DrawMainContent();
            DrawFooter();
        }

        private void DrawViewInstalledPackagedPage()
        {
            GUILayout.Space(10);
            DrawCenteredTitle("Installed Packages");
            DrawCenteredText(
                "If you've deleted a package and want VRCPackageManager to not try to update said package as it's not installed, click 'uninstall' here");
            GUILayout.Space(10);

            var installedVersionsCache = SettingsManager.installedVersions.Keys.ToList();
            foreach (var package in installedVersionsCache.Select(packageID => PackageManager.packages[packageID]))
            {
                DrawCenteredTitle($"{package.formattedName}: {SettingsManager.installedVersions[package.id]}");
                DrawCenteredText(package.description);
                var uninstall = DrawCenteredButton("Uninstall");
                if (uninstall)
                {
                    SettingsManager.installedVersions.Remove(package.id);
                    SettingsManager.SaveSettings();
                }
                GUILayout.Space(10);
            }
        }

        private void DrawPageTitles()
        {
            selectedPage = GUILayout.Toolbar(selectedPage, PackageManager.GetPageTitles());
            GUILayout.Space(10);
        }

        private void DrawMainContent()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.BeginVertical();
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUIStyle.none, GUIStyle.none);
            DrawPage(selectedPage);
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            GUILayout.Space(20);
            GUILayout.EndHorizontal();
        }

        private void DrawFooter()
        {
            DrawCenteredTitle("Settings");
            DrawCenteredTitle("Updates");
            var updateSDKOnStart = DrawCenteredToggle("Update SDK on start:", SettingsManager.settings.updateSDKOnStart);
            var updateSDK = DrawCenteredButton("Update SDK");
            var updatePackages = DrawCenteredButton("Update all installed packages");
            DrawCenteredTitle("Data Management");
            var clearDownloadCache = DrawCenteredButton("Clear download cache");
            var chooseNewSDK = DrawCenteredButton("Choose different SDK");
            GUILayout.Space(10);

            if (updateSDKOnStart != SettingsManager.settings.updateSDKOnStart)
            {
                SettingsManager.settings.updateSDKOnStart = updateSDKOnStart;
                SettingsManager.SaveSettings();
            }

            if (updateSDK)
            {
                var installedSDK = SettingsManager.settings.installedSDK;
                SDKInstallHandler.InstallSDK(installedSDK, SDKURLs.GetURL(installedSDK));
            }

            if (updatePackages)
            {
                var installedPackages = SettingsManager.installedVersions.Keys.ToList();
                installedPackages = installedPackages.OrderByDescending(x => x).ToList();
                foreach (var id in installedPackages) VRCPackageInstallHandler.InstallVRCPackage(id);
            }

            if (clearDownloadCache) AssetDatabase.DeleteAsset("Assets/VRCToolkit/VRCPackageManager/Downloads");
            if (chooseNewSDK)
            {
                SettingsManager.GenerateDefaultSettings();
                SettingsManager.SaveSettings();
                SettingsManager.SetAttributes();
            }
        }

        private static void DrawPage(int pageID)
        {
            var section = PackageManager.pages[pageID];
            foreach (var package in section.packages) DrawVRCPackage(package);
        }

        private static void DrawVRCPackage(Package package)
        {
            DrawCenteredTitle(package.formattedName);
            DrawCenteredText(package.description);
            var requirements = package.GetRequirements();
            if (!string.IsNullOrEmpty(requirements)) EditorGUILayout.HelpBox(new GUIContent($"This package will also install {requirements}"));

            GUILayout.BeginHorizontal();
            GUILayout.Space(100);
            var install = GUILayout.Button("Install Package", EditorStyles.miniButton);
            var openRepo = GUILayout.Button("Open Repository", EditorStyles.miniButton);
            GUILayout.Space(100);
            GUILayout.EndHorizontal();
            GUILayout.Space(20);

            if (install) VRCPackageInstallHandler.InstallVRCPackage(package.id);

            if (openRepo) Application.OpenURL(package.GetRepoURL());
        }
    }
}