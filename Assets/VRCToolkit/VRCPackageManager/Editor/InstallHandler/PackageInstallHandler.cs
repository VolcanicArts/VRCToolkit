using System.Collections.Generic;

namespace VRCToolkit.VRCPackageManager
{
    public static class VRCPackageInstallHandler
    {
        private const string GitHubRepoLatestDownload = "/releases/latest/download/";

        public static void InstallVRCPackage(int id)
        {
            var package = PackageManager.packages[id];
            foreach (var requirementID in package.requirements) InstallVRCPackage(requirementID);

            InstallPackage(package);
        }

        private static void InstallPackage(Package package)
        {
            var latestVersion = GitHubUtil.GetLatestVersion(package.repoName, package.formattedName);
            if (latestVersion == null) return;
            try
            {
                var installedVersion = SettingsManager.installedVersions[package.id];
                if (installedVersion.Equals(latestVersion.Version))
                {
                    Logger.Log($"Latest version of {package.formattedName} is already installed!");
                    return;
                }
            }
            catch (KeyNotFoundException)
            {
            }

            var downloadedFilePath = new FileDownloader(package.formattedName, latestVersion.DownloadURL).ExecuteDownload();
            if (string.IsNullOrEmpty(downloadedFilePath)) return;

            if (SettingsManager.installedVersions.TryGetValue(package.id, out _))
                SettingsManager.installedVersions[package.id] = latestVersion.Version;
            else
                SettingsManager.installedVersions.Add(package.id, latestVersion.Version);

            SettingsManager.SaveSettings();
            new PackageImporter(package.formattedName, downloadedFilePath).ExecuteImport();
        }
    }
}