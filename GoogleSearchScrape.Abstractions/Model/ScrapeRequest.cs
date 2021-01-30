using GoogleSearchScrape.Abstractions.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GoogleSearchScrape.Abstractions.Model
{
    /// <summary>
    /// The scrape request is used to get the search result. The <see cref="Interfaces.IScrapeRequestStrategy"/>
    /// uses the <see cref="SearchTerm"/> and <see cref="MaxResults"/> to scrape a result. 
    /// </summary>
    public class ScrapeRequest
    {
        /// <summary>
        /// Gets or sets the name of the strategy.
        /// </summary>
        /// <value>
        /// The name of the strategy.
        /// </value>
        [Required]
        public string StrategyName { get; set; }

        /// <summary>
        /// Gets or sets the search term used to request search results
        /// </summary>
        /// <value>
        /// The search term.
        /// </value>
        [Required]
        public string SearchTerm { get; set; }

        /// <summary>
        /// Gets or sets the target URL to search for
        /// </summary>
        /// <value>
        /// The target URL.
        /// </value>
        [Required]
        public string TargetUrl { get; set; }

        /// <summary>
        /// Gets or sets the maximum possible <see cref="ScrapeResult"/>'s 
        /// returned by this request.
        /// </summary>
        /// <value>
        /// The maximum results.
        /// </value>
        [Range(1, 200)]
        public int MaxResults { get; set; }

        /// <summary>
        /// Gets or sets the last request.
        /// </summary>
        /// <value>
        /// The last request.
        /// </value>
        public DateTimeOffset? LastRequest { get; set; }

        /// <summary>
        /// Gets or sets the scrape results.
        /// </summary>
        /// <value>
        /// The scrape results.
        /// </value>
        public virtual ICollection<ScrapeResult> ScrapeResults { get; set; }
    }
}
