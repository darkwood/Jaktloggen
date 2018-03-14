using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jaktloggen.Services;

namespace Jaktloggen.DataStores.File
{
    public class DataStore<T> : IDataStore<T> where T : BaseEntity
    {
        List<T> _items = new List<T>();
        public string Filename { get; set; }
        private bool firstLoad = true;

        public DataStore()
        {
            object p = typeof(T);
            if (p == typeof(Art))
            {
                Filename = "arter.xml";
            } 
            else if (p == typeof(Jakt))
            {
                Filename = "jakt.json";
            } 
            else if (p == typeof(Logg))
            {
                Filename = "logger.json";
            } 
            else if (p == typeof(Jeger))
            {
                Filename = "jegere.json";
            } 
            else if (p == typeof(Dog))
            {
                Filename = "dogs.json";
            }
            else if (p == typeof(ArtGroup))
            {
                Filename = "artgroup.xml";
            }
            else
            {
                throw new NotImplementedException(typeof(T).ToString() + " sitt filnavn er ikke implementert.");
            }

        }
        public async Task<bool> AddItemAsync(T item)
        {
            item.Changed = DateTime.Now;
            item.Created = DateTime.Now;
            item.ID = Guid.NewGuid().ToString();
            _items.Add(item);

            _items.SaveToLocalStorage(Filename);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(T item)
        {
            var _item = _items.FirstOrDefault(i => i.ID == item.ID);
            _items.Remove(_item);
            _items.Add(item);

            _items.SaveToLocalStorage(Filename);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemsAsync(List<T> items)
        {
            _items = items;
            _items.SaveToLocalStorage(Filename);

            return await Task.FromResult(true);  
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var _item = _items.FirstOrDefault(i => i.ID == id);
            _items.Remove(_item);

            _items.SaveToLocalStorage(Filename);
            return await Task.FromResult(true);
        }

        public async Task<T> GetItemAsync(string id)
        {
            return await Task.FromResult(_items.FirstOrDefault(s => s.ID == id));
        }

        public async Task<List<T>> GetItemsAsync(bool forceRefresh = false)
        {
            if(firstLoad || forceRefresh)
            {
                _items = await FileService.LoadFromLocalStorage<List<T>>(Filename);
                firstLoad = false;
            }

            return await Task.FromResult(_items);
        }
    }
}
