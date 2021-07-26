using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using VRCToolkit.VRCPackageManager.Editor.GitHub;
using VRCToolkit.VRCPackageManager.Editor.VRCPackage;

namespace VRCToolkit.VRCPackageManager.Editor
{
    public class VRCPackageManagerWindow : EditorWindow
    {
        private const string GitHubRepoLatestDownload = "/releases/latest/download/";
        
        private static Vector2 scrollPosition;
        private static int selectedPage;
        private static VRCPackageData packageData;
        private static List<string> pageTitles;
        
        [MenuItem("VRCToolkit/VRCPackageManager")]
        public static void ShowWindow()
        {
            GetWindow<VRCPackageManagerWindow>("VRCPackageManager");
        }

        private void OnGUI()
        {
            if (packageData == null) LoadData();
            DrawTitle();
            if (CheckIfPlaying()) return;
            DrawPageTitles();
            DrawMainContent();
        }

        private static void LoadData()
        {
            packageData = PackageManager.LoadDataFromFile();
            pageTitles = new List<string> {"VRC SDKs"};
            pageTitles.AddRange(packageData.pages.Select(page => page.title));
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
            selectedPage = GUILayout.Toolbar(selectedPage, pageTitles.ToArray());
        }

        private static void DrawMainContent()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUIStyle.none, GUIStyle.none);
            if (selectedPage == 0)
            {
                DrawSDK(nameof(SDKURLs.SDK2), SDKURLs.SDK2);
                DrawSDK(nameof(SDKURLs.SDK3Avatar), SDKURLs.SDK3Avatar);
                DrawSDK(nameof(SDKURLs.SDK3World), SDKURLs.SDK3World);
            }
            else
            {
                DrawPage(selectedPage - 1);
            }
            GUILayout.EndScrollView();
        }

        private static void DrawCenteredTitle(string title)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(title, EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        
        private static void DrawSDK(string name, string url)
        {
            DrawCenteredTitle(name);
            GUILayout.BeginHorizontal();
            GUILayout.Space(100);
            var install = GUILayout.Button("Install", EditorStyles.miniButton);
            GUILayout.Space(100);
            GUILayout.EndHorizontal();
            GUILayout.Space(20);
            
            if (!install) return;
            var packageDownloader = new PackageDownloader(name, url, $"{name}.unitypackage");
            packageDownloader.ExecuteDownload();
        }

        private static void DrawPage(int pageID)
        {
            var section = packageData.pages[pageID];
            foreach (var package in section.packages)
            {
                DrawVRCPackage(package);   
            }
        }
        
        private static void DrawVRCPackage(VRCPackage.VRCPackage package)
        {
            DrawCenteredTitle(package.formattedName);
            GUILayout.Label(package.description, EditorStyles.wordWrappedLabel);
            if (!string.IsNullOrEmpty(package.requirements)) EditorGUILayout.HelpBox(new GUIContent($"This package requires {package.requirements}"));
            
            GUILayout.BeginHorizontal();
            GUILayout.Space(100);
            var install = GUILayout.Button("Install Package", EditorStyles.miniButton);
            var openRepo = GUILayout.Button("Open Repository", EditorStyles.miniButton);
            GUILayout.Space(100);
            GUILayout.EndHorizontal();
            GUILayout.Space(20);
            
            if (install)
            {
                var latestReleaseFileName = GitHubUtil.GetLatestReleaseFileName(package.repoName, package.formattedName, package.fileNameFormat);
                if (latestReleaseFileName == null) return;
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