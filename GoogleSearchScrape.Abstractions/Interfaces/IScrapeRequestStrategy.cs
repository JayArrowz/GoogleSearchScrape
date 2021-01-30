using GoogleSearchScrape.Abstractions.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoogleSearchScrape.Abstractions.Interfaces
{
    public interface IScrapeRequestStrategy
    {
        /// <summary>
        /// Gets the Name
        /// </summary>
        /// <value>
        /// The name of the strategy.
        /// </value>
        string Name { get; }

        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <value>
        /// The URL to scrape
        /// </value>
        string Url { get; }

        /// <summary>
        /// Gets a list of scrape results for a <see cref="ScrapeRequest"/>
        /// </summary>
        /// <param name="request">The scrape request.</param>
        /// <returns>A list of <see cref="ScrapeResult"/></returns>
        Task<List<ScrapeResult>> GetAsync(ScrapeRequest request);
    }
}
