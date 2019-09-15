using System.Collections.Generic;
using System.ComponentModel;
using CommandLine;
using CommandLine.Text;

namespace MitsubaArchivizer.Options
{
    [Verb("media", HelpText = "Downloads all pics/videos from a thread.")]
    internal class MediaDumpOptions : BaseOptions
    {
        [Usage(ApplicationAlias = "dotnet MitsubaArchivizer.dll")]
        public static IEnumerable<Example> Examples {
            get {
                yield return new Example("Download media from a mixed list of URLs/thread-ids into a custom directory", new MediaDumpOptions()
                {
                    InputSeq = new [] {"b_2137", "c_1337", "https://karakao.ork/p/res/1488.html"},
                    OutputDirectory = "/home/anon/archiwa-kurahenu/",
                });
                yield return new Example("Download media, download all thumbnails, restrict to certain extensions", new MediaDumpOptions()
                {
                    InputSeq = new [] {"<arg1>", "<arg2>", "...", "<argN>"},
                    DownloadThumbnails = true,
                    AllowedMediaExtensions = new [] {"gif", "png", "jpg"}
                });
            }
        }
    }
}