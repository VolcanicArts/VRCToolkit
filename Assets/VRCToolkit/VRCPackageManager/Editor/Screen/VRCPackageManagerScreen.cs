using UnityEditor;
using UnityEngine;

namespace VRCToolkit.VRCPackageManager
{
    public class VRCPackageManagerScreen
    {
        public virtual void OnGUI()
        {
            DrawTitle();
        }

        private static void DrawTitle()
        {
            DrawCenteredTitle("VRCPackageManager");
            DrawCenteredText(
                "Welcome to the VRCPackageManager. Here you'll find a collection of useful tools, prefabs, the official SDKs for VRChat, and other packages in VRCToolkit");
        }

        protected static void DrawCenteredText(string text)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(40);
            GUILayout.Label(text, EditorStyles.wordWrappedLabel);
            GUILayout.Space(40);
            GUILayout.EndHorizontal();
        }

        protected static void DrawCenteredTitle(string title)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(title, EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        protected static bool DrawCenteredButton(string text)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(100);
            var pressed = GUILayout.Button(text, EditorStyles.miniButton);
            GUILayout.Space(100);
            GUILayout.EndHorizontal();
            return pressed;
        }

        protected static bool DrawCenteredToggle(string label, bool value)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            var newValue = EditorGUILayout.Toggle(label, value);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            return newValue;
        }

        protected static bool CheckIfPlaying()
        {
            if (!EditorApplication.isPlaying) return false;
            DrawCenteredTitle("Cannot download packages in play mode");
            return true;
        }
    }
}