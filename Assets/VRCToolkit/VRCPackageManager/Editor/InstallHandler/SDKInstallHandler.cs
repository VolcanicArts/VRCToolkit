using UnityEditor;

namespace VRCToolkit.VRCPackageManager
{
    public static class SDKInstallHandler
    {
        public static void InstallSDK(string name, string url)
        {
            var downloadedFilePath = new FileDownloader(name, url).ExecuteDownload();
            if (string.IsNullOrEmpty(downloadedFilePath)) return;

            SettingsManager.settings.installedSDK = name;
            SettingsManager.SaveSettings();
            AssignEvents();
            new PackageImporter(name, downloadedFilePath).ExecuteImport();
            UnAssignEvents();
            SettingsManager.LoadSettings(true);
            SettingsManager.SetAttributes();
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

        private static void OnImportPackageCancelled(string packageName)
        {
            SettingsManager.GenerateDefaultSettings();
            SettingsManager.SaveSettings();
        }

        private static void OnImportPackageFailed(string packageName, string err)
        {
            SettingsManager.GenerateDefaultSettings();
            SettingsManager.SaveSettings();
        }
    }
}