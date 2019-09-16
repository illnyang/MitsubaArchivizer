using System;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using MitsubaArchivizer.Models;
using MitsubaArchivizer.Utils;
using Url = Flurl.Url;

namespace MitsubaArchivizer
{
    internal static class ThreadParser
    {
        private const string Domain = "karachan.org";
        
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private static string SanitizeRelativeSrcUrl(string url, int idx) => url != null ? Url.Combine($"https://{Domain}/", url.Substring(idx)) : null;
        private static Uri GetThreadUri(string board, uint id) => new Uri($@"https://{Domain}/{board}/res/{id}.html"); 
        
        public static async Task<Thread> TryParse(string input)
        {
            var match = Regex.Match(input, $@"^https?:\/\/{Domain}\/(.+)\/res\/(\d+)");
            if (match.Success && match.Groups.Count == 3)
            {
                return await ParseFromMatch(match);
            }
            
            match = Regex.Match(input, @"(\w+)_(\d+)");
            if (match.Success && match.Groups.Count == 3)
            {
                return await ParseFromMatch(match);
            }
            
            throw new ArgumentException("Provided input was neither an url nor a thread-identificator.");
        }
        
        private static async Task<Thread> ParseFromMatch(Match match)
        {
            var board = match.Groups[1].Value;
            var id = Convert.ToUInt32(match.Groups[2].Value);

            return await ParseThreadInternal(GetThreadUri(board, id), board, id);
        }

        private static async Task<Thread> ParseThreadInternal(Uri threadUri, string board, uint id)
        {
            using (var hc = new HttpClient())
            {
                var response = await hc.GetAsync(threadUri);

                if (!response.IsSuccessStatusCode)
                {
                    switch ((int)response.StatusCode)
                    {
                        case 404:
                            throw new Exception($"Provided thread({board}_{id}) is either non-existent or was already deleted (404).");
                        case 520:
                            throw new Exception($"Karakao sie zesralo xD. (thread: {board}_{id})");
                        default:
                            throw new Exception($"Failed to get provided thread's content ({board}_{id}) - remote host returned {Convert.ToUInt32(response.StatusCode)}.");
                    }
                }

                var rawPage = await response.Content.ReadAsStringAsync();
                
                var context = BrowsingContext.New(Configuration.Default);
                var dom = await context.OpenAsync(req => req.Content(rawPage));
                
                var elemThread = dom.All.SingleOrDefault(x =>
                    x.LocalName == "div" &&
                    x.ClassList.Contains("thread") &&
                    x.HasAttribute("data-board") &&
                    x.GetAttribute("data-board") == board &&
                    x.ChildElementCount > 0);

                if (elemThread == null)
                {
                    throw new Exception("Failed to find main container within provided thread's content.");
                }

                var result = new Thread
                {
                    Board = board,
                };

                Logger.Info("Parsing OP post.");
                
                var opPost = ParsePostInternal(elemThread.Children.First(), id);
                result.Posts.Add(opPost);
                
                Logger.Info("Parsing remaining {0} posts.", elemThread.Children.Length);
                
                foreach (var elemPostContainer in elemThread.Children.Skip(1))
                {
                    result.Posts.Add(ParsePostInternal(elemPostContainer, id, opPost.Id));
                    Logger.Info("Parsing post: {0}/{1}", result.Posts.Count, elemThread.Children.Length);
                }

                return result;
            }
        }
        
