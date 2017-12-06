using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jaktloggen.DataStores.Mock
{
    public class LogDataStore : IDataStore<Log>
    {
        List<Log> items;

        public LogDataStore()
        {
            items = new List<Log>();
            for (var i = 1; i <= 10; i++)
            {
                items.Add(CreateLog(i));
            }
        }

        private Log CreateLog(int id){
            return new Log { 
                Id = id.ToString(),
                HuntId = new Random().Next(1, 10).ToString(),
                Date = DateTime.Now.AddHours(new Random().Next(-5, 5)).AddMinutes(new Random().Next(0, 59)),
                Latitude = "63." + new Random().Next(3800, 4300),
                Longitude = "10." + new Random().Next(2000, 3000),
                Notes ="This is a note of some length. It can be short, but it can also be of, well, some length." };
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
