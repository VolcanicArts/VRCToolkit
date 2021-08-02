using VRCToolkit.VRCPackageManager.Editor.GitHub;

namespace VRCToolkit.VRCPackageManager.Editor
{
    public class VRCPackageInstallHandler
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
            var latestReleaseFileName = string.Format(package.fileNameFormat, package.formattedName, latestVersion);
            var latestReleaseURL = package.GetRepoURL() + GitHubRepoLatestDownload + latestReleaseFileName;
            var fileDownloader = new FileDownloader(package.formattedName, latestReleaseURL, latestReleaseFileName);
            var downloadedFilePath = fileDownloader.ExecuteDownload();
            if (string.IsNullOrEmpty(downloadedFilePath)) return;
            var packageImporter = new PackageImporter(package.formattedName, downloadedFilePath);
            packageImporter.ExecuteImport();
        }
    }
}