using System.Collections.Generic;
using CommandLine;

namespace MitsubaArchivizer.CLI.Options
{
    internal class BaseOptions
    {
        [Value(0, HelpText = "List of either: URLs pointing to a thread or thread-identificators.", Min = 1)]
        public IEnumerable<string> InputSeq { get; set; }

        [Option("ext", Separator = ',', HelpText = "Comma separated list of allowed media files extensions.")]
        public IEnumerable<string> AllowedMediaExtensions { get; set; }
        
        [Option("thumb-ext", Separator = ',', HelpText = "Comma separated list of allowed thumbnail files extensions.", Default = new [] {"mp4", "webm"})]
        public IEnumerable<string> AllowedThumbnailExtensions { get; set; }
        
        [Option('o', "out", HelpText = "Output directory, defaults to current working directory.")]
        public string OutputDirectory { get; set; }
        
        [Option("dont-separate-media", HelpText = "Don't group media files by their extensions.")]
        public bool DontSeperateMediaByExtension { get; set; }
        
        [Option("no-thumbnails", HelpText = "Don't download media thumbnails. No animated previews for videos.")]
        public bool DontDownloadThumbnails { get; set; }
    }
}