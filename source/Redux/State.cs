using FeedReader.Model;
using System.Collections.Generic;

namespace FeedReader.Redux
{
    public record State
    {
        public List<Feed> Feeds { get; set; } = new List<Feed>();

        public List<Story> Stories { get; set; } = new List<Story>();
    }
}