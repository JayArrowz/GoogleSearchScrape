using Dawn;
using GoogleSearchScrape.Abstractions.Interfaces;
using GoogleSearchScrape.Abstractions.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSearchScrape.Scrapers.Handlers
{
    /// <seealso cref="GoogleSearchScrape.Abstractions.Interfaces.IScrapeHandler{GoogleSearchScrape.Abstractions.Model.ScrapeRequest}" />
    public class ScrapeRequestHandler : IScrapeHandler<ScrapeRequest>
    {
        private readonly Dictionary<string, IScrapeRequestStrategy> _strategies;

        public ScrapeRequestHandler(IScrapeRequestStrategy[] strategies)
        {
            Guard.Argument(strategies).NotNull().NotEmpty();
            _strategies = strategies.ToDictionary(t => t.Name.ToUpper(), t => t);
        }

        /// <summary>
        /// Handles the specified scrape request.
        /// </summary>
        /// <param name="scrapeRequest">The scrape request.</param>
        /// <param name="requestTime">The time of the request</param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException">Strategy not found</exception>
        public async Task<List<ScrapeResult>> HandleAsync(ScrapeRequest scrapeRequest, DateTimeOffset requestTime)
        {
            var strategy = _strategies[scrapeRequest.StrategyName.ToUpper()];
            var scrapeResultList = await strategy.GetAsync(scrapeRequest);
            scrapeRequest.LastRequest = requestTime;
            return Filter(scrapeRequest, scrapeResultList);
        }

        /// <summary>
        /// Filters the specified <paramref name="lists"/> with information from <see cref="ScrapeRequest.TargetUrl"/>
        /// </summary>
        /// <param name="request">The scrape request.</param>
        /// <param name="lists">A collection of <see cref="ScrapeResult"/> created from the ScapeRequest.</param>
        /// <returns>Filtered results</returns>
        protected virtual List<ScrapeResult> Filter(ScrapeRequest request, List<ScrapeResult> lists)
        {
            return lists
                .Where(t => t.Url.Contains(request.TargetUrl, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }
    }
}
