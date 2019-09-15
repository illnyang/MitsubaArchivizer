using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using CommandLine;
using CommandLine.Text;
using MitsubaArchivizer.CLI;
using MitsubaArchivizer.Models;
using MitsubaArchivizer.Options;
using Newtonsoft.Json;
using Scriban;

namespace MitsubaArchivizer
{
    internal static class Program
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        
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
            
            var result = parser.ParseArguments<HtmlDumpOptions, JsonDumpOptions, MediaDumpOptions>(args);

            return result.MapResult(
                (HtmlDumpOptions opts) =>
                {
                    try
                    {
                        RunHtmlDump(opts).Wait();
                    }
                    catch (Exception ex)
                    {
                        Logger.Fatal(ex);
                        return 1;
                    }
                    return 0;
                },
                (JsonDumpOptions opts) =>
                {
                    try
                    {
                        RunJsonDump(opts).Wait();
                    }
                    catch (Exception ex)
                    {
                        Logger.Fatal(ex);
                        return 1;
                    }
                    return 0;
                },
                (MediaDumpOptions opts) =>
                {
                    try
                    {
                        RunMediaDump(opts).Wait();
                    }
                    catch (Exception ex)
                    {
                        Logger.Fatal(ex);
                        return 1;
                    }
                    return 0;
                },
                errs =>
                {
                    var helpText = HelpText.AutoBuild(result, h =>
                    {
                        h = HelpText.DefaultParsingErrorsHandler(result, h);
                        h.Copyright = "wykucowane by DJMATI";
                        h.Heading = $"MitsubaArchivizer @ {ThisAssembly.Git.Commit}";
                        h.AutoVersion = false;
                        h.AddDashesToOption = true;
                        return h;
                    }, e => e, true, 190);

                    Console.Error.WriteLine(helpText);
                    
                    return 1;
                });
        }

        private static string GetBaseOutputPath(BaseOptions options) => string.IsNullOrEmpty(options.OutputDirectory)
            ? Environment.CurrentDirectory
            : options.OutputDirectory;

        private static string GetOutputPath(BaseOptions options, Thread thread) =>
            Path.Combine(GetBaseOutputPath(options), $"{thread.Board}_{thread.Posts.First().Number}");
        
        private static async Task RunHtmlDump(HtmlDumpOptions options)
        {
            Logger.Info("Selected mode: Standalone HTML");
            Logger.Debug("Searching for template files...");
            
            var resourcesPath = Path.Combine(Environment.CurrentDirectory, "Resources");

            if (!Directory.Exists(resourcesPath))
            {
                resourcesPath = Path.Combine(Assembly.GetExecutingAssembly().Location, "Resources");
                Logger.Debug("'Resources' folder was not found in current working directory, searching in executing assemby's location.");
            }

            if (!Directory.Exists(resourcesPath))
            {
                Logger.Error("Could not find 'Resources' folder. Aborting.");
                return;
            }

            var templatePath = Path.Combine(resourcesPath, "template.sbn-html");
            if (!File.Exists(templatePath))
            {
                Logger.Error("Could not find template file within 'Resources' directory. Aborting.");
                return;
            }
            
            Logger.Info("Found template files!");

            var threads = await RunBaseDump(options, !options.DontResolveMedia);

            var baseOutDir = Path.Combine(GetBaseOutputPath(options), "Resources");

            foreach (var dirPath in Directory.GetDirectories(resourcesPath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(resourcesPath, baseOutDir));
            }
            
            Logger.Info("Copying template resources to the output dir...");
            
            foreach (var newPath in Directory.GetFiles(resourcesPath, "*.*", SearchOption.AllDirectories))
            {
                if (newPath.EndsWith("template.sbn-html"))
                {
                    continue;
                }

                var finalResPath = newPath.Replace(resourcesPath, baseOutDir);

                if (!File.Exists(finalResPath))
                {
                    File.Copy(newPath, finalResPath, false);
                }
            }
            
            Logger.Info("Parsing template file...");
            
            var template = Template.Parse(await File.ReadAllTextAsync(templatePath));

            foreach (var thread in threads)
            {
                var idToPostCountMap = new Dictionary<string, uint>();
                var numberToReferencesMap = new Dictionary<string, List<string>>();

                var posts = thread.Posts.ToList();

                for (var i = 0; i < posts.Count; i++)
                {
                    var post = posts[i];

                    if (!string.IsNullOrEmpty(post.Id))
                    {
                        if (!idToPostCountMap.ContainsKey(post.Id))
                        {
                            idToPostCountMap.Add(post.Id, 0);
                        }

                        idToPostCountMap[post.Id]++;
                    }
                    
                    if (post.Number.HasValue)
                    {
                        var refList = new List<string>();

                        for (var j = i; j < posts.Count; j++)
                        {
                            var nextPost = posts[j];
                            if (nextPost.Number.HasValue && nextPost.MessageText.Contains($">>{post.Number}"))
                            {
                                refList.Add(nextPost.Number.Value.ToString());
                            }
                        }

                        if (refList.Any())
                        {
                            numberToReferencesMap.Add(post.Number.Value.ToString(), refList);
                        }
                    }
                }

                var uniqueIds = idToPostCountMap.Count;

                var sameFagRatio = (double)(thread.Posts.Count + 1) / (double)uniqueIds;

                var context = new
                {
                    PostCount = idToPostCountMap,
                    PostReferences = numberToReferencesMap,
                    UniqueIds = uniqueIds,
                    SameFagRatio = sameFagRatio,
                    UseNames = !options.DontIncludeCustomPosterNames,
                    UseColors = !options.DontUseColoredNames,
                    UseSameFagCount = !options.DontIncludeSameFagCount,
                    Style = options.Style,
                    Thread = thread
                };
                
                var outDir = GetOutputPath(options, thread);
                
                Directory.CreateDirectory(outDir);

                var finalFilePath = Path.Combine(outDir, "index.html");
                
                Logger.Info("Rendering output HTML...");
                var html = template.Render(context);
                
                Logger.Info("{0} output HTML...", options.Prettify ? "Prettifying" : "Minifying");
                var browsingContext = BrowsingContext.New(Configuration.Default);
                var dom = await browsingContext.OpenAsync(req => req.Content(html));

                var finalHtml = options.Prettify ? dom.Prettify() : dom.Minify();
                
                await File.WriteAllTextAsync(finalFilePath, finalHtml, Encoding.UTF8);
                Logger.Info("Done! Saved to {0}", finalFilePath);
            }
        }
        
        private static async Task RunJsonDump(JsonDumpOptions options)
        {
            Logger.Info("Selected mode: JSON");
            var threads = await RunBaseDump(options, !options.DontResolveMedia);
            foreach (var thread in threads)
            {
                var outDir = GetOutputPath(options, thread);

                Directory.CreateDirectory(outDir);
                
                var finalFilePath = Path.Combine(outDir, "thread.json");
                Logger.Info("Serializing to JSON...");
                var serialized = JsonConvert.SerializeObject(thread, options.Formatted ? Formatting.Indented : Formatting.None);
                await File.WriteAllTextAsync(finalFilePath, serialized);
                Logger.Info("Done! Saved to {0}", finalFilePath);
            }
        }
        
        private static async Task RunMediaDump(MediaDumpOptions options)
        {
            await RunBaseDump(options, true);
        }
        
        private static async Task<List<Thread>> RunBaseDump(BaseOptions options, bool downloadMedia)
        {
            Logger.Info("Performing initial parsing...");

            if (!downloadMedia)
            {
                Logger.Info("Media files will be ignored due to a user's request.");
            }

            if (!options.DownloadThumbnails)
            {
                Logger.Info("Media thumbnails will be ignored due to a user's request.");
            }
            
            var result = new List<Thread>();
            
            foreach (var input in options.InputSeq)
            {
                Logger.Info("Trying to parse thread: '{0}'", input);
                var thread = await ThreadParser.TryParse(input);

                if (!downloadMedia)
                {
                    foreach (var post in thread.Posts)
                    {
                        post.File = null;
                    }
                    
                    result.Add(thread);
                    Logger.Info("Thread '{0}' from board '/{1}/' has been parsed!", thread.Posts[0].Number, thread.Board);
                    continue;
                }
                
                var outDir = GetOutputPath(options, thread);

                var mediaOutDir = Path.Combine(outDir, "media");

                var allPosts = thread.Posts.Where(x => x.File != null).ToList();

                foreach (var post in allPosts)
                {
                    var file = post.File;

                    var ext = Path.GetExtension(file.FileUrl).Substring(1);

                    if (options.AllowedMediaExtensions.Any() && !options.AllowedMediaExtensions.Contains(ext))
                    {
                        Logger.Info("Black-listed extension, media file '{0}' has been skipped.", file.FileName);
                        post.File = null;
                        continue;
                    }

                    var finalOutDir = mediaOutDir;

                    if (!options.DontSeperateMediaByExtension)
                    {
                        finalOutDir = Path.Combine(finalOutDir, ext);
                    }
                    
                    Directory.CreateDirectory(finalOutDir);

                    var finalFilePath = Path.Combine(finalOutDir, file.FileName);

                    if (!File.Exists(finalFilePath))
                    {
                        using (var hc = new HttpClient())
                        {
                            var response = await hc.GetAsync(file.FileUrl);
                            if (!response.IsSuccessStatusCode)
                            {
                                Logger.Warn("Failed to download media file '{0}' ({1}). Re-run the command to download missing files.", file.FileName, (int)response.StatusCode);
                                post.File = null;
                                continue;
                            }
                            
                            await File.WriteAllBytesAsync(finalFilePath, await response.Content.ReadAsByteArrayAsync());
                            Logger.Info("Downloaded media file: {0}", file.FileName);
                        }
                    }
                    else
                    {
                        Logger.Info("Media file already exists: {0}", file.FileName);
                    }

                    post.File.FileUrl = Path.GetRelativePath(outDir, finalFilePath);
                    
                    if (options.DownloadThumbnails)
                    {
                        if (string.Equals(post.File.FileThumbUrl, "SPOILER",
                            StringComparison.InvariantCultureIgnoreCase))
                        {
                            Logger.Info("Media file {0} does not have a thumbnail because its hidden in a spoiler.", file.FileName);
                            continue;
                        }
                        
                        var finalOutDirThumb = Path.Combine(finalOutDir, "thumb");
                        
                        Directory.CreateDirectory(finalOutDirThumb);

                        var extension = post.File.FileThumbUrl.Substring(post.File.FileThumbUrl.LastIndexOf(".", StringComparison.Ordinal));
                        var finalThumbPath = Path.Combine(finalOutDirThumb, Path.GetFileNameWithoutExtension(file.FileName) + extension);

                        if (!File.Exists(finalThumbPath))
                        {
                            using (var hc = new HttpClient())
                            {
                                var response = await hc.GetAsync(file.FileThumbUrl);
                                if (!response.IsSuccessStatusCode)
                                {
                                    post.File.FileThumbUrl = null;
                                    Logger.Warn("Failed to download media thumbnail for '{0}' ({1}). Re-run the command to download missing files.", file.FileName, (int)response.StatusCode);
                                    continue;
                                }
                                
                                await File.WriteAllBytesAsync(finalThumbPath, await response.Content.ReadAsByteArrayAsync());
                                Logger.Info("Downloaded thumbnail for media file: {0}", file.FileName);
                            }
                        }
                        else
                        {
                            Logger.Info("Thumbnail for media file already exists: {0}", file.FileName);
                        }

                        
                        post.File.FileThumbUrl = Path.GetRelativePath(outDir, finalThumbPath);
                    }
                    else
                    {
                        if (!string.Equals(post.File.FileThumbUrl, "SPOILER",
                            StringComparison.InvariantCultureIgnoreCase))
                        {
                            post.File.FileThumbUrl = null;
                        }
                    }
                }
                
                result.Add(thread);
                Logger.Info("Thread '{0}' from board '/{1}/' has been parsed!", thread.Posts[0].Number, thread.Board);
            }

            return result;
        }
    }
}