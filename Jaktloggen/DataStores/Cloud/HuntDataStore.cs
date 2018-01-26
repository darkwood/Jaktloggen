using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Plugin.Connectivity;

namespace Jaktloggen.DataStores.Cloud
{
    public class HuntDataStore : IDataStore<Jakt>
    {
        HttpClient client;
        List<Jakt> items;

        public HuntDataStore()
        {
            client = new HttpClient();
            //TODO: Authenticate user
            client.BaseAddress = new Uri($"{App.BackendUrl}/");

            items = new List<Jakt>();
        }

        public async Task<List<Jakt>> GetItemsAsync(bool forceRefresh = false)
        {
            if (forceRefresh) // && CrossConnectivity.Current.IsConnected)
            {
                var json = await client.GetStringAsync($"api/hunts/");
                items = await Task.Run(() => JsonConvert.DeserializeObject<List<Jakt>>(json));
            }

            return items;
        }

        public async Task<Jakt> GetItemAsync(string id)
        {
            if (id != null && CrossConnectivity.Current.IsConnected)
            {
                var json = await client.GetStringAsync($"api/hunt/{id}");
                return await Task.Run(() => JsonConvert.DeserializeObject<Jakt>(json));
            }
            return default(Jakt);
        }

        public async Task<bool> AddItemAsync(Jakt item)
        {
            if (item == null || !CrossConnectivity.Current.IsConnected)
                return false;

            var serializedItem = JsonConvert.SerializeObject(item);

            var response = await client.PostAsync($"api/hunt", new StringContent(serializedItem, Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateItemAsync(Jakt item)
        {
            if (item == null || item.ID == null || !CrossConnectivity.Current.IsConnected)
                return false;

            var serializedItem = JsonConvert.SerializeObject(item);
            var buffer = Encoding.UTF8.GetBytes(serializedItem);
            var byteContent = new ByteArrayContent(buffer);

            var response = await client.PutAsync(new Uri($"api/hunt/{item.ID}"), byteContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            if (string.IsNullOrEmpty(id) && !CrossConnectivity.Current.IsConnected)
                return false;

            var response = await client.DeleteAsync($"api/hunt/{id}");

            return response.IsSuccessStatusCode;
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
