using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace VRCToolkit.VRCPackageManager
{
    public static class GitHubUtil
    {
        private const string GitHubAPIBase = "https://api.github.com/repos/";
        private const string GitHubAPILatestRelease = "/releases/latest";

        public static string GetLatestVersion(string repoName, string formattedName)
        {
            var url = GitHubAPIBase + repoName + GitHubAPILatestRelease;
            Logger.Log($"Requesting for latest version of {formattedName} using URL: {url}");
            var uwr = new UnityWebRequest(url) { downloadHandler = new DownloadHandlerBuffer() };
            uwr.SendWebRequest();

            while (!uwr.isDone)
                EditorUtility.DisplayProgressBar($"[VRCPackageManager] Getting latest version of {formattedName}", "", uwr.downloadProgress);

            EditorUtility.ClearProgressBar();

            if (uwr.error != null)
            {
                Logger.LogError($"Could not get latest version of {formattedName}. Aborting download");
                return null;
            }

            var responseData = uwr.downloadHandler.text;
            var gitHubData = JsonUtility.FromJson<GitHubAPIResponse>(responseData);
            Logger.Log($"Found latest version of {gitHubData.tag_name}");
            return gitHubData.tag_name;
        }
    }
}