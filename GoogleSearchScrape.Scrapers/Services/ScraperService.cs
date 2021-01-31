using GoogleSearchScrape.Abstractions.Interfaces;
using GoogleSearchScrape.Abstractions.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using NetScape.Modules.DAL;
using Serilog;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleSearchScrape.Scrapers.Services
{
    /// <summary>
    /// The scraper service runs all the requests in the background every 30 seconds
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.Hosting.BackgroundService" />
    public class ScraperService : BackgroundService
    {
        private readonly IScrapeHandler<TimedScrapeRequest> _scrapeHandler;
        private readonly IDbContextFactory<DatabaseContext> _databaseContextFactory;

        public ScraperService(IScrapeHandler<TimedScrapeRequest> scrapeHandler,
            IDbContextFactory<DatabaseContext> databaseContextFactory)
        {
            _scrapeHandler = scrapeHandler;
            _databaseContextFactory = databaseContextFactory;
        }

        /// <summary>
        /// Executes the scraper service task every 30 seconds. 
        /// This service esentially executes <see cref="IScrapeHandler{TimedScrapeRequest}"/> and persists
        /// any filtered results to the database
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var dbUpdated = false;
                    var now = DateTimeOffset.UtcNow;
                    using (var dbContext = _databaseContextFactory.CreateDbContext())
                    {
                        var requests = await dbContext.Requests.ToListAsync(cancellationToken);

                        var scraperTasks = requests.Select(request => _scrapeHandler.HandleAsync(request, now)).ToList();
                        await Task.WhenAll(scraperTasks);

                        foreach (var scraperTask in scraperTasks)
                        {
                            if (scraperTask.IsCompletedSuccessfully)
                            {
                                dbContext.AddRange(scraperTask.Result);
                                dbUpdated = true;
                            }
                        }

                        if (dbUpdated)
                        {
                            dbContext.UpdateRange(requests);
                            await dbContext.SaveChangesAsync(cancellationToken);
                        }
                    }
                    await Task.Delay(TimeSpan.FromSeconds(30), cancellationToken);
                } catch(Exception e)
                {
                    Log.Logger.Error(e, nameof(ExecuteAsync));
                }
            }
        }
    }
}
