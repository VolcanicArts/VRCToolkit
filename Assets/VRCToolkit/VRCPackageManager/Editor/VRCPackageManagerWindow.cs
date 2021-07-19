using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;

namespace VRCToolkit.VRCPackageManager.Editor
{
    public class VRCPackageManagerWindow : EditorWindow
    {
        private const string LogPrefix = "[VRCToolkit/VRCPackageManager]";

        private const string VrcBase = "https://vrchat.com/download/";
        private const string SDK2 = "sdk2";
        private const string SDK3Avatar = "sdk3-avatars";
        private const string SDK3World = "sdk3-worlds";

        private const string GitHubAPIBase = "https://api.github.com/repos/";
        private const string GitHubAPILatestRelease = "/releases/latest";
        private const string GitHubRepoBase = "https://github.com/";
        private const string GitHubRepoLatestDownload = "/releases/latest/download/";

        private const string UdonSharp = "MerlinVR/UdonSharp";
        private const string UdonSharpNameFormat = "{0}_{1}.unitypackage";
        private const string UdonSharpDescription = "UdonSharp is a compiler that compiles C# to Udon assembly";
        private const string CyanEmu = "CyanLaser/CyanEmu";
        private const string CyanEmuNameFormat = "{0}.{1}.unitypackage";
        private const string CyanEmuDescription = "A VRChat client emulator in Unity for SDK2 and SDK3";
        private const string VRWorldToolkit = "oneVR/VRWorldToolkit";
        private const string VRWorldToolkitNameFormat = "{0}{1}.unitypackage";
        private const string VRWorldToolkitDescription =
            "VRWorld Toolkit is a Unity Editor extension made to make VRChat world creation more accessible and lower the entry-level to make a good performing world";
        private const string VrcPlayersOnlyMirrorSDK2 = "acertainbluecat/VRCPlayersOnlyMirror";
        private const string VrcPlayersOnlyMirrorSDK2NameFormat = "{0}_{1}.unitypackage";
        private const string VrcPlayersOnlyMirrorSDK3 = "acertainbluecat/VRCPlayersOnlyMirror";
        private const string VrcPlayersOnlyMirrorSDK3NameFormat = "{0}_{1}.unitypackage";
        private const string VrcPlayersOnlyMirrorDescription =
            "VRCPlayersOnlyMirror is a simple mirror prefab that shows players only without any background";
        private const string USharpVideo = "MerlinVR/USharpVideo";
        private const string USharpVideoNameFormat = "{0}_{1}.unitypackage";
        private const string USharpVideoDescription = "A basic video player made for VRChat using Udon and UdonSharp";

        private void OnGUI()
        {
            AddSectionTitle("VRC SDKs");
            AddSDKInstallButton(nameof(SDK2), SDK2);
            AddSDKInstallButton(nameof(SDK3Avatar), SDK3Avatar);
            AddSDKInstallButton(nameof(SDK3World), SDK3World);

            GUILayout.Space(40);
            
            AddSectionTitle("Tools");
            AddGitHubInstallButton(nameof(UdonSharp), UdonSharp, UdonSharpNameFormat, UdonSharpDescription, nameof(SDK3World));
            AddGitHubInstallButton(nameof(CyanEmu), CyanEmu, CyanEmuNameFormat, CyanEmuDescription, $"{nameof(SDK2)}/{nameof(SDK3World)}");
            AddGitHubInstallButton(nameof(VRWorldToolkit), VRWorldToolkit, VRWorldToolkitNameFormat, VRWorldToolkitDescription, nameof(SDK3World));

            GUILayout.Space(40);
            
            AddSectionTitle("Prefabs");
            AddGitHubInstallButton(nameof(VrcPlayersOnlyMirrorSDK2), VrcPlayersOnlyMirrorSDK2, VrcPlayersOnlyMirrorSDK2NameFormat, VrcPlayersOnlyMirrorDescription, nameof(SDK2));
            AddGitHubInstallButton(nameof(VrcPlayersOnlyMirrorSDK3), VrcPlayersOnlyMirrorSDK3, VrcPlayersOnlyMirrorSDK3NameFormat, VrcPlayersOnlyMirrorDescription, nameof(SDK3World));
            AddGitHubInstallButton(nameof(USharpVideo), USharpVideo, USharpVideoNameFormat, USharpVideoDescription, nameof(UdonSharp));
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

        private static void AddGitHubInstallButton(string packageName, string repoName, string format, string description, string requirements)
        {
            GUILayout.Label(packageName, EditorStyles.boldLabel);
            GUILayout.Label(description);
            EditorGUILayout.HelpBox(new GUIContent($"This package requires {requirements}"));
            if (!GUILayout.Button("Install", EditorStyles.miniButtonMid)) return;
            var latestReleaseFileName = GetLatestReleaseFileName(repoName, packageName, format);
            var latestReleaseURL = GitHubRepoBase + repoName + GitHubRepoLatestDownload + latestReleaseFileName;
            HandleDownload(packageName, latestReleaseURL, latestReleaseFileName);
        }

        private static string GetLatestReleaseFileName(string repo, string repoName, string nameFormat)
        {
            var url = GitHubAPIBase + repo + GitHubAPILatestRelease;
            Debug.Log($"{LogPrefix} Requesting for latest version of {repoName} using URL: {url}");
            var uwr = new UnityWebRequest(url) {downloadHandler = new DownloadHandlerBuffer()};
            uwr.SendWebRequest();

            while (!uwr.isDone)
            {
            } // wait for download

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
            } // wait for download

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