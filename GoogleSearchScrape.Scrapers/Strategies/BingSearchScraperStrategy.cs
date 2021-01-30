﻿using GoogleSearchScrape.Abstractions.Model;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GoogleSearchScrape.Scrapers.Strategies
{
    public class BingSearchScraperStrategy : HttpScraperStrategy
    {
        /// <summary>
        /// XPath for each title with href being the url
        /// </summary>
        private const string BingSearchItemXPath = "//html/body/div/main/ol/li[@class='b_algo']/h2/a";

        /// <summary>
        /// Google search URL with {0} being the <see cref="ScrapeRequest.SearchTerm"/> 
        /// and {1} being <see cref="ScrapeRequest.MaxResults"/> 
        /// </summary>
        public override string Url => "https://www.bing.com/search?q={0}&count={1}";

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public override string Name => "Bing";

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
            var htmlBlock = doc.DocumentNode.SelectNodes(BingSearchItemXPath);
            var links = htmlBlock
                .Select((aElement, index) => new ScrapeResult
            {
                Title = aElement.InnerText,
                Url = aElement.GetAttributeValue<string>("href", string.Empty),
                Created = DateTimeOffset.UtcNow,
                Index = index + 1
            });
            return links.ToList();
        }
    }
}
