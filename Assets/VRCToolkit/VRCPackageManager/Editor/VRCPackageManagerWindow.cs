using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using VRCToolkit.VRCPackageManager.Editor;
using Debug = UnityEngine.Debug;

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
    private const string CyanEmu = "CyanLaser/CyanEmu";
    private const string CyanEmuNameFormat = "{0}.{1}.unitypackage";
    private const string VRWorldToolkit = "oneVR/VRWorldToolkit";
    private const string VRWorldToolkitNameFormat = "{0}{1}.unitypackage";
    private const string VrcPlayersOnlyMirrorSDK2 = "acertainbluecat/VRCPlayersOnlyMirror";
    private const string VrcPlayersOnlyMirrorSDK2NameFormat = "{0}_{1}.unitypackage";
    private const string VrcPlayersOnlyMirrorSDK3 = "acertainbluecat/VRCPlayersOnlyMirror";
    private const string VrcPlayersOnlyMirrorSDK3NameFormat = "{0}_{1}.unitypackage";
    private const string USharpVideo = "MerlinVR/USharpVideo";
    private const string USharpVideoNameFormat = "{0}_{1}.unitypackage";

    private void OnGUI()
    {
        // SDKs
        using (new GUILayout.HorizontalScope())
        {
            GUILayout.FlexibleSpace();
            GUILayout.Label("VRC SDKs", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
        }

        // SDK2
        GUILayout.Label("SDK2", EditorStyles.boldLabel);
        if (GUILayout.Button("Install", EditorStyles.miniButtonMid))
        {
            var latestReleaseURL = $"{VrcBase}{SDK2}";
            HandleDownload(nameof(SDK2), latestReleaseURL, "unitypackage");
        }

        // SDK3
        GUILayout.Label("SDK3 Avatar", EditorStyles.boldLabel);
        if (GUILayout.Button("Install", EditorStyles.miniButtonMid))
        {
            var latestReleaseURL = $"{VrcBase}{SDK3Avatar}";
            HandleDownload(nameof(SDK3Avatar), latestReleaseURL, "unitypackage");
        }

        GUILayout.Label("SDK3 World", EditorStyles.boldLabel);
        if (GUILayout.Button("Install", EditorStyles.miniButtonMid))
        {
            var latestReleaseURL = $"{VrcBase}{SDK3World}";
            HandleDownload(nameof(SDK3World), latestReleaseURL, "unitypackage");
        }

        GUILayout.Space(40);

        // Tools
        using (new GUILayout.HorizontalScope())
        {
            GUILayout.FlexibleSpace();
            GUILayout.Label("Tools", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
        }

        GUILayout.Label("UdonSharp", EditorStyles.boldLabel);
        GUILayout.Label("UdonSharp is a compiler that compiles C# to Udon assembly");
        AddInstallButton(nameof(UdonSharp), UdonSharp, UdonSharpNameFormat, "unitypackage");

        GUILayout.Label("CyanEmu", EditorStyles.boldLabel);
        GUILayout.Label("A VRChat client emulator in Unity for SDK2 and SDK3");
        AddInstallButton(nameof(CyanEmu), CyanEmu, CyanEmuNameFormat, "unitypackage");

        GUILayout.Label("VRWorld Toolkit", EditorStyles.boldLabel);
        GUILayout.Label("VRWorld Toolkit is a Unity Editor extension made to make VRChat world creation more" +
                        "\naccessible and lower the entry-level to make a good performing world");
        AddInstallButton(nameof(VRWorldToolkit), VRWorldToolkit, VRWorldToolkitNameFormat, "unitypackage");

        GUILayout.Space(40);

        // Prefabs
        using (new GUILayout.HorizontalScope())
        {
            GUILayout.FlexibleSpace();
            GUILayout.Label("Prefabs", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
        }

        GUILayout.Label("VRCPlayersOnlyMirror SDK2", EditorStyles.boldLabel);
        GUILayout.Label("VRCPlayersOnlyMirror is a simple mirror prefab that shows players" +
                        "\nonly without any background");
        AddInstallButton(nameof(VrcPlayersOnlyMirrorSDK2), VrcPlayersOnlyMirrorSDK2, VrcPlayersOnlyMirrorSDK2NameFormat,
            "unitypackage");

        GUILayout.Label("VRCPlayersOnlyMirror SDK3", EditorStyles.boldLabel);
        GUILayout.Label("VRCPlayersOnlyMirror is a simple mirror prefab that shows players" +
                        "\nonly without any background");
        AddInstallButton(nameof(VrcPlayersOnlyMirrorSDK3), VrcPlayersOnlyMirrorSDK3, VrcPlayersOnlyMirrorSDK3NameFormat,
            "unitypackage");

        GUILayout.Label("USharpVideo", EditorStyles.boldLabel);
        GUILayout.Label("A basic video player made for VRChat using Udon and UdonSharp");
        AddInstallButton(nameof(USharpVideo), USharpVideo, USharpVideoNameFormat, "unitypackage");
    }

    [MenuItem("VRCToolkit/VRCPackageManager")]
    public static void ShowWindow()
    {
        GetWindow<VRCPackageManagerWindow>("VRCPackageManager");
    }

    private void AddInstallButton(string packageName, string repoName, string format, string extension)
    {
        if (!GUILayout.Button("Install", EditorStyles.miniButtonMid)) return;
        var latestReleaseURL = GetLatestReleaseURL(repoName, packageName, format);
        HandleDownload(packageName, latestReleaseURL, extension);
    }

    private string GetLatestReleaseURL(string repo, string repoName, string nameFormat)
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
        var downloadURL = GitHubRepoBase + repo + GitHubRepoLatestDownload + fileName;
        Debug.Log($"{LogPrefix} Found latest version of {gitHubData.tag_name}");
        return downloadURL;
    }

    private void HandleDownload(string fileName, string url, string extension)
    {
        Debug.Log($"{LogPrefix} Attempting to download {fileName} using URL {url}");
        var uwr = new UnityWebRequest(url);
        var path = $"{Application.dataPath}/VRCToolkit/VRCPackageManager/Downloads/{fileName}.{extension}";
        uwr.downloadHandler = new DownloadHandlerFile(path);
        uwr.SendWebRequest();

        while (!uwr.isDone)
        {
        } // wait for download

        if (uwr.error == null)
        {
            Debug.Log($"{LogPrefix} Package successfully downloaded. Importing package!");
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            Process.Start(path);
        }
        else
        {
            Debug.LogError(uwr.error);
        }
    }
}