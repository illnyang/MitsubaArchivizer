using System;
using System.Collections.Generic;
using System.Drawing;
using Colorful;
using CommandLine;
using CommandLine.Text;
using MitsubaArchivizer.CLI.Options;
using MitsubaArchivizer.Processors;
using Console = Colorful.Console;

namespace MitsubaArchivizer.CLI
{
    internal static class Program
    {
        private static int Main(string[] args)
        {
            void Configuration(ParserSettings with)
            {
                with.EnableDashDash = true;
                with.CaseInsensitiveEnumValues = true;
                with.AutoHelp = true;
                with.HelpWriter = null;
            }

            var parser = new Parser(Configuration);

            var parsed = parser.ParseArguments<HtmlDumpOptions, JsonDumpOptions, MediaDumpOptions>(args);

            var result = 0;
            try
            {
                var resolveMedia = false;
                BaseOptions baseOpts = null;
                var processors = new List<IProcessor>();

                parsed
                    .WithParsed<HtmlDumpOptions>(opts =>
                    {
                        processors.Add(new HtmlGeneratorProcessor(
                             opts.OutputDirectory,
                            !opts.DontIncludeCustomPosterNames,
                            !opts.DontUseColoredNames,
                            !opts.DontIncludeSameFagCount, 
                             opts.Style, 
                             opts.Prettify));

                        baseOpts = opts;
                        resolveMedia = !opts.DontResolveMedia;
                    })
                    .WithParsed<JsonDumpOptions>(opts =>
                    {
                        processors.Add(new JsonSerializerProcessor(opts.OutputDirectory, opts.Formatted));
                        
                        baseOpts = opts;
                        resolveMedia = !opts.DontResolveMedia;
                    })
                    .WithParsed<MediaDumpOptions>(opts =>
                    {
                        baseOpts = opts;
                        resolveMedia = true;
                    })
                    .WithNotParsed(errs =>
                    {
                        var helpText = HelpText.AutoBuild(parsed, h =>
                        {
                            h = HelpText.DefaultParsingErrorsHandler(parsed, h);
                            h.Copyright = "";
                            h.Heading = "";
                            h.AutoVersion = false;
                            h.AddDashesToOption = true;
                            return h;
                        }, e => e, true, 190);
                        
                        Console.WriteAscii("Mitsuba Archivizer", Color.DodgerBlue);
                        
                        Console.WriteFormatted("commit {0} @ {1}", 
                            new Formatter(ThisAssembly.Git.Commit, Color.Crimson), 
                            new Formatter(ThisAssembly.Git.Branch, Color.Chartreuse), Color.White);
                        
                        Console.Write(helpText);

                        result = 1;
                    });

                if (result != 1)
                {
                    var postCount = 0;
                    var cursorBackupTop1 = -1;
                    var cursorBackupLeft1 = -1;
                    
                    ThreadParser.OnPostCount += count => postCount = count;
                    ThreadParser.OnPostParsing += idx =>
                    {
                        if (cursorBackupTop1 == -1 || cursorBackupLeft1 == -1)
                        {
                            cursorBackupTop1 = Console.CursorTop;
                            cursorBackupLeft1 = Console.CursorLeft;
                        }

                        Console.SetCursorPosition(cursorBackupLeft1, cursorBackupTop1);
                        Console.Write(new string(' ', Console.WindowWidth));
                        Console.SetCursorPosition(cursorBackupLeft1, cursorBackupTop1);
                        Console.WriteLine("Parsing post: ({0}/{1})", idx + 1, postCount);
                    };
                    
                    var resolver = new MediaResolver(
                        resolveMedia,
                        baseOpts.OutputDirectory,
                        !baseOpts.DontSeperateMediaByExtension,
                        !baseOpts.DontDownloadThumbnails,
                        baseOpts.AllowedMediaExtensions,
                        baseOpts.AllowedThumbnailExtensions);

                    var mediaCount = 0;
                    var cursorBackupTop2 = -1;
                    var cursorBackupLeft2 = -1;
                    
                    resolver.OnPostWithMediaCount += count => mediaCount = count;
                    resolver.OnProcessingPostMedia += (post, idx) =>
                    {
                        if (cursorBackupTop2 == -1 || cursorBackupLeft2 == -1)
                        {
                            cursorBackupTop2 = Console.CursorTop;
                            cursorBackupLeft2 = Console.CursorLeft;
                        }
                        
                        Console.SetCursorPosition(cursorBackupLeft2, cursorBackupTop2);
                        Console.Write(new string(' ', Console.WindowWidth));
                        Console.SetCursorPosition(cursorBackupLeft2, cursorBackupTop2);
                        Console.WriteLine("Downloading media: {0} ({1}/{2})", post.File.FileName, idx + 1, mediaCount);
                    };
                    
                    var pipeline = new ProcessorPipeline(processors, resolver);

                    pipeline.OnProcessorInvoked += name => Console.WriteLine("{0} is working...", name); 
                    
                    foreach (var input in baseOpts.InputSeq)
                    {
                        var thread = ThreadParser.TryParse(input).Result;
                        pipeline.Process(thread).Wait();
                    }
                
                    Console.WriteLine("Done!");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                result = 1;
            }

            return result;
        }
    }
}