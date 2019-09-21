using System;
using System.IO;
using System.Linq;
using MitsubaArchivizer.Models;

namespace MitsubaArchivizer.Utils
{
    public class PathUtils
    {
        internal static string MakeRelativePath(string fromPath, string toPath)
        {
            if (string.IsNullOrEmpty(fromPath))
            {
                throw new ArgumentNullException(nameof(fromPath));
            }

            if (string.IsNullOrEmpty(toPath))
            {
                throw new ArgumentNullException(nameof(toPath));
            }

            if (fromPath.Last() != Path.DirectorySeparatorChar)
            {
                fromPath += Path.DirectorySeparatorChar;
            }

            if (toPath.Last() != Path.DirectorySeparatorChar)
            {
                toPath += Path.DirectorySeparatorChar;
            }
            
            var fromUri = new Uri(fromPath);
            var toUri = new Uri(toPath);

            if (fromUri.Scheme != toUri.Scheme)
            {
                return toPath;
            }

            var relativeUri = fromUri.MakeRelativeUri(toUri);
            var relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            if (toUri.Scheme.Equals("file", StringComparison.InvariantCultureIgnoreCase))
            {
                relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            }

            return relativePath.Trim(Path.DirectorySeparatorChar);
        }
        
        public static string GetBaseOutputPath(string outputDir) => string.IsNullOrEmpty(outputDir)
            ? Environment.CurrentDirectory
            : outputDir;

        public static string GetOutputPath(string outputDir, Thread thread) =>
            Path.Combine(GetBaseOutputPath(outputDir), $"{thread.Board}_{thread.Posts.First().Number}");
    }
}