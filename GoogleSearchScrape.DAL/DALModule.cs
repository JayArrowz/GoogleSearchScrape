using Autofac;
using GoogleSearchScrape.Abstractions.Model;
using GoogleSearchScrape.DAL.DataAdapter;

namespace GoogleSearchScrape.DAL
{
    public class DALModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(BaseDataAdapter<,>));
            builder.RegisterType<ScrapeResultDataAdapter>().As<BaseDataAdapter<ScrapeResult, int>>();
            base.Load(builder);
        }
    }
}
