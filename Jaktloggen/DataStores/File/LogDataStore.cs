using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jaktloggen.Services.FIle
{
    public class LogDataStore : IDataStore<Log>
    {
        List<Log> items = new List<Log>();
        private static string FILENAME = "logger.json";

        public async Task<bool> AddItemAsync(Log item)
        {
            item.Changed = DateTime.Now;
            item.Created = DateTime.Now;
            item.ID = Guid.NewGuid().ToString();
            items.Add(item);

            items.SaveToLocalStorage(FILENAME);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Log item)
        {
            var _item = items.Where((Log arg) => arg.ID == item.ID).FirstOrDefault();
            items.Remove(_item);
            items.Add(item);

            items.SaveToLocalStorage(FILENAME);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var _item = items.Where((Log arg) => arg.ID == id).FirstOrDefault();
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
            items = FileService.LoadFromLocalStorage<List<Log>>(FILENAME);
            return await Task.FromResult(items);
        }
    }
}
