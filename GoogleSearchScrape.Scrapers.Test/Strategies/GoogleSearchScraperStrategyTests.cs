using FluentAssertions;
using GoogleSearchScrape.Abstractions.Model;
using PuppeteerSharp;
using System.Threading.Tasks;
using Xunit;

namespace GoogleSearchScrape.Scrapers.Strategies.Test
{
    /// <summary>
    /// This test uses the itnernet, might be best not to run it on CI
    /// </summary>
    public class GoogleSearchScraperStrategyTests
    {
        static GoogleSearchScraperStrategyTests()
        {
            //Download browser required to scrape
            //DOM generated from JS will be available
            var result = new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision).Result;
        }

        private readonly GoogleSearchScraperStrategy _strategy;
        public GoogleSearchScraperStrategyTests()
        {
            _strategy = new GoogleSearchScraperStrategy();
        }

        [Fact]
        public async Task CanScrapeGoogleSearchResults()
        {
            var request = new ScrapeRequest
            {
                TargetUrl = "google.com",
                SearchTerm = "Google",
                MaxResults = 100,
            };

            var result = await _strategy.GetAsync(request);
            result.Should().NotBeNull().And.NotBeEmpty();
        }

        [Fact]
        public async Task StrategyCanReturnEmptyListWhenNoResults()
        {
            var emptyResultRequest = new ScrapeRequest
            {
                TargetUrl = "?",
                SearchTerm = "asfu09w3uirt0912urt019ur09iwq",
                MaxResults = 1
            };

            var result = await _strategy.GetAsync(emptyResultRequest);
            result.Should().NotBeNull().And.BeEmpty();
        }
    }
}
