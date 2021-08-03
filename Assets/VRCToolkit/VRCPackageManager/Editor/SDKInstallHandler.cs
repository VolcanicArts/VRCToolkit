using UnityEditor;
using VRCToolkit.VRCPackageManager.Editor.Settings;

namespace VRCToolkit.VRCPackageManager.Editor
{
    public class SDKInstallHandler
    {
        public static void InstallSDK(string name, string url)
        {
            var fileDownloader = new FileDownloader(name, url, $"{name}.unitypackage");
            var downloadedFilePath = fileDownloader.ExecuteDownload();

            if (string.IsNullOrEmpty(downloadedFilePath)) return;
            SettingsManager.settings.installedSDK = name;
            SettingsManager.SaveSettings();
            AssignEvents();
            var packageImporter = new PackageImporter(name, downloadedFilePath);
            packageImporter.ExecuteImport();
            UnAssignEvents();
            SettingsManager.LoadSettings(true);
            if (!string.IsNullOrEmpty(SettingsManager.settings.installedSDK)) VRCPackageManagerWindow.selectedScreen = 1;
        }

        private static void AssignEvents()
        {
            AssetDatabase.importPackageFailed += OnImportPackageFailed;
            AssetDatabase.importPackageCancelled += OnImportPackageCancelled;
        }

        private static void UnAssignEvents()
        {
            AssetDatabase.importPackageFailed -= OnImportPackageFailed;
            AssetDatabase.importPackageCancelled -= OnImportPackageCancelled;
        }

        private static void OnImportPackageCancelled(string packagename)
        {
            SettingsManager.settings.installedSDK = null;
            SettingsManager.SaveSettings();
        }

        private static void OnImportPackageFailed(string packagename, string errormessage)
        {
            SettingsManager.settings.installedSDK = null;
            SettingsManager.SaveSettings();
        }
    }
}