using Autofac;
using GoogleSearchScrape.Abstractions.Interfaces;
using GoogleSearchScrape.Abstractions.Model;
using GoogleSearchScrape.Scrapers.Handlers;
using GoogleSearchScrape.Scrapers.Strategies;
using PuppeteerSharp;

namespace GoogleSearchScrape.Scrapers
{
    public class ScraperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //Download browser required to scrape
            //DOM generated from JS will be available
            var result = new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision).Result;

            builder.RegisterType<GoogleSearchScraperStrategy>().As<IScrapeRequestStrategy>();
            builder.RegisterType<BingSearchScraperStrategy>().As<IScrapeRequestStrategy>();
            builder.RegisterType<TimedScrapeRequestHandler>().As<IScrapeHandler<TimedScrapeRequest>>();
            builder.RegisterType<ScrapeRequestHandler>().As<IScrapeHandler<ScrapeRequest>>();
            base.Load(builder);
        }
    }
}
