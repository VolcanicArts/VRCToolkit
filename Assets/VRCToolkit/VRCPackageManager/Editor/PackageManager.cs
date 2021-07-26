using System.IO;
using UnityEngine;
using VRCToolkit.VRCPackageManager.Editor.VRCPackage;

namespace VRCToolkit.VRCPackageManager.Editor
{
    public static class PackageManager
    {
        public static VRCPackageData LoadDataFromFile()
        {
            var packageDataLocation = $"{Application.dataPath}/VRCToolkit/VRCPackageManager/Editor/Resources/VRCPackages.json";
            var packageDataJson = File.ReadAllText(packageDataLocation);
            return JsonUtility.FromJson<VRCPackageData>(packageDataJson);
        }
    }
}