using GoogleSearchScrape.Scrapers.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GoogleSearchScrape.Scrapers.Extensions
{
    public static class ScraperExtensions
    {
        public static IServiceCollection AddScraperService(this IServiceCollection collection)
        {
            return collection.AddHostedService<ScraperService>();
        }
    }
}
