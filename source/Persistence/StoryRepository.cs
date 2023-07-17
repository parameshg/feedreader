using EnsureThat;
using FeedReader.Model;
using LiteDB;
using System.Collections.Generic;
using System.Linq;

namespace FeedReader.Persistence
{
    internal interface IStoryRepository
    {
        bool Exists(string? id);

        Story GetStoryById(string? id);

        List<Story> GetStoriesByCategory(string? category, bool viewed = false);

        void Save(Story story);
    }

    internal class StoryRepository : IStoryRepository
    {
        private LiteDatabase Database { get; }

        public StoryRepository(LiteDatabase db)
        {
            Database = EnsureArg.IsNotNull(db);
        }

        public bool Exists(string? id)
        {
            return Database.GetCollection<Story>().Exists(i => i.Id == id);
        }

        public Story GetStoryById(string? id)
        {
            return Database.GetCollection<Story>().FindOne(i => i.Id == id);
        }

        public List<Story> GetStoriesByCategory(string? category, bool viewed = false)
        {
            return Database.GetCollection<Story>().Find(i => i.Viewed == viewed).ToList();
        }

        public void Save(Story story)
        {
            var collection = Database.GetCollection<Story>();

            if (collection != null)
            {
                if (collection.Exists(i => i.Id == story.Id))
                {
                    Database.GetCollection<Story>().Update(story.Id, story);
                }
                else
                {
                    Database.GetCollection<Story>().Insert(story);
                }
            }
        }
    }
}