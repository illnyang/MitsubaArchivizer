using System.Collections.Generic;

namespace MitsubaArchivizer.Models
{
    public class Thread
    {
        public string Board { get; set; }
        public IList<Post> Posts { get; set; } = new List<Post>();
    }
}