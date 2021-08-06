using System.Collections.Generic;
using VRCToolkit.VRCPackageManager.Editor.GitHub;
using VRCToolkit.VRCPackageManager.Editor.Settings;

namespace VRCToolkit.VRCPackageManager.Editor
{
    public static class VRCPackageInstallHandler
    {
        private const string GitHubRepoLatestDownload = "/releases/latest/download/";

        public static void InstallVRCPackage(int id)
        {
            var package = VRCPackage.VRCPackageManager.packages[id];
            foreach (var requirementID in package.requirements)
            {
                InstallVRCPackage(requirementID);
            }

            InstallPackage(package);
        }

        private static void InstallPackage(VRCPackage.VRCPackage package)
        {
            var latestVersion = GitHubUtil.GetLatestVersion(package.repoName, package.formattedName);
            if (latestVersion == null) return;
            try
            {
                var installedVersion = SettingsManager.installedVersions[package.id];
                if (installedVersion.Equals(latestVersion))
                {
                    Logger.Log($"Latest version of {package.formattedName} is already installed!");
                    return;
                }
            }
            catch (KeyNotFoundException)
            {
            }

            var latestReleaseFileName = string.Format(package.fileNameFormat, package.formattedName, latestVersion);
            var latestReleaseURL = package.GetRepoURL() + GitHubRepoLatestDownload + latestReleaseFileName;
            var downloadedFilePath = new FileDownloader(package.formattedName, latestReleaseURL, latestReleaseFileName).ExecuteDownload();
            if (string.IsNullOrEmpty(downloadedFilePath)) return;

            if (SettingsManager.installedVersions.TryGetValue(package.id, out _))
            {
                SettingsManager.installedVersions[package.id] = latestVersion;
            }
            else
            {
                SettingsManager.installedVersions.Add(package.id, latestVersion);
            }

            SettingsManager.SaveSettings();
            new PackageImporter(package.formattedName, downloadedFilePath).ExecuteImport();
        }
    }
}