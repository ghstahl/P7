using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hugo.Data.Json;
using P7.BlogStore.Core;
using P7.Core.Utils;
using P7.Store;

namespace P7.BlogStore.Hugo
{
    public class HugoStoreBase<T> where T: class, IDocumentBase, new()
    {
        protected IBiggyConfiguration _biggyConfiguration;
        private ISorter<T> _sorter;
        protected HugoStoreBase(IBiggyConfiguration biggyConfiguration, string collection, ISorter<T> sorter)
        {
            _biggyConfiguration = biggyConfiguration;
            _collection = collection;
            _sorter = sorter;
        }
        protected static object TheLock
        {
            get { return ConcurrencyLock.TheLock; }
        }

        protected JsonStore<T> _theStore = null;
        private string _collection;

        protected JsonStore<T> Store
        {
            get
            {
                if (_theStore == null)
                {
                    _theStore =
                        new JsonStore<T>(_biggyConfiguration.FolderStorage,
                            _biggyConfiguration.DatabaseName, _collection);
                    _theStore.Sorter = _sorter;
                }
                return _theStore;
            }
        }

        protected async Task GoAsync(Action action)
        {
            await Task.Run(action);
        }

        protected static async Task<TResult> GoAsync<TResult>(Func<TResult> func)
        {

            var task = Task.Run(func);
            var result = await task;
            return result;

        }
        public async Task InsertAsync(T doc)
        {
            var existing = await FetchAsync(doc.Id);
            await GoAsync(() =>
            {
                lock (TheLock)
                {

                    if (existing == null)
                    {
                        Store.Add(doc);
                    }
                    else
                    {
                        Store.Update(doc);
                    }
                }
            });
        }
        public async Task<T> FetchAsync(Guid id)
        {
            var result = await GoAsync(() =>
                {
                    T r2 = null;
                    lock (TheLock)
                    {
                        var collection = this.Store.TryLoadData();
                        var query = from item in collection
                            where id == item.Id
                            select item;
                        if (!query.Any())
                            return null;
                        var record = query.SingleOrDefault();

                        r2 = record;
                    }
                    return r2;
                }
            );
            return result;
        }
        public async Task UpdateAsync(T doc)
        {
            await InsertAsync(doc);
        }
        public async Task DeleteAsync(Guid id)
        {
            await GoAsync(() =>
            {
                lock (TheLock)
                {
                    var collection = this.Store.TryLoadData();
                    var query = from item in collection
                        where item.Id == id
                        select item;
                    foreach (var item in query)
                    {
                        this.Store.Delete(item);
                    }
                }
            });
        }
        public async Task<List<T>> RetrieveAsync()
        {

            var result = await GoAsync(() =>
                {
                    List<T> r2 = null;
                    lock (TheLock)
                    {
                        var collection = this.Store.TryLoadData();
                        if (collection.Any())
                        {
                            r2 = collection.ToList();
                        }
                    }
                    return r2 ?? new List<T>();
                }
            );
            return result;
        }
        public async Task<IPage<T>> PageAsync(
            int pageSize, 
            byte[] pagingState, 
            DateTime? timeStampLowerBoundary = null,   
            DateTime? timeStampUpperBoundary = null, 
            string[] categories = null, 
            string[] tags = null)
        {
            byte[] currentPagingState = pagingState;
            PagingState ps = pagingState.Deserialize();
            var records = await RetrieveAsync();

            var slice = records.Skip(ps.CurrentIndex).Take(pageSize).ToList();
            if (slice.Count < pageSize)
            {
                // we are at the end
                pagingState = null;
            }
            else
            {
                ps.CurrentIndex += pageSize;
                pagingState = ps.Serialize();
            }

            var page = new PageProxy<T>(currentPagingState, pagingState, slice);
            return page;
        }

    }
}