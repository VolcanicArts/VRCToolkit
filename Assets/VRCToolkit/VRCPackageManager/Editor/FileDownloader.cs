using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace VRCToolkit.VRCPackageManager.Editor
{
    public class FileDownloader
    {
        private readonly string formattedName;
        private readonly string filePath;
        private readonly UnityWebRequest uwr;

        public FileDownloader(string formattedName, string url, string fileName)
        {
            this.formattedName = formattedName;
            filePath = $"{Application.dataPath}/VRCToolkit/VRCPackageManager/Downloads/{fileName}";
            uwr = new UnityWebRequest(url) {downloadHandler = new DownloadHandlerFile(filePath)};
        }

        public string ExecuteDownload()
        {
            uwr.SendWebRequest();
            Logger.Log($"Attempting to download {formattedName} using URL {uwr.url}");

            while (!uwr.isDone)
            {
                EditorUtility.DisplayProgressBar($"[VRCPackageManager] Downloading {formattedName}", "", uwr.downloadProgress);
            }

            EditorUtility.ClearProgressBar();

            if (uwr.error != null)
            {
                Logger.LogError(uwr.error);
                return null;
            }

            Logger.Log($"{formattedName} has been successfully downloaded!");
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            return filePath;
        }
    }
}