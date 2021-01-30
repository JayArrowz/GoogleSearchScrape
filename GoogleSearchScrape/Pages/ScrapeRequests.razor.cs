using GoogleSearchScrape.Abstractions.Interfaces;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleSearchScrape.Pages
{
    public class ScrapeRequestsBase : ComponentBase
    {
        [Inject]
        private IScrapeRequestStrategy[] _strategies { get; set; }

        protected string[] StrategyNames { get; set; }

        protected override void OnInitialized()
        {
            StrategyNames = _strategies.Select(t => t.Name).ToArray();
            base.OnInitialized();
        }
    }
}
