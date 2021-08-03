﻿using System.Linq;
using UnityEditor;
using UnityEngine;
using VRCToolkit.VRCPackageManager.Editor.GitHub;
using VRCToolkit.VRCPackageManager.Editor.Settings;
using VRCToolkit.VRCPackageManager.Editor.VRCPackage;

namespace VRCToolkit.VRCPackageManager.Editor.Screen
{
    public class PackageScreen : VRCPackageManagerScreen
    {
        private int selectedPage;
        private Vector2 scrollPosition;

        public override void OnGUI()
        {
            base.OnGUI();
            if (CheckIfPlaying()) return;

            VRCPackage.VRCPackageManager.LoadDataFromFile(false);
            DrawPageTitles();
            DrawMainContent();
            DrawFooter();
        }
        
        private void DrawPageTitles()
        {
            selectedPage = GUILayout.Toolbar(selectedPage, VRCPackage.VRCPackageManager.GetPageTitles());
        }

        private void DrawMainContent()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUIStyle.none, GUIStyle.none);
            DrawPage(selectedPage);
            GUILayout.EndScrollView();
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
                VRCPackageManagerWindow.selectedScreen = 0;
                SettingsManager.settings.installedSDK = null;
                SettingsManager.installedVersions.Clear();
                SettingsManager.SaveSettings();
            }
        }

        private static void DrawPage(int pageID)
        {
            var section = VRCPackage.VRCPackageManager.pages[pageID];
            foreach (var package in section.packages)
            {
                DrawVRCPackage(package);   
            }
        }
        
        private static void DrawVRCPackage(VRCPackage.VRCPackage package)
        {
            DrawCenteredTitle(package.formattedName);
            GUILayout.Label(package.description, EditorStyles.wordWrappedLabel);
            var requirements = package.GetRequirements();
            if (!string.IsNullOrEmpty(requirements)) EditorGUILayout.HelpBox(new GUIContent($"This package will also install {requirements}"));
            
            GUILayout.BeginHorizontal();
            GUILayout.Space(100);
            var install = GUILayout.Button("Install Package", EditorStyles.miniButton);
            var openRepo = GUILayout.Button("Open Repository", EditorStyles.miniButton);
            GUILayout.Space(100);
            GUILayout.EndHorizontal();
            GUILayout.Space(20);
            
            if (install)
            {
                VRCPackageInstallHandler.InstallVRCPackage(package.id);
            }

            if (openRepo)
            {
                Application.OpenURL(package.GetRepoURL());
            }
        }

    }
}