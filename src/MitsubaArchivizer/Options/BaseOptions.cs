using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace MitsubaArchivizer.Options
{
    internal class BaseOptions
    {
        [Value(0, HelpText = "List of either: URLs pointing to a thread or thread-identificators.")]
        public IEnumerable<string> InputSeq { get; set; }

        [Option('e', "extensions", Separator = ',', HelpText = "Comma separated list of allowed media files extensions.")]
        public IEnumerable<string> AllowedMediaExtensions { get; set; }
        
        [Option('o', "out", HelpText = "Output directory, defaults to current working directory.")]
        public string OutputDirectory { get; set; }
        
        [Option("dont-separate-media", HelpText = "Don't group media files by their extensions.")]
        public bool DontSeperateMediaByExtension { get; set; }
        
        [Option('t', "thumbnails", HelpText = "Download media thumbnails. Animated previews for videos.")]
        public bool DownloadThumbnails { get; set; }
    }
}