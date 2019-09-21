using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using MitsubaArchivizer.Models;
using MitsubaArchivizer.Utils;
using Scriban;

namespace MitsubaArchivizer.Processors
{
    public class HtmlGeneratorProcessor : IProcessor
    {
        public string OutputDirectory { get; set; }
        public bool IncludeCustomPosterNames { get; set; }
        public bool UseColoredNames { get; set; }
        public bool IncludeSameFagCount { get; set; }
        public string Style { get; set; }
        public bool Prettify { get; set; }

        private readonly string _resourcesPath;
        private readonly Template _template;
        
        public HtmlGeneratorProcessor(string outputDirectory, bool includeCustomPosterNames, bool useColoredNames, bool includeSameFagCount, string style, bool prettify)
        {
            OutputDirectory = outputDirectory;
            IncludeCustomPosterNames = includeCustomPosterNames;
            UseColoredNames = useColoredNames;
            IncludeSameFagCount = includeSameFagCount;
            Style = style;
            Prettify = prettify;
            
            _resourcesPath = Path.Combine(Environment.CurrentDirectory, "Resources");

            if (!Directory.Exists(_resourcesPath))
            {
                _resourcesPath = Path.Combine(Assembly.GetExecutingAssembly().Location, "Resources");
            }

            if (!Directory.Exists(_resourcesPath))
            {
                throw new Exception("Failed to find Resources directory.");
            }

            var templatePath = Path.Combine(_resourcesPath, "template.sbn-html");
            if (!File.Exists(templatePath))
            {
                throw new Exception("Failed to find template file.");
            }
            
            _template = Template.Parse(File.ReadAllText(templatePath));
        }

        public string GetName() => "HTML Generator";

        public async Task ProcessThread(Thread thread)
        {
            var baseOutDir = Path.Combine(PathUtils.GetBaseOutputPath(OutputDirectory), "Resources");

            foreach (var dirPath in Directory.GetDirectories(_resourcesPath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(_resourcesPath, baseOutDir));
            }

            foreach (var newPath in Directory.GetFiles(_resourcesPath, "*.*", SearchOption.AllDirectories))
            {
                if (newPath.EndsWith("template.sbn-html"))
                {
                    continue;
                }

                var finalResPath = newPath.Replace(_resourcesPath, baseOutDir);

                if (!File.Exists(finalResPath))
                {
                    File.Copy(newPath, finalResPath, false);
                }
            }

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

            var sameFagRatio = (double) (thread.Posts.Count + 1) / (double) uniqueIds;

            var context = new
            {
                PostCount = idToPostCountMap,
                PostReferences = numberToReferencesMap,
                UniqueIds = uniqueIds,
                SameFagRatio = sameFagRatio,
                UseNames = IncludeCustomPosterNames,
                UseColors = UseColoredNames,
                UseSameFagCount = IncludeSameFagCount,
                Style = Style,
                Thread = thread
            };

            var outDir = PathUtils.GetOutputPath(OutputDirectory, thread);

            Directory.CreateDirectory(outDir);

            var finalFilePath = Path.Combine(outDir, "index.html");

            var html = _template.Render(context);

            var browsingContext = BrowsingContext.New(Configuration.Default);
            var dom = await browsingContext.OpenAsync(req => req.Content(html));

            var finalHtml = Prettify ? dom.Prettify() : dom.Minify();

            File.WriteAllText(finalFilePath, finalHtml, Encoding.UTF8);
        }
    }
}