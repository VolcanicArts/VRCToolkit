using System;
using System.Linq;

namespace VRCToolkit.VRCPackageManager
{
    [Serializable]
    public class Package
    {
        public string RepoURL => $"https://github.com/{repoName}";

        public string Requirements => requirements
            .Aggregate("", (current, requirementID) => current + $"{PackageManager.packages[requirementID].formattedName}").TrimEnd('/');

        public int id;
        public string formattedName;
        public string repoName;
        public string description;
        public int[] requirements;
    }
}