using FluentAssertions;
using GoogleSearchScrape.Abstractions.Interfaces;
using GoogleSearchScrape.Abstractions.Model;
using GoogleSearchScrape.Scrapers.Handlers;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace GoogleSearchScrape.Scrapers.Test.Handlers
{
    public class TimedScrapeRequestHandlerTests
    {
        private TimedScrapeRequestHandler _timedScrapeRequestHandler;

        public TimedScrapeRequestHandlerTests()
        {
            var fakeScrapeHandler = Substitute.For<IScrapeHandler<ScrapeRequest>>();
            fakeScrapeHandler
                .HandleAsync(Arg.Any<ScrapeRequest>(), Arg.Any<DateTimeOffset>())
                .Returns(Task.FromResult(new List<ScrapeResult> { new() }));
            _timedScrapeRequestHandler = new(fakeScrapeHandler);
        }

        [Fact]
        public async Task CanBlockEarlyRequests()
        {
            var now = DateTimeOffset.Now;
            var scrapeRequest = new TimedScrapeRequest
            {
                LastRequest = now,
                RepeatTime = now.Date.Add(TimeSpan.FromSeconds(60))
            };

            await AssertResultCount(scrapeRequest, 0);
        }

        [Fact]
        public async Task CanExecuteRequest()
        {
            var now = DateTimeOffset.Now;
            var scrapeRequest = new TimedScrapeRequest
            {
                LastRequest = now.AddSeconds(-61),
                RepeatTime = now.Date.Add(TimeSpan.FromSeconds(60))
            };
            await AssertResultCount(scrapeRequest, 1);
        }

        private async Task AssertResultCount(TimedScrapeRequest request, int count)
        {
            var result = await _timedScrapeRequestHandler.HandleAsync(request, DateTimeOffset.Now);
            result.Should().HaveCount(count);
        }
    }
}
