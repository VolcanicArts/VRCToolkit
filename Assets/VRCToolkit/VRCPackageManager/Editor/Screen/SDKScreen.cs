using UnityEngine;

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
            var packageDownloader = new PackageDownloader(name, url, $"{name}.unitypackage");
            packageDownloader.ExecuteDownload();
            VRCPackageManagerWindow.installedSDK = name;
            VRCPackageManagerWindow.selectedScreen = 1;
            VRCPackage.VRCPackageManager.LoadDataFromFile(VRCPackageManagerWindow.installedSDK, true);
        }
    }
}