using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jaktloggen.DataStores.Mock
{
    public class HuntDataStore : IDataStore<Jakt>
    {
        List<Jakt> items;

        public HuntDataStore()
        {
            items = new List<Jakt>();
            for (var i = 1; i <= 10; i++)
            {
                items.Add(CreateHunt(i));
            }
        }

        private Jakt CreateHunt(int id){
            var hunt = new Jakt { 
                ID = id.ToString(), 
                Sted = "Høylandet",
                DatoFra = DateTime.Today.AddDays(new Random(id).Next(-2000, 0)),
                Notes="This is a note of some length. It can be short, but it can also be of, well, some length."
            };
            hunt.DatoTil = hunt.DatoFra.AddDays(new Random(id).Next(0, 10));
            return hunt;
        }
        public async Task<bool> AddItemAsync(Jakt item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Jakt item)
        {
            var _item = items.Where((Jakt arg) => arg.ID == item.ID).FirstOrDefault();
            items.Remove(_item);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var _item = items.Where((Jakt arg) => arg.ID == id).FirstOrDefault();
            items.Remove(_item);

            return await Task.FromResult(true);
        }

        public async Task<Jakt> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.ID == id));
        }

        public async Task<List<Jakt>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }

        public List<Jakt> GetCachedItems()
        {
            return items;
        }

        public Task<bool> UpdateItemsAsync(List<Jakt> items)
        {
            throw new NotImplementedException();
        }
    }
}
