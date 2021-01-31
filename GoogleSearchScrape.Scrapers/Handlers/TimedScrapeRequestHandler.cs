using GoogleSearchScrape.Abstractions.Interfaces;
using GoogleSearchScrape.Abstractions.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoogleSearchScrape.Scrapers.Handlers
{
    /// <summary>
    /// This handler handles the execution of the <see cref="TimedScrapeRequest"/>
    /// </summary>
    /// <seealso cref="GoogleSearchScrape.Abstractions.Interfaces.IScrapeHandler{GoogleSearchScrape.Abstractions.Model.TimedScrapeRequest}" />
    public class TimedScrapeRequestHandler : IScrapeHandler<TimedScrapeRequest>
    {
        private readonly IScrapeHandler<ScrapeRequest> _scrapeRequestHandler;

        public TimedScrapeRequestHandler(IScrapeHandler<ScrapeRequest> scrapeRequestHandler)
        {
            _scrapeRequestHandler = scrapeRequestHandler;
        }

        /// <summary>
        /// Handles execution of the <see cref="TimedScrapeRequest"/>
        /// This handler will block any calls which are made to soon
        /// </summary>
        /// <param name="scrapeRequest">The scrape request.</param>
        /// <param name="requestTime">The time of request.</param>
        /// <returns>A list of <see cref="ScrapeResult"/></returns>
        public async Task<List<ScrapeResult>> HandleAsync(TimedScrapeRequest scrapeRequest, DateTimeOffset requestTime)
        {
            var minutes = scrapeRequest.RepeatTime.TimeOfDay.Minutes;
            var hours = scrapeRequest.RepeatTime.TimeOfDay.Hours;

            if(minutes == 0 && hours == 0)
            {
                return new();
            }

            var timeToAdd = TimeSpan.FromHours(hours).Add(TimeSpan.FromMinutes(minutes));

            var passedElapsed = !scrapeRequest.LastRequest.HasValue
                || requestTime > scrapeRequest.LastRequest.Value.Add(timeToAdd);
            if(passedElapsed)
            {
                var results = await _scrapeRequestHandler.HandleAsync(scrapeRequest, requestTime);
                results.ForEach(result => result.RequestId = scrapeRequest.Id);
                return results;
            }

            return new();
        }
    }
}
