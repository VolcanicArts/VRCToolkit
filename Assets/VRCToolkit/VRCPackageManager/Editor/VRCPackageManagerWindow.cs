using UnityEditor;
using VRCToolkit.VRCPackageManager.Editor.Screen;

namespace VRCToolkit.VRCPackageManager.Editor
{
    public class VRCPackageManagerWindow : EditorWindow
    {
        public static string installedSDK;
        public static int selectedScreen = 0;

        private static readonly VRCPackageManagerScreen[] _screens = {new SDKScreen(), new PackageScreen()};

        private void OnGUI()
        {
            _screens[selectedScreen].OnGUI();
        }

        [MenuItem("VRCToolkit/VRCPackageManager")]
        public static void ShowWindow()
        {
            GetWindow<VRCPackageManagerWindow>("VRCPackageManager");
        }
    }
}