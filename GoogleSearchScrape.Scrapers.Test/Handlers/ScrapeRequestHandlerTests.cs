using FluentAssertions;
using GoogleSearchScrape.Abstractions.Interfaces;
using GoogleSearchScrape.Abstractions.Model;
using GoogleSearchScrape.Scrapers.Handlers;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GoogleSearchScrape.Scrapers.Test.Handlers
{
    public class ScrapeRequestHandlerTests
    {
        private readonly ScrapeRequestHandler _scrapeRequestHandler;
        private const string FakeStrategyName = "fake";

        public readonly List<ScrapeResult> _fakeData = new()
        {
            new ScrapeResult
            {
                Url = "google.com"
            },
            new ScrapeResult
            {
                Url = "google.com"
            },
            new ScrapeResult
            {
                Url = "wales.com"
            }
        };

        public ScrapeRequestHandlerTests()
        {
            var fakeStrategy = Substitute.For<IScrapeRequestStrategy>();
            fakeStrategy.Name.Returns(FakeStrategyName);
            fakeStrategy.GetAsync(Arg.Any<ScrapeRequest>()).Returns(Task.FromResult(_fakeData));
            var fakeStrategies = new IScrapeRequestStrategy[] { fakeStrategy };
            _scrapeRequestHandler = new ScrapeRequestHandler(fakeStrategies);
        }

        [Fact]
        public async Task CanFilterTargetUrls()
        {
            var result = await GetValidResults("google.com");
            result.Item2.Count.Should().Be(2);
        }

        [Fact]
        public async Task CanAttachDatesOnRequest()
        {
            var now = DateTimeOffset.Now;
            var result = await GetValidResults("google.com");
            result.Item1.LastRequest.Should().NotBeNull().And.BeOnOrAfter(now);
        }

        private async Task<(ScrapeRequest, List<ScrapeResult>)> GetValidResults(string url)
        {
            var request = new ScrapeRequest { TargetUrl = url, StrategyName = FakeStrategyName };
            var result = await _scrapeRequestHandler.HandleAsync(request, DateTimeOffset.UtcNow);
            return (request, result);
        }
    }
}
