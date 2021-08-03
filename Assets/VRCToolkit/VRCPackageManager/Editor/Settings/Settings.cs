using System;

namespace VRCToolkit.VRCPackageManager.Editor.Settings
{
    [Serializable]
    public class Settings
    {
        public string installedSDK;
        public PackageVersion[] installedPackages;
        public bool updateSDKOnStart;
    }
}