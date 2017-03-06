using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FrameworkSniffer
{
    public static class Utilities
    {
        public static void ProcessAssemblies(FileInfo[] fileInfos, bool writeToFile = false)
        {
            Console.WriteLine($"{fileInfos.Length} Assemblies found.");

            //TODO: Refactor to a proper logging mechanisim 
            var filestream = new FileStream("results.txt", FileMode.Create);
            var streamwriter = new StreamWriter(filestream) {AutoFlush = true};
            Console.SetOut(writeToFile ? streamwriter : Console.Out);
            Console.SetError(streamwriter);

            foreach (var fileInfo in fileInfos)
            {
                try
                {
                    var a = Assembly.ReflectionOnlyLoadFrom(fileInfo.FullName);
                    var runtimeVersion = a.ImageRuntimeVersion;
                    var cleanRuntimeVersion = runtimeVersion.Replace('v', ' ').Trim();

                    Console.WriteLine(
                        $"Name:{fileInfo.Name} | CLR: {runtimeVersion} | Framework: {GetFrameworkVersionFromClrVersion(cleanRuntimeVersion)}");
                }
                catch (BadImageFormatException)
                {
                    Console.WriteLine($"{fileInfo.Name}: Unknown");
                }
                catch (FileLoadException)
                {
                    //Swalow
                }

            }

            streamwriter.Close();
            filestream.Close();
        }

        private static string GetFrameworkVersionFromClrVersion(string clrVersion)
        {
            var buildVersion = ProcessFrameworkRelease(clrVersion);
            var result = string.Empty;
            try
            {
                var simpleVersions = new Dictionary<Func<int, bool>, Action>
                {
                    {x => x == 0, () => result = "2.0"},
                    {x => x == 1, () => result = "3.0"},
                    {x => x == 2, () => result = "3.5"},
                    {x => x == 3, () => result = "3.5 SP1"},
                    {x => x == 4, () => result = "4.0 Full"},
                    {x => x == 4016, () => result = "2.0 SP2"},
                    {x => x == 4037, () => result = "3.0 SP2"},
                    {x => x == 378389, () => result = "4.5"},
                    {x => x <= 378675, () => result = "4.5.1"},
                    {x => x <= 378758, () => result = "4.5.1"},
                    {x => x <= 379893, () => result = "4.5.2"},
                    {x => x <= 393295, () => result = "4.6"},
                    {x => x <= 393297, () => result = "4.6"},
                    {x => x <= 394254, () => result = "4.6.1"},
                    {x => x <= 394271, () => result = "4.6.1"},
                    {x => x <= 394802, () => result = "4.6.2"},
                    {x => x <= 394806, () => result = "4.6.2"},
                    {x => x > 394806, () => result = "Unknown"}

                };

                simpleVersions.First(sv => sv.Key(buildVersion)).Value();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return result;
        }

        /// <summary>
        /// Place framework version edge cases here. i.e. Some do not have a build number, or some are using a custom build of the framework
        /// </summary>
        /// <param name="clrVersion"></param>
        /// <returns></returns>
        private static int ProcessFrameworkRelease(string clrVersion)
        {
            var releaseVersion = 0;
            switch (clrVersion)
            {
                case "2.0":
                case "2.0.50727":
                    releaseVersion = 0;
                    break;
                case "3.0":
                    releaseVersion = 1;
                    break;
                case "3.5":
                    releaseVersion = 2;
                    break;
                case "3.5.30729.01":
                    releaseVersion = 3;
                    break;
                case "4.0":
                case "4.0.30319":
                case "4.0.CORE":
                    releaseVersion = 4;
                    break;
                default:
                    var version = new Version(clrVersion);
                    releaseVersion = version.Build;
                    break;
            }

            return releaseVersion;
        }
    }
}
