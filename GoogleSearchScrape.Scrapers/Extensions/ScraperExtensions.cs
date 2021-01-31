using GoogleSearchScrape.Scrapers.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GoogleSearchScrape.Scrapers.Extensions
{
    public static class ScraperExtensions
    {
        /// <summary>
        /// Adds the <see cref="ScraperService"/> to <see cref="IServiceCollection"/>
        /// </summary>
        /// <param name="collection">The service collection.</param>
        /// <returns>The <see cref="IServiceCollection"/> with the <see cref="ScraperService"/></returns>
        public static IServiceCollection AddScraperService(this IServiceCollection collection)
        {
            return collection.AddHostedService<ScraperService>();
        }
    }
}
