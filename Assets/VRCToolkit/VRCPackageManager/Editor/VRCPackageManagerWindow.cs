using UnityEditor;
using UnityEngine;
using VRCToolkit.VRCPackageManager.Editor.GitHub;

namespace VRCToolkit.VRCPackageManager.Editor
{
    public class VRCPackageManagerWindow : EditorWindow
    {
        private const string GitHubRepoLatestDownload = "/releases/latest/download/";
        
        private static Vector2 scrollPosition;
        private static int selectedPage;

        private void OnGUI()
        {
            VRCPackage.VRCPackageManager.LoadDataFromFile();
            DrawTitle();
            if (CheckIfPlaying()) return;
            DrawPageTitles();
            DrawMainContent();
            DrawFooter();
        }

        [MenuItem("VRCToolkit/VRCPackageManager")]
        public static void ShowWindow()
        {
            GetWindow<VRCPackageManagerWindow>("VRCPackageManager");
        }

        private static void DrawTitle()
        {
            DrawCenteredTitle("VRCPackageManager");
            GUILayout.BeginHorizontal();
            GUILayout.Space(40);
            GUILayout.Label("Welcome to the VRCPackageManager. Here you'll find a collection of useful tools, prefabs, the official SDKs for VRChat, and other packages in VRCToolkit", EditorStyles.wordWrappedLabel);
            GUILayout.Space(40);
            GUILayout.EndHorizontal();
        }

        private static bool CheckIfPlaying()
        {
            if (!EditorApplication.isPlaying) return false;
            DrawCenteredTitle("Cannot download packages in play mode");
            return true;
        }

        private static void DrawPageTitles()
        {
            selectedPage = GUILayout.Toolbar(selectedPage, VRCPackage.VRCPackageManager.GetPageTitles());
        }

        private static void DrawMainContent()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUIStyle.none, GUIStyle.none);
            DrawPage(selectedPage);
            GUILayout.EndScrollView();
        }

        private static void DrawFooter()
        {
            DrawCenteredTitle("Settings");
            var clearCachedData = DrawCenteredButton("Clear Cached Data");
            GUILayout.Space(20);

            if (clearCachedData) AssetDatabase.DeleteAsset("Assets/VRCToolkit/VRCPackageManager/Downloads");
        }

        private static void DrawCenteredTitle(string title)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(title, EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        private static bool DrawCenteredButton(string text)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(100);
            var pressed = GUILayout.Button(text, EditorStyles.miniButton);
            GUILayout.Space(100);
            GUILayout.EndHorizontal();
            return pressed;
        }
        
        private static void DrawSDK(string name, string url)
        {
            DrawCenteredTitle(name);
            var install = DrawCenteredButton("Install");
            GUILayout.Space(20);
            
            if (!install) return;
            var packageDownloader = new PackageDownloader(name, url, $"{name}.unitypackage");
            packageDownloader.ExecuteDownload();
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