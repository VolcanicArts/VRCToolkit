using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;

public class VRCPackageManagerWindow : EditorWindow
{
    private const string LogPrefix = "[VRCToolkit/VRCPackageManager]";

    private const string VrcBase = "https://vrchat.com/download/";
    private const string SDK2 = "sdk2";
    private const string SDK3Avatar = "sdk3-avatars";
    private const string SDK3World = "sdk3-worlds";

    private const string UdonSharp =
        "https://github.com/MerlinVR/UdonSharp/releases/latest/download/UdonSharp_v0.20.0.unitypackage";

    private const string CyanEmu =
        "https://github.com/CyanLaser/CyanEmu/releases/latest/download/CyanEmu.v0.3.8.unitypackage";

    private const string VRWorldToolkit =
        "https://github.com/oneVR/VRWorldToolkit/releases/latest/download/VRWorldToolkitV1.11.2.unitypackage";

    private const string VrcPlayersOnlyMirrorSDK2 =
        "https://github.com/acertainbluecat/VRCPlayersOnlyMirror/releases/latest/download/VRCPlayersOnlyMirrorSDK2_v0.1.3.unitypackage";

    private const string VrcPlayersOnlyMirrorSDK3 =
        "https://github.com/acertainbluecat/VRCPlayersOnlyMirror/releases/latest/download/VRCPlayersOnlyMirrorSDK3_v0.1.3.unitypackage";

    private const string USharpVideo =
        "https://github.com/MerlinVR/USharpVideo/releases/latest/download/USharpVideo_v1.0.0.unitypackage";

    [MenuItem("VRCToolkit/VRCPackageManager")]
    public static void ShowWindow()
    {
        GetWindow<VRCPackageManagerWindow>("VRCPackageManager");
    }

    private void OnGUI()
    {
        // SDKs
        using (new GUILayout.HorizontalScope())
        {
            GUILayout.FlexibleSpace();
            GUILayout.Label("VRC SDKs", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
        }

        GUILayout.Label("SDK2", EditorStyles.boldLabel);
        if (GUILayout.Button("Install", EditorStyles.miniButtonMid))
        {
            HandleDownload("SDK2", $"{VrcBase}{SDK2}", "unitypackage");
        }

        // SDK3
        GUILayout.Label("SDK3 Avatar", EditorStyles.boldLabel);
        if (GUILayout.Button("Install", EditorStyles.miniButtonMid))
        {
            HandleDownload("SDK3Avatar", $"{VrcBase}{SDK3Avatar}", "unitypackage");
        }

        GUILayout.Label("SDK3 World", EditorStyles.boldLabel);
        if (GUILayout.Button("Install", EditorStyles.miniButtonMid))
        {
            HandleDownload("SDK3World", $"{VrcBase}{SDK3World}", "unitypackage");
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
        if (GUILayout.Button("Install", EditorStyles.miniButtonMid))
        {
            HandleDownload("UdonSharp", UdonSharp, "unitypackage");
        }

        GUILayout.Label("CyanEmu", EditorStyles.boldLabel);
        GUILayout.Label("A VRChat client emulator in Unity for SDK2 and SDK3");
        if (GUILayout.Button("Install", EditorStyles.miniButtonMid))
        {
            HandleDownload("CyanEmu", CyanEmu, "unitypackage");
        }

        GUILayout.Label("VRWorld Toolkit", EditorStyles.boldLabel);
        GUILayout.Label(
            "VRWorld Toolkit is a Unity Editor extension made to make VRChat world creation more\naccessible and lower the entry-level to make a good performing world");
        if (GUILayout.Button("Install", EditorStyles.miniButtonMid))
        {
            HandleDownload("VRWorldToolkit", VRWorldToolkit, "unitypackage");
        }

        GUILayout.Space(40);

        // Addons
        using (new GUILayout.HorizontalScope())
        {
            GUILayout.FlexibleSpace();
            GUILayout.Label("Addons", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
        }

        GUILayout.Label("VRCPlayersOnlyMirror", EditorStyles.boldLabel);
        GUILayout.Label(
            "VRCPlayersOnlyMirror is a simple mirror prefab that shows players\nonly without any background");
        if (GUILayout.Button("Install SDK2", EditorStyles.miniButtonMid))
        {
            HandleDownload("VRCPlayersOnlyMirrorSDK2", VrcPlayersOnlyMirrorSDK2, "unitypackage");
        }

        if (GUILayout.Button("Install SDK3", EditorStyles.miniButtonMid))
        {
            HandleDownload("VRCPlayersOnlyMirrorSDK3", VrcPlayersOnlyMirrorSDK3, "unitypackage");
        }

        GUILayout.Label("USharpVideo", EditorStyles.boldLabel);
        GUILayout.Label("A basic video player made for VRChat using Udon and UdonSharp");
        if (GUILayout.Button("Install", EditorStyles.miniButtonMid))
        {
            HandleDownload("USharpVideo", USharpVideo, "unitypackage");
        }
    }

    private void HandleDownload(string fileName, string url, string extension)
    {
        Debug.Log($"{LogPrefix} Downloading ${fileName}");
        var uwr = new UnityWebRequest(url);
        var path = $"{Application.dataPath}/VRCToolkit/VRCPackageManager/Downloads/{fileName}.{extension}";
        uwr.downloadHandler = new DownloadHandlerFile(path);
        uwr.SendWebRequest();

        while (!uwr.isDone)
        {
        } // wait for download

        if (uwr.error == null)
        {
            Debug.Log($"{LogPrefix} Package successfully downloaded. Attempting to import package...");
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            Debug.Log($"{LogPrefix} Importing package!");
            System.Diagnostics.Process.Start(path);
        }
        else
        {
            Debug.LogError(uwr.error);
        }
    }
}