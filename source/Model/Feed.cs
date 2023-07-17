using System;
using System.Collections.Generic;

namespace FeedReader.Model
{
    public class Feed
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Url { get; set; }

        public string? Category { get; set; }

        public List<string> Tags { get; set; } = new List<string>();
    }
}