using UnityEditor;

namespace VRCToolkit.VRCPackageManager
{
    public class PackageImporter
    {
        private readonly string filePath;
        private readonly string formattedName;

        public PackageImporter(string formattedName, string filePath)
        {
            this.formattedName = formattedName;
            this.filePath = filePath;
        }

        public void ExecuteImport()
        {
            AssignEvents();
            AssetDatabase.ImportPackage(filePath, false);
        }

        private void FinishedImport()
        {
            UnAssignEvents();
            FileUtil.DeleteFileOrDirectory(filePath);
            FileUtil.DeleteFileOrDirectory($"{filePath}.meta");
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }

        private void AssignEvents()
        {
            AssetDatabase.importPackageStarted += OnImportPackageStarted;
            AssetDatabase.importPackageCompleted += OnImportPackageCompleted;
            AssetDatabase.importPackageFailed += OnImportPackageFailed;
        }

        private void UnAssignEvents()
        {
            AssetDatabase.importPackageStarted -= OnImportPackageStarted;
            AssetDatabase.importPackageCompleted -= OnImportPackageCompleted;
            AssetDatabase.importPackageFailed -= OnImportPackageFailed;
        }

        private void OnImportPackageStarted(string ignored)
        {
            Logger.Log($"Attempting to import {formattedName}");
        }

        private void OnImportPackageCompleted(string ignored)
        {
            Logger.Log($"{formattedName} has been successfully imported!");
            FinishedImport();
        }

        private void OnImportPackageFailed(string ignored, string errorMessage)
        {
            Logger.LogError($"Failed to import {formattedName}: {errorMessage}");
            FinishedImport();
        }
    }
}