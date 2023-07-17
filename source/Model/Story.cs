using System.Collections.Generic;

namespace FeedReader.Model
{
    public class Story
    {
        public string? Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? Url { get; set; }

        public string? Image { get; set; }

        public bool Loved { get; set; }

        public bool Viewed { get; set; }

        public string? Category { get; set; }

        public List<string> Tags { get; set; } = new List<string>();
    }
}