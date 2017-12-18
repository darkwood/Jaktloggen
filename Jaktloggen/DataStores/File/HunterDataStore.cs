using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jaktloggen.Services;

namespace Jaktloggen.DataStores.File
{
    public class HunterDataStore : IDataStore<Hunter>
    {
        List<Hunter> items = new List<Hunter>();
        private static string FILENAME = "jegere.json";

        public async Task<bool> AddItemAsync(Hunter item)
        {
            item.Changed = DateTime.Now;
            item.Created = DateTime.Now;
            item.ID = Guid.NewGuid().ToString();
            items.Add(item);

            items.SaveToLocalStorage(FILENAME);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Hunter item)
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

        public async Task<Hunter> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.ID == id));
        }

        public async Task<List<Hunter>> GetItemsAsync(bool forceRefresh = false)
        {
            items = FileService.LoadFromLocalStorage<List<Hunter>>(FILENAME);
            return await Task.FromResult(items);
        }
    }
}
