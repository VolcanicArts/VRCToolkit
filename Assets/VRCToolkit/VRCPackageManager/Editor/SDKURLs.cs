namespace VRCToolkit.VRCPackageManager.Editor
{
    public static class SDKURLs
    {
        private const string VrcBase = "https://vrchat.com/download/";
        public static string SDK2 => VrcBase + "sdk2";
        public static string SDK3Avatar => VrcBase + "sdk3-avatars";
        public static string SDK3World => VrcBase + "sdk3-worlds";

        public static string GetURL(string name)
        {
            switch (name)
            {
                case "SDK2":
                    return SDK2;
                case "SDK3World":
                    return SDK3World;
                case "SDK3Avatar":
                    return SDK3Avatar;
            }

            return null;
        }
    }
}