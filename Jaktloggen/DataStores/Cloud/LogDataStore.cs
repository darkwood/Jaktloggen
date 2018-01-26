using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Plugin.Connectivity;

namespace Jaktloggen.DataStores.Cloud
{
    public class LogDataStore : IDataStore<Logg>
    {
        HttpClient client;
        List<Logg> items;

        public LogDataStore()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri($"{App.BackendUrl}/");

            items = new List<Logg>();
        }

        public async Task<List<Logg>> GetItemsAsync(bool forceRefresh = false)
        {
            if (forceRefresh && CrossConnectivity.Current.IsConnected)
            {
                var json = await client.GetStringAsync($"api/Logs/");
                items = await Task.Run(() => JsonConvert.DeserializeObject<List<Logg>>(json));
            }

            return items;
        }

        public async Task<Logg> GetItemAsync(string id)
        {
            if (id != null && CrossConnectivity.Current.IsConnected)
            {
                var json = await client.GetStringAsync($"api/Log/{id}");
                return await Task.Run(() => JsonConvert.DeserializeObject<Logg>(json));
            }
            return default(Logg);
        }

        public async Task<bool> AddItemAsync(Logg item)
        {
            if (item == null || !CrossConnectivity.Current.IsConnected)
                return false;

            var serializedItem = JsonConvert.SerializeObject(item);

            var response = await client.PostAsync($"api/Log", new StringContent(serializedItem, Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateItemAsync(Logg item)
        {
            if (item == null || item.ID == null || !CrossConnectivity.Current.IsConnected)
                return false;

            var serializedItem = JsonConvert.SerializeObject(item);
            var buffer = Encoding.UTF8.GetBytes(serializedItem);
            var byteContent = new ByteArrayContent(buffer);

            var response = await client.PutAsync(new Uri($"api/Log/{item.ID}"), byteContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            if (string.IsNullOrEmpty(id) && !CrossConnectivity.Current.IsConnected)
                return false;

            var response = await client.DeleteAsync($"api/Log/{id}");

            return response.IsSuccessStatusCode;
        }

        public List<Logg> GetCachedItems()
        {
            return items;
        }

        public Task<bool> UpdateItemsAsync(List<Logg> items)
        {
            throw new NotImplementedException();
        }
    }
}
