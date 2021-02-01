using ChartJs.Blazor;
using ChartJs.Blazor.Common;
using ChartJs.Blazor.Common.Axes;
using ChartJs.Blazor.Common.Enums;
using ChartJs.Blazor.Common.Time;
using ChartJs.Blazor.LineChart;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using NetScape.Modules.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Threading.Tasks;

namespace GoogleSearchScrape.Pages
{
    public class ChartData
    {
        public string XLabel
        {
            get;
            set;
        }
        public int Index
        {
            get;
            set;
        }
        public DateTime Created
        {
            get;
            set;
        }
        public string Url
        {
            get;
            set;
        }
    }

    public class IndexBase : ComponentBase, IDisposable
    {
        private static readonly Random _random = new Random();
        public List<ChartData> ScrapeResults
        {
            get;
            set;
        }

        protected LineConfig _config;

        [Inject]
        private IDbContextFactory<DatabaseContext> _dbContextFactory
        {
            get;
            set;
        }
        public Dictionary<string, List<ChartData>> StrategyResultsMap
        {
            get;
            set;
        }

        protected Chart ChartRef { get; set; }
        public Timer _timer { get; set; }
        private readonly List<(string Name, IDataset<TimePoint> DataSet)?> _dataSets = new();
        private void SetResults()
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var now = DateTimeOffset.UtcNow;
                var maxDate = now.AddDays(1);
                var minDate = now.AddDays(-1);
                var results = dbContext.Results
                    .AsQueryable()
                    .Include(t => t.Request)
                    .Where(result => result.Created <= maxDate && result.Created >= minDate)
                    .ToList();

                ScrapeResults = results.Select(t => new ChartData
                {
                    Created = t.Created.DateTime,
                    XLabel = $"Strategy: {t.Request.StrategyName} Url: {t.Url} Keyword: {t.Request.SearchTerm}",
                    Index = t.Index,
                    Url = t.Url
                }).ToList();
                StrategyResultsMap = ScrapeResults
                    .Select(t => t.XLabel)
                    .Distinct()
                    .ToDictionary(url => url,
                        url => ScrapeResults.Where(t => url.Equals(t.XLabel))
                        .OrderBy(result => result.Created)
                        .ToList());
            }
        }

        public void StartRefereshTimer()
        {
            _timer = new Timer(5000);
            _timer.Elapsed += RefreshUI;
            _timer.Start();
        }

        private void RefreshUI(object sender, ElapsedEventArgs e)
        {
            SetResults();
            SetChartDataset(false);
            InvokeAsync(ChartRef.Update);
        }

        protected override async Task OnInitializedAsync()
        {
            SetResults();
            CreateChartConfig();
            SetChartDataset(true);
            await base.OnInitializedAsync();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                StartRefereshTimer();
            }
            base.OnAfterRender(firstRender);
        }

        private void SetChartDataset(bool createNew)
        {
            var keys = StrategyResultsMap.Keys.ToList();
            for (int i = 0; i < StrategyResultsMap.Count; i++)
            {
                var key = keys[i];
                IDataset<TimePoint> dataSet = !createNew ?
                    (_dataSets.FirstOrDefault(t => t?.Name == key)?.DataSet) :
                    NewLineDataset(key);
                var newDataSet = createNew || dataSet == null;
                if (!createNew && newDataSet)
                {
                    dataSet = NewLineDataset(key);
                }
                foreach (var month in StrategyResultsMap[key])
                {
                    var timePoint = new TimePoint(month.Created, month.Index);

                    if (!dataSet.Contains(timePoint))
                    {
                        dataSet.Add(timePoint);
                    }
                }

                if (newDataSet)
                {
                    var dataSetToAdd = (key, dataSet);
                    _dataSets.Add(dataSetToAdd);
                    _config.Data.Datasets.Add(dataSet);
                }
            }
        }

        private IDataset<TimePoint> NewLineDataset(string key)
        {
            return new LineDataset<TimePoint>()
            {
                Label = key,
                Fill = FillingMode.Disabled,
                SteppedLine = SteppedLine.True,
                BorderColor = string.Format("#{0:x}", _random.Next(0x1000000) & 0x7F7F7F) //Random dark color
            };
        }

        private void CreateChartConfig()
        {
            _config = new LineConfig
            {
                Options = new LineOptions
                {
                    Responsive = true,
                    Title = new OptionsTitle
                    {
                        Display = true,
                        Text = "URL Ranking overtime (The lower the better)"
                    },
                    Tooltips = new Tooltips
                    {
                        Mode = InteractionMode.Nearest,
                        Intersect = true
                    },
                    Hover = new Hover
                    {
                        Mode = InteractionMode.Nearest,
                        Intersect = true
                    },
                    Scales = new Scales
                    {
                        XAxes = new List<CartesianAxis> {
                            new TimeAxis {
                                ScaleLabel = new ScaleLabel {
                                    LabelString = "Date"
                                },
                                Ticks = new ChartJs.Blazor.Common.Axes.Ticks.TimeTicks {
                                    AutoSkip = true
                                },
                                Time = new TimeOptions {
                                    TooltipFormat = "ll dd MMM HH:mm"
                                },
                            }
                        },
                        YAxes = new List<CartesianAxis> {
                            new LinearCartesianAxis {
                                ScaleLabel = new ScaleLabel {
                                    LabelString = "Value"
                                }
                            }
                        }
                    }
                }
            };
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}