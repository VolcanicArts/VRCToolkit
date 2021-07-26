using System;

namespace VRCToolkit.VRCPackageManager.Editor.VRCPackage
{
    [Serializable]
    public class VRCPackage
    {
        private readonly string GitHubRepoBase = "https://github.com/";
        
        public string formattedName;
        public string repoName;
        public string fileNameFormat;
        public string description;
        public string requirements;

        public string GetRepoURL()
        {
            return GitHubRepoBase + repoName;
        }
    }
}