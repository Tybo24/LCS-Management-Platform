using LCS_Management_Platform.Data;
using LCS_Management_Platform.Models;

namespace LCS_Management_Platform.Helpers
{
    /// <summary>
    /// Handles manipulation of data from models
    /// </summary>
    public class DataManipHelper
    {

        /// <summary>
        /// Moves Production environment to the top of the list and sets 
        /// application constants
        /// </summary>
        /// <param name="datum">List of environments</param>
        /// <returns>List of environments</returns>
        public static async Task<IList<EnvDataList>> ProductionEnvironmentFromList(IList<EnvDataList> datum)
        {
            var itemToMove = datum.FirstOrDefault(i => i.EnvironmentType == "Production"); // filter production env to top of list

            if (itemToMove != null)
            {
                datum.Remove(itemToMove);
                datum.Insert(0, itemToMove);
                AppState.envPlatformVersion = itemToMove.CurrentPlatformVersion;
                AppState.envBuildVersion = itemToMove.CurrentApplicationBuildVersion;
                AppState.envReleaseVersion = itemToMove.CurrentApplicationReleaseName;
            }
            // to account for if production environment is not specified
            else
            {
                AppState.envPlatformVersion = string.Empty;
                AppState.envBuildVersion = string.Empty;
                AppState.envReleaseVersion = string.Empty;
            }

            return datum;
        }

        /// <summary>
        /// Retrieves the most current customisation for the data list provided.
        /// </summary>
        /// <param name="datum">List of environment history data from LCS</param>
        /// <returns>String of update package name</returns>
        public static string CurrentCustomisation(List<EnvHistDataList> datum)
        {
            EnvHistDataList? packageName = new EnvHistDataList();

            try
            {
                packageName = datum != null && datum.Any()
                    ? datum
                        .Where(d => d.TypeDisplay.ToLower() == ("update environment") 
                        || d.TypeDisplay.ToLower() == "application deployable package"
                        || d.TypeDisplay.ToLower() == "merged package"
                        && d.Status.ToLower() == "completed")
                        .OrderByDescending(d => d.StartDateTimeUTC)
                        .FirstOrDefault()
                    : null;
            }
            catch
            {
                packageName.Name = string.Empty;
            }

            if (packageName != null)
            {
                return packageName.Name;
            }

            return string.Empty;
        }

        /// <summary>
        /// Method used for comparing version numbers retreived from LCS.
        /// Numbers can omit the number 0 which makes comparing them as literals difficult.
        /// Break the sections down into arrays and then compare numbers + lengths.
        /// </summary>
        /// <param name="version1">First version number to compare</param>
        /// <param name="version2">Second version number to compare</param>
        /// <returns>
        /// 1 - First version parameter is ahead
        /// -1 - Second version parametre is ahead
        /// 0 - versions are equal
        /// -2 - data not correctly specified
        /// </returns>
        public static int CompareVersionNumbers(string version1, string version2)
        {
            if (!string.IsNullOrEmpty(version1) && !string.IsNullOrEmpty(version2))
            {
                var version1Parts = version1.Split('.').Select(int.Parse).ToArray();
                var version2Parts = version2.Split('.').Select(int.Parse).ToArray();

                int length = Math.Min(version1Parts.Length, version2Parts.Length);

                for (int i = 0; i < length; i++)
                {
                    if (version1Parts[i] > version2Parts[i])
                    {
                        return 1;  // version1 is ahead
                    }
                    if (version1Parts[i] < version2Parts[i])
                    {
                        return -1; // version1 is behind
                    }
                }

                // If both versions are equal so far, but one version has more parts
                if (version1Parts.Length > version2Parts.Length)
                {
                    return 1; // version1 has more parts, so it's ahead
                }
                if (version1Parts.Length < version2Parts.Length)
                {
                    return -1; // version1 has fewer parts, so it's behind
                }

                return 0; // Both versions are equal
            }

            return -2; // If no data specified, just return -2
        }
    }
}
