using EnsureThat;
using FeedReader.Model;
using FeedReader.Persistence;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Redux;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace FeedReader.Redux
{
    internal interface IReducer
    {
        State? Execute(State state, IAction action);
    }

    internal class Reducer : IReducer
    {
        private IFeedRepository Feeds { get; }

        private IStoryRepository Stories { get; }

        public Reducer(IFeedRepository feeds, IStoryRepository stories)
        {
            Feeds = EnsureArg.IsNotNull(feeds);

            Stories = EnsureArg.IsNotNull(stories);
        }

        public State Execute(State state, IAction action)
        {
            var result = JsonConvert.DeserializeObject<State>(JsonConvert.SerializeObject(state));

            if (action is RefreshAction)
            {
                //Feeds.Save(new Model.Feed
                //{
                //    Id = Guid.Parse("46DC1314-AB16-47FD-848D-35CF2B567338"),
                //    Name = "Tech Crunch",
                //    Category = "Technology",
                //    Url = "https://techcrunch.com/feed",
                //    Tags = new List<string>(new[] { "news", "tech" })
                //});

                if (result != null)
                {
                    foreach (var feed in  Feeds.GetFeeds())
                    {
                        foreach (var story in CodeHollow.FeedReader.FeedReader.ReadAsync(feed.Url).GetAwaiter().GetResult().Items)
                        {
                            if (!Stories.Exists(Hash(story.Title)))
                            {
                                Stories.Save(new Story
                                {
                                    Id = Hash(story.Title),
                                    Title = story.Title,
                                    Description = HtmlEntity.DeEntitize(StripHtml(story.Description)),
                                    Image = "",
                                    Url = story.Link,
                                    Category = feed.Category,
                                    Loved = false,
                                    Viewed = false,
                                    Tags = feed.Tags
                                });
                            }
                        };
                    }
                }
            }

            if (action is LoadAction)
            {
                result.Stories.Clear();

                result?.Stories.AddRange(Stories.GetStoriesByCategory("Technology"));
            }

            if (action is HideAction)
            {
                var command = action as HideAction;

                if (command != null)
                {
                    var story = Stories.GetStoryById(command.Id);

                    if (story != null)
                    {
                        story.Viewed = true;

                        Stories.Save(story);
                    }

                    result.Stories.Clear();

                    result.Stories.AddRange(state.Stories.Where(i => i.Id != command.Id).ToArray());
                }
            }

            return result;
        }

        private string StripHtml(string input)
        {
            return Regex.Replace(input, "<[a-zA-Z/].*?>", String.Empty);
        }

        private string Hash(string text)
        {
            using (var hash = SHA256.Create())
            {
                byte[] data = hash.ComputeHash(Encoding.UTF8.GetBytes(text));

                var sb = new StringBuilder();

                for (int i = 0; i < data.Length; i++)
                {
                    sb.Append(data[i].ToString("X2"));
                }

                return sb.ToString();
            }
        }
    }
}