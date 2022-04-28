using System;

namespace VRCToolkit.VRCPackageManager
{
    [Serializable]
    public class GitHubAPIResponse
    {
        public string tag_name;
        public AssetResponse[] assets;
    }
}