using Autofac;
using GoogleSearchScrape.DAL;
using GoogleSearchScrape.Scrapers;
using GoogleSearchScrape.Scrapers.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetScape.Modules.DAL;
using Serilog;
using Syncfusion.Blazor;
using WebEssentials.AspNetCore.Pwa;

namespace GoogleSearchScrape
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Config = configuration;
        }

        public IConfiguration Config { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Config, sectionName: "SeriLog").CreateLogger();
            Log.Logger = logger;

            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
            services.AddHttpClient();
            services.AddRazorPages();
            services.AddServerSideBlazor(t =>
            {
                t.DetailedErrors = true;
            }).AddHubOptions(o =>
            {
                o.MaximumReceiveMessageSize = 102400000;
            });
            services.AddDbContextFactory<DatabaseContext>(BuildDbOptions);

            services.AddProgressiveWebApp(new PwaOptions
            {
                Strategy = ServiceWorkerStrategy.NetworkFirst
            });
            services.AddSyncfusionBlazor();
            services.AddScraperService();
        }

        private void BuildDbOptions(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Config.GetConnectionString("ScraperDb"),
                 x => x.MigrationsAssembly(typeof(Startup)
                    .Assembly.GetName().Name));
        }

        internal static void RegisterAutofac(ContainerBuilder builder)
        {
            builder.RegisterModule(new DALModule());
            builder.RegisterModule(new ScraperModule());

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
