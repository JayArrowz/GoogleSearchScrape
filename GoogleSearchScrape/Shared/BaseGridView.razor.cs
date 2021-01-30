using GoogleSearchScrape.Abstractions.Interfaces;
using GoogleSearchScrape.DAL.DataAdapter;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using NetScape.Modules.DAL;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.Grids;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleSearchScrape.Shared
{
    public partial class BaseGridView<TItem, TKey> where TItem : class, IDao<TKey>
    {
    }

    public class BaseGridViewBase<TItem, TKey> : ComponentBase where TItem : class, IDao<TKey>
    {
        [Parameter]
        public string Id { get; set; }

        protected const string ExcelExportToolbarItem = "ExcelExport";
        protected const string RefreshToolbarItem = "Refresh";
        protected SfGrid<TItem> SfGridRef { get; set; }

        [Parameter]
        public RenderFragment GridColumns { get; set; }

        [Inject]
        protected IDbContextFactory<DatabaseContext> DbContextFactory { get; set; }

        protected SfDataManager DataManager { get; set; }

        protected Type DataAdapterType => typeof(BaseDataAdapter<TItem, TKey>);

        protected virtual async Task OnActionBegin(ActionEventArgs<TItem> args)
        {
            if (args.RequestType == Syncfusion.Blazor.Grids.Action.Delete)
            {
                await OnBatchDelete(null);
            }
        }

        private async Task OnBatchDelete(BeforeBatchDeleteArgs<TItem> args)
        {
            try
            {
                using (var dbContext = DbContextFactory.CreateDbContext())
                {
                    var deletedRecords = await SfGridRef.GetSelectedRecords();
                    var recordIds = deletedRecords.Select(t => t.Id);
                    dbContext.RemoveRange(dbContext.Set<TItem>().Where(t => recordIds.Contains(t.Id)));
                    await dbContext.SaveChangesAsync();
                    await SfGridRef.ClearSelection();
                }
            }
            catch (Exception e)
            {
                Serilog.Log.Logger.Error(e, nameof(OnBatchDelete));
            }
        }

        public virtual Task ToolbarClickHandler(Syncfusion.Blazor.Navigations.ClickEventArgs args)
        {
            if(args.Item.Id.EndsWith(ExcelExportToolbarItem, StringComparison.InvariantCultureIgnoreCase))
            {
                SfGridRef?.ExcelExport();
            } else if(args.Item.Id.EndsWith(RefreshToolbarItem, StringComparison.InvariantCultureIgnoreCase))
            {
                SfGridRef?.Refresh();
            }
            return Task.CompletedTask;
        }
    }
}
