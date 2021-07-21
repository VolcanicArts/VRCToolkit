using System;

namespace VRCToolkit.VRCPackageManager.Editor.VRCPackage
{
    [Serializable]
    public class VRCPackageSection
    {
        public string title;
        public VRCPackage[] packages;
    }
}