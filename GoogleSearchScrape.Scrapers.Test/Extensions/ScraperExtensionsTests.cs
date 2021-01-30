using FluentAssertions;
using GoogleSearchScrape.Scrapers.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GoogleSearchScrape.Scrapers.Test.Extensions
{
    public class ScraperExtensionsTests
    {
        [Fact]
        public void CanAddScraperService()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScraperService();
            serviceCollection.Count.Should().Be(1);
        }
    }
}
