using System;

namespace VRCToolkit.VRCPackageManager
{
    [Serializable]
    public class Settings
    {
        public string installedSDK;
        public PackageVersion[] installedPackages;
        public bool updateSDKOnStart;
    }
}