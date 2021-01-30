using GoogleSearchScrape.Abstractions.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoogleSearchScrape.Abstractions.Model
{
    public class ScrapeResult : IDao<int>
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the URL returned from the <see cref="ScrapeRequest"/>.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        [StringLength(2000)]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the position in the search engine 
        /// rank
        /// </summary>
        /// <value>
        /// The index/position of the result
        /// </value>
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the date this result was created.
        /// </summary>
        /// <value>
        /// The creation date
        /// </value>
        public DateTimeOffset Created { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// The scrape request ID.
        /// </summary>
        /// <value>
        /// The request identifier.
        /// </value>
        public int RequestId { get; set; }

        /// <summary>
        /// Gets or sets the request that created <c>this</c>.
        /// </summary>
        /// <value>
        /// The request.
        /// </value>
        public TimedScrapeRequest Request { get; set; }
    }
}
