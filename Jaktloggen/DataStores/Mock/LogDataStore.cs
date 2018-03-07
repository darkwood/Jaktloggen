using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jaktloggen.DataStores.Mock
{
    public class LogDataStore : IDataStore<Logg>
    {
        List<Logg> items;

        public LogDataStore()
        {
            items = new List<Logg>();
            for (var i = 1; i <= 10; i++)
            {
                items.Add(CreateLog(i));
            }
        }

        private Logg CreateLog(int id){
            return new Logg { 
                ID = id.ToString(),
                JaktId = new Random().Next(1, 10).ToString(),
                Dato = DateTime.Now.AddHours(new Random().Next(-5, 5)).AddMinutes(new Random().Next(0, 59)),
                Latitude = "63." + new Random().Next(3800, 4300),
                Longitude = "10." + new Random().Next(2000, 3000),
                Notes ="This is a note of some length. It can be short, but it can also be of, well, some length." };
        }

        public async Task<bool> AddItemAsync(Logg item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Logg item)
        {
            var _item = items.Where((Logg arg) => arg.ID == item.ID).FirstOrDefault();
            items.Remove(_item);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var _item = items.Where((Logg arg) => arg.ID == id).FirstOrDefault();
            items.Remove(_item);

            return await Task.FromResult(true);
        }

        public async Task<Logg> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.ID == id));
        }

        public async Task<List<Logg>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }

        public Task<bool> UpdateItemsAsync(List<Logg> items)
        {
            throw new NotImplementedException();
        }
    }
}
