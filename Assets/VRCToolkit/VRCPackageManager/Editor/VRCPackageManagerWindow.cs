using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using VRCToolkit.VRCPackageManager.Editor.GitHub;

namespace VRCToolkit.VRCPackageManager.Editor
{
    public class VRCPackageManagerWindow : EditorWindow
    {
        private static Vector2 scrollPosition;
        private static bool[] foldouts;

        private const string VrcBase = "https://vrchat.com/download/";
        private const string SDK2 = "sdk2";
        private const string SDK3Avatar = "sdk3-avatars";
        private const string SDK3World = "sdk3-worlds";

        private const string GitHubAPIBase = "https://api.github.com/repos/";
        private const string GitHubAPILatestRelease = "/releases/latest";
        private const string GitHubRepoBase = "https://github.com/";
        private const string GitHubRepoLatestDownload = "/releases/latest/download/";

        private void OnGUI()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUIStyle.none, GUIStyle.none);

            AddCenteredTitle("VRCPackageManager");
            GUILayout.BeginHorizontal();
            GUILayout.Space(40);
            GUILayout.Label(
                "Welcome to the VRCPackageManager. Here you'll find a collection of useful addons, prefabs, and the official SDKs for VRChat",
                EditorStyles.wordWrappedLabel);
            GUILayout.Space(40);
            GUILayout.EndHorizontal();
            GUILayout.Space(20);
            
            foldouts[0] = EditorGUILayout.Foldout(foldouts[0], "VRC SDKs");
            if (foldouts[0])
            {
                AddSDKInstallButton(nameof(SDK2), SDK2);
                AddSDKInstallButton(nameof(SDK3Avatar), SDK3Avatar);
                AddSDKInstallButton(nameof(SDK3World), SDK3World);
            }

            for (var i = 0; i < Startup.packageData.sections.Length; i++)
            {
                var section = Startup.packageData.sections[i];
                foldouts[i+1] = EditorGUILayout.Foldout(foldouts[i+1], section.title);
                if (!foldouts[i+1]) continue;
                foreach (var package in section.packages)
                {
                    AddVRCPackage(package);
                }
            }

            GUILayout.EndScrollView();
        }

        [MenuItem("VRCToolkit/VRCPackageManager")]
        public static void ShowWindow()
        {
            foldouts = new bool[Startup.packageData.sections.Length + 1];
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

        private static void AddSDKInstallButton(string name, string endpoint)
        {
            AddCenteredTitle(name);
            
            GUILayout.BeginHorizontal();
            GUILayout.Space(100);
            var clicked = GUILayout.Button("Install", EditorStyles.miniButton);
            GUILayout.Space(100);
            GUILayout.EndHorizontal();
            
            GUILayout.Space(20);

            if (!clicked) return;
            var latestReleaseURL = $"{VrcBase}{endpoint}";
            var packageDownloader = new PackageDownloader(name, latestReleaseURL, $"{name}.unitypackage");
            packageDownloader.ExecuteDownload();
        }

        private static void AddVRCPackage(VRCPackage.VRCPackage package)
        {
            AddCenteredTitle(package.formattedName);
            GUILayout.Label(package.description, EditorStyles.wordWrappedLabel);
            EditorGUILayout.HelpBox(new GUIContent($"This package requires {package.requirements}"));
            
            GUILayout.BeginHorizontal();
            GUILayout.Space(100);
            var clicked = GUILayout.Button("Install", EditorStyles.miniButton);
            GUILayout.Space(100);
            GUILayout.EndHorizontal();
            
            GUILayout.Space(20);

            if (!clicked) return;
            var latestReleaseFileName = GetLatestReleaseFileName(package.repoName, package.formattedName, package.fileNameFormat);
            if (latestReleaseFileName == null) return;
            var latestReleaseURL = GitHubRepoBase + package.repoName + GitHubRepoLatestDownload + latestReleaseFileName;
            var packageDownloader = new PackageDownloader(package.formattedName, latestReleaseURL, latestReleaseFileName);
            packageDownloader.ExecuteDownload();
        }

        private static string GetLatestReleaseFileName(string repo, string repoName, string nameFormat)
        {
            var url = GitHubAPIBase + repo + GitHubAPILatestRelease;
            Logger.Log($"Requesting for latest version of {repoName} using URL: {url}");
            var uwr = new UnityWebRequest(url) {downloadHandler = new DownloadHandlerBuffer()};
            uwr.SendWebRequest();

            while (!uwr.isDone)
            {
                EditorUtility.DisplayProgressBar($"[VRCPackageManager] Getting latest version of {repoName}", "", uwr.downloadProgress);
            }
            EditorUtility.ClearProgressBar();

            if (uwr.error != null)
            {
                Logger.LogError($"Could not get latest version of {repoName}. Aborting download");
                return null;
            }

            var responseData = uwr.downloadHandler.text;
            var gitHubData = JsonUtility.FromJson<GitHubAPIResponse>(responseData);
            var fileName = string.Format(nameFormat, repoName, gitHubData.tag_name);
            Logger.Log($"Found latest version of {gitHubData.tag_name}");
            return fileName;
        }
    }
}