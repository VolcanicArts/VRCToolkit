using UnityEditor;
using UnityEngine;
using VRCToolkit.VRCPackageManager.Editor.Screen;
using VRCToolkit.VRCPackageManager.Editor.Settings;

namespace VRCToolkit.VRCPackageManager.Editor
{
    public class VRCPackageManagerWindow : EditorWindow
    {
        public static int selectedScreen;

        private static readonly VRCPackageManagerScreen[] _screens = {new SDKScreen(), new PackageScreen()};

        private void OnGUI()
        {
            if (selectedScreen == 0)
            {
                var loadedSettings = SettingsManager.LoadSettings(false);
                if (loadedSettings)
                {
                    Logger.Log($"Installed SDK is {SettingsManager.settings.installedSDK}");
                    if (!string.IsNullOrEmpty(SettingsManager.settings.installedSDK))
                    {
                        selectedScreen = 1;
                        VRCPackage.VRCPackageManager.LoadDataFromFile(true);
                    }
                }
            }

            _screens[selectedScreen].OnGUI();
        }

        [MenuItem("VRCToolkit/VRCPackageManager")]
        public static void ShowWindow()
        {
            SettingsManager.LoadSettings(true);
            SettingsManager.SetAttributes();
            GetWindow<VRCPackageManagerWindow>("VRCPackageManager");
            if (SettingsManager.settings.updateSDKOnStart)
            {
                var installedSDK = SettingsManager.settings.installedSDK;
                if (!string.IsNullOrEmpty(installedSDK)) SDKInstallHandler.InstallSDK(installedSDK, SDKURLs.GetURL(installedSDK));
            }
        }
    }
}