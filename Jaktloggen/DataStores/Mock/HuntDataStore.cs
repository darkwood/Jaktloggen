using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jaktloggen.DataStores.Mock
{
    public class HuntDataStore : IDataStore<Hunt>
    {
        List<Hunt> items;

        public HuntDataStore()
        {
            items = new List<Hunt>();
            for (var i = 1; i <= 10; i++)
            {
                items.Add(CreateHunt(i));
            }
        }

        private Hunt CreateHunt(int id){
            var hunt = new Hunt { 
                ID = id.ToString(), 
                Sted = "Høylandet",
                DatoFra = DateTime.Today.AddDays(new Random(id).Next(-2000, 0)),
                Notes="This is a note of some length. It can be short, but it can also be of, well, some length."
            };
            hunt.DatoTil = hunt.DatoFra.AddDays(new Random(id).Next(0, 10));
            return hunt;
        }
        public async Task<bool> AddItemAsync(Hunt item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Hunt item)
        {
            var _item = items.Where((Hunt arg) => arg.ID == item.ID).FirstOrDefault();
            items.Remove(_item);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var _item = items.Where((Hunt arg) => arg.ID == id).FirstOrDefault();
            items.Remove(_item);

            return await Task.FromResult(true);
        }

        public async Task<Hunt> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.ID == id));
        }

        public async Task<List<Hunt>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }

        public List<Hunt> GetCachedItems()
        {
            return items;
        }
    }
}
