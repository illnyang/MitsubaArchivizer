using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MitsubaArchivizer.Models;
using MitsubaArchivizer.Utils;

namespace MitsubaArchivizer
{
    public class MediaResolver
    {
        public bool ResolveMedia { get; set; }
        public string OutputDirectory { get; set; }
        public bool SeparateMediaByExtension { get; set; }
        public bool DownloadThumbnails { get; set; }
        public IEnumerable<string> AllowedMediaExtensions { get; set; }
        public IEnumerable<string> AllowedThumbnailExtensions { get; set; }

        public delegate void PostWithMediaCount(int count);
        public event PostWithMediaCount OnPostWithMediaCount;

        public delegate void ProcessingPostMedia(Post post, int idx);
        public event ProcessingPostMedia OnProcessingPostMedia;

        public MediaResolver(bool resolveMedia, string outputDirectory, bool separateMediaByExtension, bool downloadThumbnails, IEnumerable<string> allowedMediaExtensions, IEnumerable<string> allowedThumbnailExtensions)
        {
            ResolveMedia = resolveMedia;
            OutputDirectory = outputDirectory;
            SeparateMediaByExtension = separateMediaByExtension;
            DownloadThumbnails = downloadThumbnails;
            AllowedMediaExtensions = allowedMediaExtensions;
            AllowedThumbnailExtensions = allowedThumbnailExtensions;
        }

        public MediaResolver()
        {
            ResolveMedia = false;
        }
        
        public async Task ResolveMediaForThread(Thread thread)
        {
            var outDir = PathUtils.GetOutputPath(OutputDirectory, thread);

            var mediaOutDir = Path.Combine(outDir, "media");

            var allPosts = thread.Posts.Where(x => x.File != null).ToList();

            OnPostWithMediaCount?.Invoke(allPosts.Count);

            for (int i = 0; i < allPosts.Count; i++)
            {
                Post post = allPosts[i];

                if (!ResolveMedia)
                {
                    post.File = null;
                    continue;
                }

                var file = post.File;

                var ext = Path.GetExtension(file.FileUrl).Substring(1);

                if (AllowedMediaExtensions != null && AllowedMediaExtensions.Any() && !AllowedMediaExtensions.Contains(ext))
                {
                    post.File = null;
                    continue;
                }

                OnProcessingPostMedia?.Invoke(post, i);

                var finalOutDir = mediaOutDir;

                if (SeparateMediaByExtension)
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
                            post.File = null;
                            continue;
                        }

                        File.WriteAllBytes(finalFilePath, await response.Content.ReadAsByteArrayAsync());
                    }
                }

                post.File.FileUrl = PathUtils.MakeRelativePath(outDir, finalFilePath);

                if (DownloadThumbnails)
                {
                    if (string.Equals(post.File.FileThumbUrl, "SPOILER",
                        StringComparison.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }

                    if (AllowedThumbnailExtensions != null && AllowedThumbnailExtensions.Any() &&
                        !AllowedThumbnailExtensions.Contains(ext))
                    {
                        post.File.FileThumbUrl = null;
                        continue;
                    }

                    var finalOutDirThumb = Path.Combine(finalOutDir, "thumb");

                    Directory.CreateDirectory(finalOutDirThumb);

                    var extension =
                        post.File.FileThumbUrl.Substring(
                            post.File.FileThumbUrl.LastIndexOf(".", StringComparison.Ordinal));
                    var finalThumbPath = Path.Combine(finalOutDirThumb,
                        Path.GetFileNameWithoutExtension(file.FileName) + extension);

                    if (!File.Exists(finalThumbPath))
                    {
                        using (var hc = new HttpClient())
                        {
                            var response = await hc.GetAsync(file.FileThumbUrl);
                            if (!response.IsSuccessStatusCode)
                            {
                                post.File.FileThumbUrl = null;
                                continue;
                            }

                            File.WriteAllBytes(finalThumbPath, await response.Content.ReadAsByteArrayAsync());
                        }
                    }

                    post.File.FileThumbUrl = PathUtils.MakeRelativePath(outDir, finalThumbPath);
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
        }
    }
}