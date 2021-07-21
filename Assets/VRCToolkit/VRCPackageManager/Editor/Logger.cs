using UnityEngine;

namespace VRCToolkit.VRCPackageManager.Editor
{
    public class Logger
    {
        
        private const string LogPrefix = "[VRCToolkit/VRCPackageManager]";
        
        public static void Log(string message)
        {
            Debug.Log($"{LogPrefix} {message}");
        }

        public static void LogError(string message)
        {
            Debug.LogError($"{LogPrefix} {message}");
        }
    }
}