using GoogleSearchScrape.Abstractions.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

#nullable disable

namespace NetScape.Modules.DAL
{
    public partial class DatabaseContext : DbContext
    {
        private readonly ILoggerFactory _loggerFactory;

        public DbSet<TimedScrapeRequest> Requests { get; set; }
        public DbSet<ScrapeResult> Results { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options, ILoggerFactory loggerFactory = null)
            : base(options)
        {
            _loggerFactory = loggerFactory;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TimedScrapeRequest>()
                .HasMany(t => t.ScrapeResults)
                .WithOne(t => t.Request)
                .HasForeignKey(t => t.RequestId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
