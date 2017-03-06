using System;
using System.IO;

namespace FrameworkSniffer
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = string.Empty;
            var writeToFile = false;

            if (args.Length == 0)
            {
                Console.WriteLine("Please specify a path to search.");
                path = Console.ReadLine();
            }
            else
            {
                path = args[0];
            }

            if (args.Length > 0 && args[1] != null)
            {
                bool.TryParse(args[1], out writeToFile);
            }

            var di = new DirectoryInfo(Path.GetDirectoryName(path));
            var fileInfos = di.GetFiles("*.dll", SearchOption.AllDirectories);

            Utilities.ProcessAssemblies(fileInfos, writeToFile);
        }
    }
}
