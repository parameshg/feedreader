using FeedReader.Persistence;
using FeedReader.Redux;
using LiteDB;
using Microsoft.Extensions.DependencyInjection;
using Redux;
using System;
using System.Windows;

namespace FeedReader
{
    public partial class App : Application
    {
        private ServiceProvider Provider;

        public App()
        {
            var services = new ServiceCollection();

            services.AddSingleton(i => new LiteDatabase(@"FeedReader.db"));

            services.AddTransient<IFeedRepository, FeedRepository>();

            services.AddTransient<IStoryRepository, StoryRepository>();

            services.AddTransient<IReducer, Reducer>();

            services.AddSingleton<IStore<State>>(i => new Store<State>(i.GetRequiredService<IReducer>().Execute, new State()));

            services.AddSingleton<Home>();

            Provider = services.BuildServiceProvider();
        }

        private void OnStartup(object sender, StartupEventArgs args)
        {
            Provider.GetService<Home>()?.Show();
        }
    }
}