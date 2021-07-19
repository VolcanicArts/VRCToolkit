namespace VRCToolkit.VRCPackageManager.Editor
{
    public class VRCPackage
    {
        public readonly string FormattedName;
        public readonly string RepoName;
        public readonly string FileNameFormat;
        public readonly string Description;
        public readonly string Requirements;

        public VRCPackage(string formattedName, string repoName, string fileNameFormat, string description, string requirements)
        {
            FormattedName = formattedName;
            RepoName = repoName;
            FileNameFormat = fileNameFormat;
            Description = description;
            Requirements = requirements;
        }
    }
}