using GoogleSearchScrape.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore;
using NetScape.Modules.DAL;
using Serilog;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GoogleSearchScrape.DAL.DataAdapter
{
    /// <summary>
    /// The base data adapter handles DAL operations for the syncfusion controls
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity <see cref="IDao{TKey}"/>.</typeparam>
    /// <typeparam name="TKey">The type of the primary key.</typeparam>
    /// <seealso cref="Syncfusion.Blazor.DataAdaptor" />
    public class BaseDataAdapter<TEntity, TKey> : DataAdaptor where TEntity : class, IDao<TKey>
    {
        protected IDbContextFactory<DatabaseContext> DatabaseContextFactory { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDataAdapter{TEntity, TKey}"/> class.
        /// </summary>
        /// <param name="contextFactory">The context factory.</param>
        public BaseDataAdapter(IDbContextFactory<DatabaseContext> contextFactory)
        {
            DatabaseContextFactory = contextFactory;
        }

        /// <summary>
        /// Any filter/groupby operations to the Queryable should occur here, this method
        /// is executed before the query is sent to the database
        /// </summary>
        /// <param name="query">The current query</param>
        /// <returns>The new query</returns>
        protected virtual IQueryable<TEntity> FilterRead(IQueryable<TEntity> query)
        {
            return query;
        }

        /// <summary>
        /// Returns a queryable for <typeparamref name="TEntity"/> within the <see cref="DatabaseContext"/>
        /// </summary>
        /// <param name="databaseContext">The database context.</param>
        /// <returns>Queryable for <typeparamref name="TEntity"/></returns>
        public IQueryable<TEntity> AsQueryable(DatabaseContext databaseContext)
        {
            return FilterRead(databaseContext.Set<TEntity>().AsQueryable());
        }

        /// <summary>
        /// Gets the entity from the database using the specifed primary <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The primary key.</param>
        /// <returns>The <typeparamref name="TEntity"/> from the database</returns>
        public TEntity Get(TKey key)
        {
            using (var databaseContext = DatabaseContextFactory.CreateDbContext())
            {
                return AsQueryable(databaseContext)
                    .FirstOrDefault(t => t.Id.Equals(key));
            }
        }

        /// <summary>
        /// Gets <typeparamref name="TEntity"/>
        /// </summary>
        /// <param name="key">The primary key.</param>
        /// <returns>The <typeparamref name="TEntity"/> from the database</returns>
        public async Task<TEntity> GetAsync(TKey key)
        {
            using (var databaseContext = DatabaseContextFactory.CreateDbContext())
            {
                var entity = await AsQueryable(databaseContext)
                    .FirstOrDefaultAsync(t => t.Id.Equals(key));
                return entity;
            }
        }

        /// <summary>
        /// The list filter will modify the returning list from the database after the query is executed
        /// </summary>
        /// <param name="list">The list returned from the database.</param>
        /// <returns>The new list</returns>
        protected virtual List<TEntity> ListFilter(List<TEntity> list)
        {
            return list;
        }
        
        public override object Read(DataManagerRequest dm, string key = null)
        {
            using (var databaseContext = DatabaseContextFactory.CreateDbContext())
            {
                var dataSource = FilterRead(databaseContext.Set<TEntity>().AsQueryable());


                if (dm.Search != null && dm.Search.Count > 0)
                {
                    // Searching
                    dataSource = DataOperations.PerformSearching(dataSource, dm.Search);
                }

                if (dm.Sorted != null && dm.Sorted.Count > 0)
                {
                    // Sorting
                    dataSource = DataOperations.PerformSorting(dataSource, dm.Sorted);
                }

                if (dm.Where != null && dm.Where.Count > 0) //Filtering
                {
                    try
                    {
                        dataSource = DataOperations.PerformFiltering(dataSource, dm.Where, dm.Where[0].Operator);
                    }
                    catch (Exception e)
                    {
                        Log.Logger.Error(nameof(Read), e);
                    }
                }

                var countBeforePaging = dataSource.Count();
                if (dm.Skip != 0)
                {
                    //Paging
                    dataSource = DataOperations.PerformSkip(dataSource, dm.Skip);
                }

                if (dm.Take != 0)
                {
                    dataSource = DataOperations.PerformTake(dataSource, dm.Take);
                }

                var toList = dataSource.ToList();
                toList = ListFilter(toList);

                return dm.RequiresCounts
                    ? new DataResult { Result = toList, Count = countBeforePaging }
                    : (object)toList;
            }
        }

        public override async Task<object> RemoveAsync(DataManager dataManager, object data, string keyField, string key)
        {
            using (var databaseContext = DatabaseContextFactory.CreateDbContext())
            {
                var dataSet = databaseContext.Set<TEntity>();
                var dao = dataSet.FirstOrDefault(t => t.Id.Equals((TKey)data));
                if (dao != null)
                {
                    dataSet.Remove(dao);
                    await databaseContext.SaveChangesAsync();
                }
                return data;
            }
        }

        public override async Task<object> InsertAsync(DataManager dataManager, object data, string key)
        {
            using (var databaseContext = DatabaseContextFactory.CreateDbContext())
            {
                var dataSet = databaseContext.Set<TEntity>();
                dataSet.Attach((TEntity)data);
                dataSet.Add((TEntity)data);
                await databaseContext.SaveChangesAsync();
                return data;
            }
        }

        public override async Task<object> UpdateAsync(DataManager dataManager, object data, string keyField, string key)
        {
            using (var databaseContext = DatabaseContextFactory.CreateDbContext())
            {
                var dataSet = databaseContext.Set<TEntity>();
                var dao = data as TEntity;
                var dbData = dataSet.AsQueryable().FirstOrDefault(t => t.Id.Equals(dao.Id));
                databaseContext.Entry(dbData).CurrentValues.SetValues(dao);
                await databaseContext.SaveChangesAsync();
                return data;
            }
        }
    }
}
