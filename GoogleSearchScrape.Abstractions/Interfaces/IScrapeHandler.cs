using GoogleSearchScrape.Abstractions.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoogleSearchScrape.Abstractions.Interfaces
{
    /// <summary>
    /// Handles the initial <see cref="ScrapeRequest"/> before the strategy is executed
    /// </summary>
    /// <typeparam name="T">The type of request</typeparam>
    public interface IScrapeHandler<T> where T : ScrapeRequest
    {
        /// <summary>
        /// Handles the specified scrape request.
        /// </summary>
        /// <param name="scrapeRequest">The scrape request.</param>
        /// <param name="requestTime">The time of the request</param>
        /// <returns>A list of <see cref="ScrapeResult"/>s returned by <see cref="IScrapeRequestStrategy"/></returns>
        Task<List<ScrapeResult>> HandleAsync(T scrapeRequest, DateTimeOffset requestTime);
    }
}
