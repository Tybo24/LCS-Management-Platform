namespace LCS_Management_Platform.Data
{
    public static class AppState
    {
        // access token for API calls
        public static string accessToken {  get; set; }
        // used to determine if a token has been acquired or not
        public static bool tokenAcquired { get; set; }
        // used to only request new token when required
        public static DateTime tokenExpiresAt = DateTime.MinValue;
        public static bool environmentData {  get; set; }
        // used to stop API calls when too many have been made
        public static DateTime waitUntil { get; set; }
        // used to determine if a timer is active
        public static bool timerActive {  get; set; }
        // used for refresh button to show current timer state
        public static int timerCountdown { get; set; }
        // used to show when API calls were last made
        public static string envDataLastUpdate { get; set; }
        public static string envPlatformVersion { get; set; }
        public static string envReleaseVersion { get; set; }
        public static string envBuildVersion { get; set; }
    }

}
