using System;

namespace VRCToolkit.VRCPackageManager.Editor.VRCPackage
{
    [Serializable]
    public class VRCPackageData
    {
        public VRCPackagePage[] SDK2;
        public VRCPackagePage[] SDK3World;
        public VRCPackagePage[] SDK3Avatar;
    }
}