using GoogleSearchScrape.Abstractions.Model;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GoogleSearchScrape.Scrapers.Strategies
{
    public class GoogleSearchScraperStrategy : HttpScraperStrategy
    {
        /// <summary>
        /// XPath for each google search div
        /// </summary>
        private const string GoogleSearchXPath = "//div[@class='g']";

        /// <summary>
        /// XPath for each title with href being the url
        /// </summary>
        private readonly string GoogleSearchTitlePath = $"{GoogleSearchXPath}/div/div/a";

        private readonly string FirstResultPath = $"{GoogleSearchXPath}/div/div/div/a";
        private readonly string FirstResultPathTitle = $"{GoogleSearchXPath}/div/div/div/*/h3";

        /// <summary>
        /// Google search URL with {0} being the <see cref="ScrapeRequest.SearchTerm"/> 
        /// and {1} being <see cref="ScrapeRequest.MaxResults"/> 
        /// </summary>
        public override string Url => "https://www.google.co.uk/search?q={0}&num={1}";

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public override string Name => "Google";

        /// <summary>
        /// Formats the URL depending on the <see cref="ScrapeRequest" /> passed.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// Formatted url with args ready for HttpClient
        /// </returns>
        protected override string FormatUrl(ScrapeRequest request) => string.Format(Url, request.SearchTerm, request.MaxResults);

        /// <summary>
        /// Gets the Collection of <see cref="ScrapeResult" /> from a <see cref="HtmlDocument" />
        /// </summary>
        /// <param name="request">The scrape request</param>
        /// <param name="doc">The html doc to convert</param>
        /// <returns></returns>
        protected override List<ScrapeResult> Get(ScrapeRequest request, HtmlDocument doc)
        {
            ScrapeResult firstScrapeResult = null;

            var firstResult = doc.DocumentNode.SelectSingleNode(FirstResultPath);
            if (firstResult != null)
            {
                var firstResultText = doc.DocumentNode.SelectSingleNode(FirstResultPathTitle)?.InnerText;
                firstScrapeResult = new ScrapeResult
                {
                    Title = firstResultText,
                    Url = firstResult.GetAttributeValue<string>("href", string.Empty),
                    Created = DateTimeOffset.UtcNow,
                    Index = 1
                };
            } else
            {
                Serilog.Log.Logger.Warning("First result for {@request} not found", request);
            }

            var htmlBlock = doc.DocumentNode.SelectNodes(GoogleSearchTitlePath);
            var links = htmlBlock
                .Select((aElement, index) => new ScrapeResult
                {
                    Title = aElement.InnerText,
                    Url = aElement.GetAttributeValue<string>("href", string.Empty),
                    Created = DateTimeOffset.UtcNow,
                    Index = firstScrapeResult == null ? index + 1 : index + 2
                });
            var resultList = links.ToList();

            if (firstScrapeResult != null)
            {
                resultList.Add(firstScrapeResult);
            }
            return resultList;
        }
    }
}
