using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;

namespace VRCToolkit.VRCPackageManager.Editor
{
    public class VRCPackageManagerWindow : EditorWindow
    {
        private Vector2 scrollPosition;
        
        private const string LogPrefix = "[VRCToolkit/VRCPackageManager]";

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
            
            AddSectionTitle("VRC SDKs");
            AddSDKInstallButton(nameof(SDK2), SDK2);
            AddSDKInstallButton(nameof(SDK3Avatar), SDK3Avatar);
            AddSDKInstallButton(nameof(SDK3World), SDK3World);

            foreach (var section in Startup.packageData.sections)
            {
                GUILayout.Space(40);
                AddSectionTitle(section.title);
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
            GetWindow<VRCPackageManagerWindow>("VRCPackageManager");
        }

        private static void AddSectionTitle(string title)
        {
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label(title, EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
            }
        }

        private static void AddSDKInstallButton(string name, string endpoint)
        {
            GUILayout.Label(name, EditorStyles.boldLabel);
            if (!GUILayout.Button("Install", EditorStyles.miniButtonMid)) return;
            var latestReleaseURL = $"{VrcBase}{endpoint}";
            HandleDownload(name, latestReleaseURL, $"{name}.unitypackage");
        }

        private static void AddVRCPackage(VRCPackage package)
        {
            GUILayout.Label(package.formattedName, EditorStyles.boldLabel);
            GUILayout.Label(package.description, EditorStyles.wordWrappedLabel);
            EditorGUILayout.HelpBox(new GUIContent($"This package requires {package.requirements}"));
            if (!GUILayout.Button("Install", EditorStyles.miniButtonMid)) return;
            var latestReleaseFileName = GetLatestReleaseFileName(package.repoName, package.formattedName, package.fileNameFormat);
            if (latestReleaseFileName == null) return;
            var latestReleaseURL = GitHubRepoBase + package.repoName + GitHubRepoLatestDownload + latestReleaseFileName;
            HandleDownload(package.formattedName, latestReleaseURL, latestReleaseFileName);
        }

        private static string GetLatestReleaseFileName(string repo, string repoName, string nameFormat)
        {
            var url = GitHubAPIBase + repo + GitHubAPILatestRelease;
            Log($"Requesting for latest version of {repoName} using URL: {url}");
            var uwr = new UnityWebRequest(url) {downloadHandler = new DownloadHandlerBuffer()};
            uwr.SendWebRequest();

            while (!uwr.isDone)
            {
                EditorUtility.DisplayProgressBar($"[VRCPackageManager] Getting latest version of {repoName}", "", uwr.downloadProgress);
            }
            EditorUtility.ClearProgressBar();

            if (uwr.error != null)
            {
                LogError($"Could not get latest version of {repoName}. Aborting download");
                return null;
            }

            var responseData = uwr.downloadHandler.text;
            var gitHubData = JsonUtility.FromJson<GitHubAPIResponse>(responseData);
            var fileName = string.Format(nameFormat, repoName, gitHubData.tag_name);
            Log($"Found latest version of {gitHubData.tag_name}");
            return fileName;
        }

        private static void HandleDownload(string packageName, string url, string fileName)
        {
            Log($"Attempting to download {packageName} using URL {url}");
            var uwr = new UnityWebRequest(url);
            var path = $"{Application.dataPath}/VRCToolkit/VRCPackageManager/Downloads/{fileName}";
            uwr.downloadHandler = new DownloadHandlerFile(path);
            uwr.SendWebRequest();

            while (!uwr.isDone)
            {
                EditorUtility.DisplayProgressBar($"[VRCPackageManager] Downloading {packageName}", "", uwr.downloadProgress);
            }
            EditorUtility.ClearProgressBar();

            if (uwr.error == null)
            {
                Log($"{packageName} successfully downloaded. Importing unitypackage!");
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                Process.Start(path);
            }
            else
            {
                Debug.LogError(uwr.error);
            }
        }

        private static void Log(string message)
        {
            Debug.Log($"{LogPrefix} {message}");
        }

        private static void LogError(string message)
        {
            Debug.LogError($"{LogPrefix} {message}");
        }
    }
}