using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using VRCToolkit.VRCPackageManager.Editor.GitHub;
using VRCToolkit.VRCPackageManager.Editor.VRCPackage;

namespace VRCToolkit.VRCPackageManager.Editor
{
    public class VRCPackageManagerWindow : EditorWindow
    {
        private static Vector2 scrollPosition;
        private int selectedPage;
        private List<string> pageTitles;

        private const string GitHubAPIBase = "https://api.github.com/repos/";
        private const string GitHubAPILatestRelease = "/releases/latest";
        private const string GitHubRepoLatestDownload = "/releases/latest/download/";

        private static VRCPackageData packageData;

        private void OnGUI()
        {
            if (packageData == null) LoadPackageData();

            AddCenteredTitle("VRCPackageManager");
            GUILayout.BeginHorizontal();
            GUILayout.Space(40);
            GUILayout.Label(
                "Welcome to the VRCPackageManager. Here you'll find a collection of useful tools, prefabs, the official SDKs for VRChat, and other packages in VRCToolkit",
                EditorStyles.wordWrappedLabel);
            GUILayout.Space(40);
            GUILayout.EndHorizontal();
            
            if (EditorApplication.isPlaying)
            {
                AddCenteredTitle("Cannot download packages in play mode");
                return;
            }

            selectedPage = GUILayout.Toolbar(selectedPage, pageTitles.ToArray());
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUIStyle.none, GUIStyle.none);

            switch (selectedPage)
            {
                case 0:
                    AddSDKInstallButton(nameof(SDKURLs.SDK2), SDKURLs.SDK2);
                    AddSDKInstallButton(nameof(SDKURLs.SDK3Avatar), SDKURLs.SDK3Avatar);
                    AddSDKInstallButton(nameof(SDKURLs.SDK3World), SDKURLs.SDK3World);
                    break;
                default:
                    AddSectionToPage(selectedPage - 1);
                    break;
            }
            
            GUILayout.EndScrollView();
        }

        private static void AddSectionToPage(int sectionID)
        {
            var section = packageData.pages[sectionID];
            foreach (var package in section.packages)
            {
                AddVRCPackage(package);   
            }
        }

        [MenuItem("VRCToolkit/VRCPackageManager")]
        public static void ShowWindow()
        {
            GetWindow<VRCPackageManagerWindow>("VRCPackageManager");
        }

        private static void AddCenteredTitle(string title)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(title, EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        private static void AddSDKInstallButton(string name, string url)
        {
            AddCenteredTitle(name);
            
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

        private static void AddVRCPackage(VRCPackage.VRCPackage package)
        {
            AddCenteredTitle(package.formattedName);
            GUILayout.Label(package.description, EditorStyles.wordWrappedLabel);
            if (!string.IsNullOrEmpty(package.requirements)) EditorGUILayout.HelpBox(new GUIContent($"This package requires {package.requirements}"));
            
            GUILayout.BeginHorizontal();
            GUILayout.Space(100);
            var install = GUILayout.Button("Install Package", EditorStyles.miniButton);
            var openURL = GUILayout.Button("Open Repository", EditorStyles.miniButton);
            GUILayout.Space(100);
            GUILayout.EndHorizontal();
            
            GUILayout.Space(20);

            if (install)
            {
                var latestReleaseFileName = GetLatestReleaseFileName(package.repoName, package.formattedName, package.fileNameFormat);
                if (latestReleaseFileName == null) return;
                var latestReleaseURL = package.GetRepoURL() + GitHubRepoLatestDownload + latestReleaseFileName;
                var packageDownloader = new PackageDownloader(package.formattedName, latestReleaseURL, latestReleaseFileName);
                packageDownloader.ExecuteDownload();
            }

            if (openURL)
            {
                Application.OpenURL(package.GetRepoURL());
            }
        }

        private static string GetLatestReleaseFileName(string repoName, string formattedName, string nameFormat)
        {
            var url = GitHubAPIBase + repoName + GitHubAPILatestRelease;
            Logger.Log($"Requesting for latest version of {formattedName} using URL: {url}");
            var uwr = new UnityWebRequest(url) {downloadHandler = new DownloadHandlerBuffer()};
            uwr.SendWebRequest();

            while (!uwr.isDone)
            {
                EditorUtility.DisplayProgressBar($"[VRCPackageManager] Getting latest version of {formattedName}", "", uwr.downloadProgress);
            }
            EditorUtility.ClearProgressBar();

            if (uwr.error != null)
            {
                Logger.LogError($"Could not get latest version of {formattedName}. Aborting download");
                return null;
            }

            var responseData = uwr.downloadHandler.text;
            var gitHubData = JsonUtility.FromJson<GitHubAPIResponse>(responseData);
            var fileName = string.Format(nameFormat, formattedName, gitHubData.tag_name);
            Logger.Log($"Found latest version of {gitHubData.tag_name}");
            return fileName;
        }

        private void LoadPackageData()
        {
            var packageDataLocation = $"{Application.dataPath}/VRCToolkit/VRCPackageManager/Editor/Resources/VRCPackages.json";
            var packageDataJson = File.ReadAllText(packageDataLocation);
            packageData = JsonUtility.FromJson<VRCPackageData>(packageDataJson);

            pageTitles = new List<string> {"VRC SDKs"};
            foreach (var page in packageData.pages)
            {
                pageTitles.Add(page.title);
            }
        }
    }
}