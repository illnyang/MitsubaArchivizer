using System;
using System.Drawing;

namespace MitsubaArchivizer.Models
{
    public class Post
    {
        public class PostFile
        {
            public string FileName { get; set; }
            public string FileUrl { get; set; }
            public string FileThumbUrl { get; set; }
        }
        
        public string Subject { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public Color NameColor { get; set; }
        public string Id { get; set; }
        public DateTime? Date { get; set; }
        public uint? Number { get; set; }
        public string MessageBody { get; set; }
        public string MessageText { get; set; }
        public PostFile File { get; set; }
        public string EmbedUrl { get; set; }
    }
}