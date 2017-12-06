using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jaktloggen.Services.FIle
{
    public class LogDataStore : IDataStore<Log>
    {
        List<Log> items;

        public LogDataStore()
        {
            
        }

        public async Task<bool> AddItemAsync(Log item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Log item)
        {
            var _item = items.Where((Log arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(_item);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var _item = items.Where((Log arg) => arg.Id == id).FirstOrDefault();
            items.Remove(_item);

            return await Task.FromResult(true);
        }

        public async Task<Log> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<List<Log>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}
