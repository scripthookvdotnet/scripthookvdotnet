using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace GTA
{
    public static class ScriptHookVersionComparer
    {
        /// <summary>
        /// Regex for a version string which contains a number for the major, minor, build and revision version.
        /// </summary>
        public static Regex VersionNumberRegex = new Regex("(\\d+)\\.(\\d+)\\.(\\d+)\\.(\\d+)");

        /// <summary>
        /// -1 = Smaller
        /// 0 = Equals
        /// 1 = Bigger
        /// </summary>
        /// <param name="versionAttribute"></param>
        /// <returns></returns>
        public static int CompareToAssemblyVersion(this ScriptHookVersionAttribute versionAttribute)
        {
            var ret = 0;
            Version minVersion;
            var match = VersionNumberRegex.Match(versionAttribute.MinRequiredVersion);
            var assemblyVersion = Assembly.GetAssembly(typeof(Script)).GetName().Version;
            if (match.Success && assemblyVersion != null)
            {
                minVersion = new Version(int.Parse(match.Groups[1].Value),
                    int.Parse(match.Groups[2].Value),
                    int.Parse(match.Groups[3].Value),
                    int.Parse(match.Groups[4].Value));

                return minVersion.CompareTo(assemblyVersion);
            }
            return ret;
        }

    }
}
