using System;

namespace VRCToolkit.VRCPackageManager.Editor
{
    [Serializable]
    public class VRCPackage
    {
        public string formattedName;
        public string repoName;
        public string fileNameFormat;
        public string description;
        public string requirements;
    }
}