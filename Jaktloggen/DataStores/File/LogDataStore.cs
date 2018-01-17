using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Jaktloggen.Services;

namespace Jaktloggen.DataStores.File
{
    public class LogDataStore : IDataStore<Log>
    {
        List<Log> items = new List<Log>();
        private static string FILENAME = "logger.json";
        private bool firstLoad = true;

        public async Task<bool> AddItemAsync(Log item)
        {
            if(string.IsNullOrEmpty(item.JaktId)){
                throw new ArgumentNullException("Missing required parameter: HuntId");
            }
            item.Changed = DateTime.Now;
            item.Created = DateTime.Now;
            item.ID = Guid.NewGuid().ToString();
            items.Add(item);

            items.SaveToLocalStorage(FILENAME);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Log item)
        {
            var _item = items.FirstOrDefault(i => i.ID == item.ID);
            items.Remove(_item);
            items.Add(item);

            items.SaveToLocalStorage(FILENAME);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var _item = items.FirstOrDefault(i => i.ID == id);
            items.Remove(_item);

            items.SaveToLocalStorage(FILENAME);

            return await Task.FromResult(true);
        }

        public async Task<Log> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.ID == id));
        }

        public async Task<List<Log>> GetItemsAsync(bool forceRefresh = false)
        {
            if (firstLoad || forceRefresh)
            {
                items = FileService.LoadFromLocalStorage<List<Log>>(FILENAME);
                firstLoad = false;
            }
            return await Task.FromResult(items);
        }

        public List<Log> GetCachedItems()
        {
            return items;
        }
    }
}
