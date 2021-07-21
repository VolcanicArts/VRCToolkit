using System.IO;
using UnityEditor;
using UnityEngine;
using VRCToolkit.VRCPackageManager.Editor.VRCPackage;

namespace VRCToolkit.VRCPackageManager.Editor
{
    [InitializeOnLoad]
    internal class Startup
    {
        public static readonly VRCPackageData packageData;
        
        static Startup()
        {
            var packageDataLocation = $"{Application.dataPath}/VRCToolkit/VRCPackageManager/Editor/Resources/VRCPackages.json";
            var packageDataJson = File.ReadAllText(packageDataLocation);
            packageData = JsonUtility.FromJson<VRCPackageData>(packageDataJson);
        }
    }
}