        private static Post ParsePostInternal(IParentNode postContainer, uint threadId, string opId = null)
        {
            var result = new Post();

            var elemPost = postContainer.Children.SingleOrDefault(x => x.ClassList.Contains("post"));
            
            var elemPostInfo = elemPost?.Children.SingleOrDefault(x => x.ClassName == "postInfo");

            var elemSubject = elemPostInfo?.Children
                .SingleOrDefault(x => 
                    x.LocalName == "span" && 
                    x.ClassName == "subject");

            result.Subject = elemSubject?.InnerHtml;

            var elemNameBlock = elemPostInfo?.Children
                .SingleOrDefault(x =>
                    x.LocalName == "span" &&
                    x.ClassName == "nameBlock");

            result.Email = elemNameBlock?.Children.FirstOrDefault()?.InnerHtml;
            
            var id = elemNameBlock?.Children.ElementAtOrDefault(1)?.InnerHtml;
            result.Id = id?.Substring(5, id.Length - 6);

            var elemDate = elemPostInfo?.Children
                .SingleOrDefault(x =>
                    x.LocalName == "span" &&
                    x.HasAttribute("data-raw"));

            var dateRawStr = elemDate?.GetAttribute("data-raw");

            if (elemDate != null && !string.IsNullOrEmpty(dateRawStr) && long.TryParse(dateRawStr, out var dateRaw))
            {
                result.Date = DateTimeOffset.FromUnixTimeSeconds(dateRaw).UtcDateTime;
            }

            var elemPostNum = elemPostInfo?.Children
                .SingleOrDefault(x =>
                    x.LocalName == "span" &&
                    x.ClassName == "postNum");

            var elemPostQuote = elemPostNum?.Children.ElementAtOrDefault(1);

            if (elemPostQuote != null && !string.IsNullOrEmpty(elemPostQuote.InnerHtml) &&
                uint.TryParse(elemPostQuote.InnerHtml, out var postNum))
            {
                result.Number = postNum;
            }

            var elemPostMessage = elemPost?.Children
                .SingleOrDefault(x =>
                    x.LocalName == "blockquote" &&
                    x.ClassList.Contains("postMessage"));

            if (elemPostMessage != null)
            {
                var quoteLinks = elemPostMessage.Children
                    .Where(x =>
                        x.LocalName == "a" &&
                        x.ClassName == "quotelink");

                foreach (var quoteLink in quoteLinks)
                {
                    var href = quoteLink.GetAttribute("href");
                    quoteLink.SetAttribute("href", href.Substring(href.LastIndexOf("#", StringComparison.Ordinal)));
                }

                var images = elemPostMessage.Children
                    .Where(x =>
                        x.LocalName == "img" &&
                        x.HasAttribute("src"));

                foreach (var img in images)
                {
                    img.SetAttribute("src", "../Resources/" + img.GetAttribute("src"));
                }
            }

            result.MessageBody = elemPostMessage?.InnerHtml;

            if (elemPostMessage != null)
            {
                var backup = elemPostMessage.InnerHtml;
                elemPostMessage.InnerHtml = elemPostMessage.InnerHtml
                    .Replace("<br>", "\n", StringComparison.InvariantCultureIgnoreCase)
                    .Trim('\n');
                result.MessageText = elemPostMessage.Text().Trim().Replace("  ", " ").Replace(" \n", "\n");
                elemPostMessage.InnerHtml = backup;
            }

            var elemFile = elemPost?.Children
                .SingleOrDefault(x => x.LocalName == "div" &&
                                      x.ClassName == "file");
            
            var elemFileLink = elemFile?
                .Children.FirstOrDefault()?
                .Children.FirstOrDefault()?
                .Children.SingleOrDefault(x =>
                    x.LocalName == "a" &&
                    x.HasAttribute("href"));
            
            if (elemFileLink != null)
            {
                if (elemFileLink.HasAttribute("download"))
                {
                    var fileName = elemFileLink.GetAttribute("download");
                    var fileUrl = SanitizeRelativeSrcUrl(elemFileLink.GetAttribute("href"), 5);

                    var elemFileThumbImg = elemFile.Children.ElementAtOrDefault(1)?
                        .Children.SingleOrDefault(x =>
                            x.LocalName == "img" &&
                            x.HasAttribute("src"));

                    var fileThumbUrl = SanitizeRelativeSrcUrl(elemFileThumbImg?.GetAttribute("src"), 5);

                    if (fileThumbUrl != null)
                    {
                        result.File = new Post.PostFile
                        {
                            FileName = fileName,
                            FileUrl = fileUrl,
                            FileThumbUrl = fileThumbUrl
                        };
                    }
                }
                else if (elemFileLink.InnerHtml.Contains("Spoiler"))
                {
                    var fileName = elemFileLink.GetAttribute("href");
                    fileName = fileName.Substring(fileName.LastIndexOf('/') + 1);
                    
                    var fileUrl = SanitizeRelativeSrcUrl(elemFileLink.GetAttribute("href"), 5);

                    result.File = new Post.PostFile
                    {
                        FileName = fileName,
                        FileUrl = fileUrl,
                        FileThumbUrl = "SPOILER"
                    };
                }
            }
            else
            {
                var elemEmbed = elemFile?.Children.ElementAtOrDefault(1)?
                    .Children.SingleOrDefault(x =>
                        x.LocalName == "iframe" &&
                        x.HasAttribute("src"));

                result.EmbedUrl = elemEmbed?.GetAttribute("src");
            }

            if (opId == null || result.Id == opId)
            {
                result.Name = "OP";
                result.NameColor = Color.Red;
            }
            else
            {
                var (name, color) = UidHighlighter.GetHighlightForPost(threadId, result.Id);
                result.Name = name;
                result.NameColor = color;
            }

            return result;
        }
    }
}