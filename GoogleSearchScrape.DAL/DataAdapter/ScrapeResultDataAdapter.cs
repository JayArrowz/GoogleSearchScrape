using GoogleSearchScrape.Abstractions.Model;
using Microsoft.EntityFrameworkCore;
using NetScape.Modules.DAL;
using System.Linq;

namespace GoogleSearchScrape.DAL.DataAdapter
{
    public class ScrapeResultDataAdapter : BaseDataAdapter<ScrapeResult, int>
    {
        public ScrapeResultDataAdapter(IDbContextFactory<DatabaseContext> contextFactory) : base(contextFactory)
        {
        }

        protected override IQueryable<ScrapeResult> FilterRead(IQueryable<ScrapeResult> query)
        {
            return query.OrderByDescending(t => t.Created);
        }
    }
}
