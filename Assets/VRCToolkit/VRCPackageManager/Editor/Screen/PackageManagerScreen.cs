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
            GUILayout.Space(10);
            DrawCenteredTitle("VRCPackageManager");
            DrawCenteredText(
                "Welcome to the VRCPackageManager. Here you'll find a collection of useful tools, prefabs, the official SDKs for VRChat, and other packages in VRCToolkit");
            GUILayout.Space(10);
        }

        protected static void DrawCenteredText(string text)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            GUILayout.Label(text, new GUIStyle
            {
                alignment = TextAnchor.MiddleCenter,
                wordWrap = true,
                fontStyle = FontStyle.Normal,
                normal = new GUIStyleState {textColor = Color.white}
            });
            GUILayout.Space(10);
            GUILayout.EndHorizontal();
        }

        protected static void DrawCenteredTitle(string title)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(title, new GUIStyle
            {
                alignment = TextAnchor.MiddleCenter,
                wordWrap = true,
                fontStyle = FontStyle.Bold,
                normal = new GUIStyleState {textColor = Color.white}
            });
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