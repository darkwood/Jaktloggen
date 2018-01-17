using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jaktloggen.Services;

namespace Jaktloggen.DataStores.File
{
    public class DogDataStore : IDataStore<Dog>
    {
        List<Dog> items = new List<Dog>();
        private static string FILENAME = "dogs.json";
        private bool firstLoad = true;

        public async Task<bool> AddItemAsync(Dog item)
        {
            item.Changed = DateTime.Now;
            item.Created = DateTime.Now;
            item.ID = Guid.NewGuid().ToString();
            items.Add(item);

            items.SaveToLocalStorage(FILENAME);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Dog item)
        {
            var _item = items.FirstOrDefault(i => i.ID == item.ID);
            items.Remove(_item);
            items.Add(item);

            items = await GetItemsAsync(true);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var _item = items.FirstOrDefault(i => i.ID == id);
            items.Remove(_item);

            items.SaveToLocalStorage(FILENAME);
            return await Task.FromResult(true);
        }

        public async Task<Dog> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.ID == id));
        }

        public async Task<List<Dog>> GetItemsAsync(bool forceRefresh = false)
        {
            if(firstLoad || forceRefresh)
            {
                items = FileService.LoadFromLocalStorage<List<Dog>>(FILENAME);
                firstLoad = false;
            }

            return await Task.FromResult(items);
        }

        public List<Dog> GetCachedItems()
        {
            return items;
        }
    }
}
