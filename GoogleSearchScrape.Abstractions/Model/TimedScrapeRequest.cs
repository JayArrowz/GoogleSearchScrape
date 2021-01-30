using GoogleSearchScrape.Abstractions.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoogleSearchScrape.Abstractions.Model
{
    /// <summary>
    /// Represents a timed scrape request which will be repeated every <see cref="RepeatTime"/>
    /// </summary>
    /// <seealso cref="GoogleSearchScrape.Abstractions.Model.ScrapeRequest" />
    /// <seealso cref="GoogleSearchScrape.Abstractions.Interfaces.IDao{System.Int32}" />
    public class TimedScrapeRequest : ScrapeRequest, IDao<int>
    {
        /// <summary>
        /// Gets or sets the repeat time.
        /// </summary>
        /// <value>
        /// The repeat time.
        /// </value>
        public DateTimeOffset RepeatTime { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }
}
