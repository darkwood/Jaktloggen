using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jaktloggen.Services;

namespace Jaktloggen.DataStores.File
{
    public class HuntDataStore : IDataStore<Hunt>
    {
        List<Hunt> items = new List<Hunt>();
        private static string FILENAME = "jakt.json";

        public async Task<bool> AddItemAsync(Hunt item)
        {
            item.Changed = DateTime.Now;
            item.Created = DateTime.Now;
            item.ID = Guid.NewGuid().ToString();
            items.Add(item);

            await items.SaveToLocalStorage(FILENAME);

            return true;
        }

        public async Task<bool> UpdateItemAsync(Hunt item)
        {
            var _item = items.Where((Hunt arg) => arg.ID == item.ID).FirstOrDefault();
            items.Remove(_item);
            items.Add(item);

            await items.SaveToLocalStorage(FILENAME);

            return true;
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var _item = items.Where((Hunt arg) => arg.ID == id).FirstOrDefault();
            items.Remove(_item);

            await items.SaveToLocalStorage(FILENAME);
            return true;
        }

        public async Task<Hunt> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.ID == id));
        }

        public async Task<List<Hunt>> GetItemsAsync(bool forceRefresh = false)
        {
            items = await FileService.LoadFromLocalStorage<List<Hunt>>(FILENAME);
            return items;
        }
    }
}
