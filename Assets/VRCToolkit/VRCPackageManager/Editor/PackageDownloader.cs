using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace VRCToolkit.VRCPackageManager.Editor
{
    public class PackageDownloader
    {
        private readonly string packageName;
        private readonly string url;
        private readonly string path;
        private readonly UnityWebRequest uwr;
        
        public PackageDownloader(string packageName, string url, string fileName)
        {
            this.packageName = packageName;
            this.url = url;
            uwr = new UnityWebRequest(url);
            path = $"{Application.dataPath}/VRCToolkit/VRCPackageManager/Downloads/{fileName}";
            uwr.downloadHandler = new DownloadHandlerFile(path);
        }
            
        public void ExecuteDownload()
        {
            uwr.SendWebRequest();
            Logger.Log($"Attempting to download {packageName} using URL {url}");

            while (!uwr.isDone)
            {
                EditorUtility.DisplayProgressBar($"[VRCPackageManager] Downloading {packageName}", "", uwr.downloadProgress);
            }
            EditorUtility.ClearProgressBar();
            
            if (uwr.error == null)
            {
                Logger.Log($"{packageName} successfully downloaded. Importing unitypackage!");
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                AssetDatabase.ImportPackage(path, false);
            }
            else
            {
                Logger.LogError(uwr.error);;
            }
        }
    }
}