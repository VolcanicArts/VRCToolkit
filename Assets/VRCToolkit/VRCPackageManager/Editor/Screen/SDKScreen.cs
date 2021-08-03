﻿using UnityEditor;
using UnityEngine;
using VRCToolkit.VRCPackageManager.Editor.Settings;

namespace VRCToolkit.VRCPackageManager.Editor.Screen
{
    public class SDKScreen : VRCPackageManagerScreen
    {

        public override void OnGUI()
        {
            base.OnGUI();
            if (CheckIfPlaying()) return;
            
            DrawCenteredTitle("Choose an SDK");
            DrawCenteredText("This will install the latest version of the SDK, and store the SDK type");
            DrawCenteredText("If you have an SDK installed, please choose the SDK you're using");
            DrawSDK(nameof(SDKURLs.SDK2), SDKURLs.SDK2);
            DrawSDK(nameof(SDKURLs.SDK3World), SDKURLs.SDK3World);
            DrawSDK(nameof(SDKURLs.SDK3Avatar), SDKURLs.SDK3Avatar);
        }
        
        private static void DrawSDK(string name, string url)
        {
            DrawCenteredTitle(name);
            var install = DrawCenteredButton("Choose SDK");
            GUILayout.Space(20);
            
            if (!install) return;
            SDKInstallHandler.InstallSDK(name, url);
        }
    }
}