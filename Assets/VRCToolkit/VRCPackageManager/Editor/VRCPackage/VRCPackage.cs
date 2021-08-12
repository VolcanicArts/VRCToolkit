using System;

namespace VRCToolkit.VRCPackageManager
{
    [Serializable]
    public class VRCPackage
    {
        [NonSerialized] private const string GitHubRepoBase = "https://github.com/";

        public int id;
        public string formattedName;
        public string repoName;
        public string fileNameFormat;
        public string description;
        public int[] requirements;

        public string GetRepoURL()
        {
            return GitHubRepoBase + repoName;
        }

        public string GetRequirements()
        {
            var requirementsStr = "";

            for (var i = 0; i < requirements.Length; i++)
            {
                var requirementID = requirements[i];
                requirementsStr += $"{VRCPackageManager.packages[requirementID].formattedName}";
                if (i + 1 != requirements.Length) requirementsStr += "/";
            }

            return requirementsStr;
        }
    }
}