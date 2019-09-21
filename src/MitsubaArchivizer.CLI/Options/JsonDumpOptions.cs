using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace MitsubaArchivizer.CLI.Options
{
    [Verb("json", HelpText = "Parses thread data and serializes it into a single json file.")]
    internal class JsonDumpOptions : BaseOptions
    {
        [Option( "no-media", HelpText = "Don't download media files.")]
        public bool DontResolveMedia { get; set; }
        
        [Option('f', "formatted", HelpText = "Serialize into human-readable JSON.")]
        public bool Formatted { get; set; }
        
        [Usage(ApplicationAlias = "dotnet MitsubaArchivizer.CLI.dll")]
        public static IEnumerable<Example> Examples {
            get {
                yield return new Example("Serialize into JSON, download media from a mixed list of URLs/thread-ids into a custom directory", new JsonDumpOptions()
                {
                    InputSeq = new [] {"b_2137", "c_1337", "https://karakao.ork/p/res/1488.html"},
                    OutputDirectory = "/home/anon/archiwa-kurahenu/",
                });
                yield return new Example("Serialize into human-readable JSON", new JsonDumpOptions()
                {
                    InputSeq = new [] {"<arg1>", "<arg2>", "...", "<argN>"},
                    Formatted = true
                });
                yield return new Example("Serialize into JSON without any media (text-only)", new JsonDumpOptions()
                {
                    InputSeq = new [] {"<arg1>", "<arg2>", "...", "<argN>"},
                    DontResolveMedia = true
                });
            }
        }
    }
}