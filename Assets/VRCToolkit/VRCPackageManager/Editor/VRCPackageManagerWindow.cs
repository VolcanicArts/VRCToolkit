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

        private static readonly VRCPackage UdonSharp = new VRCPackage(
            nameof(UdonSharp),
            "MerlinVR/UdonSharp",
            "{0}_{1}.unitypackage",
            "UdonSharp is a compiler that compiles C# to Udon assembly",
            nameof(SDK3World));
        
        private static readonly VRCPackage CyanEmu = new VRCPackage(
            nameof(CyanEmu),
            "CyanLaser/CyanEmu",
            "{0}.{1}.unitypackage",
            "A VRChat client emulator in Unity for SDK2 and SDK3",
            $"{nameof(SDK2)}/{nameof(SDK3World)}");
        
        private static readonly VRCPackage VRWorldToolkit = new VRCPackage(
            nameof(VRWorldToolkit),
            "oneVR/VRWorldToolkit",
            "{0}{1}.unitypackage",
            "VRWorld Toolkit is a Unity Editor extension made to make VRChat world creation more accessible and lower the entry-level to make a good performing world",
            nameof(SDK3World));

        private const string VRCPlayersOnlyMirrorDescription = "VRCPlayersOnlyMirror is a simple mirror prefab that shows players only without any background";
        
        private static readonly VRCPackage VRCPlayersOnlyMirrorSDK2 = new VRCPackage(
            nameof(VRCPlayersOnlyMirrorSDK2),
            "acertainbluecat/VRCPlayersOnlyMirror",
            "{0}_{1}.unitypackage",
            VRCPlayersOnlyMirrorDescription,
            nameof(SDK2));
        
        private static readonly VRCPackage VRCPlayersOnlyMirrorSDK3 = new VRCPackage(
            nameof(VRCPlayersOnlyMirrorSDK3),
            "acertainbluecat/VRCPlayersOnlyMirror",
            "{0}_{1}.unitypackage",
            VRCPlayersOnlyMirrorDescription,
            nameof(SDK3World));
        
        private static readonly VRCPackage USharpVideo = new VRCPackage(
            "USharpVideo",
            "MerlinVR/USharpVideo",
            "{0}_{1}.unitypackage",
            "A basic video player made for VRChat using Udon and UdonSharp",
            UdonSharp.FormattedName);

        private void OnGUI()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUIStyle.none, GUIStyle.none);
            
            AddSectionTitle("VRC SDKs");
            AddSDKInstallButton(nameof(SDK2), SDK2);
            AddSDKInstallButton(nameof(SDK3Avatar), SDK3Avatar);
            AddSDKInstallButton(nameof(SDK3World), SDK3World);

            GUILayout.Space(40);
            
            AddSectionTitle("Tools");
            AddVRCPackage(UdonSharp);
            AddVRCPackage(CyanEmu);
            AddVRCPackage(VRWorldToolkit);

            GUILayout.Space(40);
            
            AddSectionTitle("Prefabs");
            AddVRCPackage(VRCPlayersOnlyMirrorSDK2);
            AddVRCPackage(VRCPlayersOnlyMirrorSDK3);
            AddVRCPackage(USharpVideo);
            
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
            GUILayout.Label(package.FormattedName, EditorStyles.boldLabel);
            GUILayout.Label(package.Description, EditorStyles.wordWrappedLabel);
            EditorGUILayout.HelpBox(new GUIContent($"This package requires {package.Requirements}"));
            if (!GUILayout.Button("Install", EditorStyles.miniButtonMid)) return;
            var latestReleaseFileName = GetLatestReleaseFileName(package.RepoName, package.FormattedName, package.FileNameFormat);
            if (latestReleaseFileName == null) return;
            var latestReleaseURL = GitHubRepoBase + package.RepoName + GitHubRepoLatestDownload + latestReleaseFileName;
            HandleDownload(package.FormattedName, latestReleaseURL, latestReleaseFileName);
        }

        private static string GetLatestReleaseFileName(string repo, string repoName, string nameFormat)
        {
            var url = GitHubAPIBase + repo + GitHubAPILatestRelease;
            Debug.Log($"{LogPrefix} Requesting for latest version of {repoName} using URL: {url}");
            var uwr = new UnityWebRequest(url) {downloadHandler = new DownloadHandlerBuffer()};
            uwr.SendWebRequest();

            while (!uwr.isDone)
            {
                EditorUtility.DisplayProgressBar($"[VRCPackageManager] Getting latest version of {repoName}", "", uwr.downloadProgress);
            }
            EditorUtility.ClearProgressBar();

            if (uwr.error != null)
            {
                Debug.LogError($"Could not get latest version of {repoName}. Aborting download");
                return null;
            }

            var responseData = uwr.downloadHandler.text;
            var gitHubData = JsonUtility.FromJson<GitHubAPIResponse>(responseData);
            var fileName = string.Format(nameFormat, repoName, gitHubData.tag_name);
            Debug.Log($"{LogPrefix} Found latest version of {gitHubData.tag_name}");
            return fileName;
        }

        private static void HandleDownload(string packageName, string url, string fileName)
        {
            Debug.Log($"{LogPrefix} Attempting to download {packageName} using URL {url}");
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
                Debug.Log($"{LogPrefix} {packageName} successfully downloaded. Importing unitypackage!");
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                Process.Start(path);
            }
            else
            {
                Debug.LogError(uwr.error);
            }
        }
    }
}