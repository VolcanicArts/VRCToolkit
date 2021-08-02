using UnityEditor;
using UnityEngine;
using VRCToolkit.VRCPackageManager.Editor.GitHub;
using VRCToolkit.VRCPackageManager.Editor.VRCPackage;

namespace VRCToolkit.VRCPackageManager.Editor.Screen
{
    public class PackageScreen : VRCPackageManagerScreen
    {
        private const string GitHubRepoLatestDownload = "/releases/latest/download/";
        
        private int selectedPage;
        private Vector2 scrollPosition;

        public override void OnGUI()
        {
            base.OnGUI();
            if (CheckIfPlaying()) return;

            VRCPackage.VRCPackageManager.LoadDataFromFile(VRCPackageManagerWindow.installedSDK, false);
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
            var clearCachedData = DrawCenteredButton("Clear Cached Data");
            GUILayout.Space(20);
            var chooseNewSDK = DrawCenteredButton("Choose New SDK");

            if (clearCachedData) AssetDatabase.DeleteAsset("Assets/VRCToolkit/VRCPackageManager/Downloads");
            if (chooseNewSDK)
            {
                VRCPackageManagerWindow.selectedScreen = 0;
                VRCPackageManagerWindow.installedSDK = null;
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
                var latestVersion = GitHubUtil.GetLatestVersion(package.repoName, package.formattedName);
                if (latestVersion == null) return;
                var latestReleaseFileName = string.Format(package.fileNameFormat, package.formattedName, latestVersion);
                var latestReleaseURL = package.GetRepoURL() + GitHubRepoLatestDownload + latestReleaseFileName;
                var packageDownloader = new PackageDownloader(package.formattedName, latestReleaseURL, latestReleaseFileName);
                packageDownloader.ExecuteDownload();
            }

            if (openRepo)
            {
                Application.OpenURL(package.GetRepoURL());
            }
        }

    }
}