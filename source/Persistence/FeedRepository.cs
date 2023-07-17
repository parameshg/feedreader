using EnsureThat;
using FeedReader.Model;
using LiteDB;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeedReader.Persistence
{
    public interface IFeedRepository
    {
        List<Feed> GetFeeds();

        void Save(Feed feed);
    }

    public class FeedRepository : IFeedRepository
    {
        private LiteDatabase Database { get; }

        public FeedRepository(LiteDatabase db)
        {
            Database = EnsureArg.IsNotNull(db);
        }

        public List<Feed> GetFeeds()
        {
            return Database.GetCollection<Feed>().FindAll().ToList();
        }

        public void Save(Feed feed)
        {
            var collection = Database.GetCollection<Feed>();

            if (collection != null)
            {
                if (collection.Exists(i => i.Id == feed.Id))
                {
                    Database.GetCollection<Feed>().Update(feed.Id, feed);
                }
                else
                {
                    Database.GetCollection<Feed>().Insert(feed);
                }
            }
        }
    }
}