using UnityEditor;

namespace VRCToolkit.VRCPackageManager
{
    public class PackageManagerWindow : EditorWindow
    {
        public static int selectedScreen;

        private static readonly VRCPackageManagerScreen[] _screens = { new SDKScreen(), new PackageScreen() };

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
                        PackageManager.LoadDataFromFile(true);
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
            GetWindow<PackageManagerWindow>("VRCPackageManager");
            if (SettingsManager.settings.updateSDKOnStart)
            {
                var installedSDK = SettingsManager.settings.installedSDK;
                if (!string.IsNullOrEmpty(installedSDK)) SDKInstallHandler.InstallSDK(installedSDK, SDKURLs.GetURL(installedSDK));
            }
        }
    }
}