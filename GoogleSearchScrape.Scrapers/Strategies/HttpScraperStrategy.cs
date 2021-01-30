using GoogleSearchScrape.Abstractions.Interfaces;
using GoogleSearchScrape.Abstractions.Model;
using HtmlAgilityPack;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleSearchScrape.Scrapers.Strategies
{
    /// <summary>
    /// Represents a Http scraper strategy which exposes a <see cref="HtmlDocument"/>
    /// object from the request
    /// </summary>
    /// <seealso cref="IScrapeRequestStrategy" />
    public abstract class HttpScraperStrategy : IScrapeRequestStrategy
    {
        public abstract string Name { get; }

        public abstract string Url { get; }

        /// <summary>
        /// Gets a list of scrape results for a <see cref="T:GoogleSearchScrape.Abstractions.Model.ScrapeRequest" />
        /// </summary>
        /// <param name="request">The scrape request.</param>
        /// <returns>
        /// A list of <see cref="T:GoogleSearchScrape.Abstractions.Model.ScrapeResult" />
        /// </returns>
        public async Task<List<ScrapeResult>> GetAsync(ScrapeRequest request)
        {
            var url = FormatUrl(request);

            // Should really use a pool and limit creation of this web driver
            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true }))
            {
                try
                {
                    var page = await browser.NewPageAsync();
                    await page.GoToAsync(url);
                    var content = await page.GetContentAsync();
                    var doc = new HtmlDocument();
                    doc.LoadHtml(content);
                    List<ScrapeResult> results = null;
                    Serilog.Log.Logger.Information("[{name}] Requesting: {@request}", Name, request);
                    results = Get(request, doc);
                    return results;
                }
                catch (Exception e)
                {
                    Serilog.Log.Logger.Error(e, nameof(GetAsync));
                    return new List<ScrapeResult>();
                }
            }
        }

        /// <summary>
        /// Formats the URL depending on the <see cref="ScrapeRequest"/> passed.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Formatted url with args ready for HttpClient</returns>
        protected abstract string FormatUrl(ScrapeRequest request);

        /// <summary>
        /// Gets the Collection of <see cref="ScrapeResult"/> from a <see cref="HtmlDocument"/>
        /// </summary>
        /// <param name="request">The scrape request</param>
        /// <param name="doc">The html doc to convert</param>
        /// <returns></returns>
        protected abstract List<ScrapeResult> Get(ScrapeRequest request, HtmlDocument doc);
    }
}
