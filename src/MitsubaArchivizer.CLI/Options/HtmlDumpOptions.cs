using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace MitsubaArchivizer.CLI.Options
{
    [Verb("html", HelpText = "Parses thread data and generates a human-readable, self-contained HTML page.")]
    internal class HtmlDumpOptions : BaseOptions
    {
        [Option("no-media", HelpText = "Don't download media files.")]
        public bool DontResolveMedia { get; set; }
        
        [Option("prettify", HelpText = "Prettify output HTML file.")]
        public bool Prettify { get; set; }
        
        [Option("no-names", HelpText = "Don't use randomly-chosen names instead of plain text.")]
        public bool DontIncludeCustomPosterNames { get; set; }
        
        [Option("no-colored-names", HelpText = "Don't use randomly-chosen color for names.")]
        public bool DontUseColoredNames { get; set; }
        
        [Option("no-samefag-count", HelpText = "Don't include post-count next to the poster ID.")]
        public bool DontIncludeSameFagCount { get; set; }
        
        [Option("style", Default = "dark_roach.css", HelpText = "Go check out 'Resources/styles' directory.")]
        public string Style { get; set; }
        
        [Usage(ApplicationAlias = "dotnet MitsubaArchivizer.CLI.dll")]
        public static IEnumerable<Example> Examples {
            get {
                yield return new Example("Generate HTML, download media from a mixed list of URLs/thread-ids into a custom directory", new HtmlDumpOptions()
                {
                    InputSeq = new [] {"b_2137", "c_1337", "https://karakao.ork/p/res/1488.html"},
                    OutputDirectory = "/home/anon/archiwa-kurahenu/",
                });
                yield return new Example("Generate HTML with custom style", new HtmlDumpOptions()
                {
                    InputSeq = new [] {"<arg1>", "<arg2>", "...", "<argN>"},
                    Style = "space.css"
                });
                yield return new Example("Generate HTML and prettify it", new HtmlDumpOptions()
                {
                    InputSeq = new [] {"<arg1>", "<arg2>", "...", "<argN>"},
                    Prettify = true
                });
                yield return new Example("Generate HTML without any media (text-only)", new HtmlDumpOptions()
                {
                    InputSeq = new [] {"<arg1>", "<arg2>", "...", "<argN>"},
                    DontResolveMedia = true
                });
            }
        }
    }
}