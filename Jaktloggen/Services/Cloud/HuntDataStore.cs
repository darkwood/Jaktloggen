using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Plugin.Connectivity;

namespace Jaktloggen.Services.Cloud
{
    public class HuntDataStore : IDataStore<Hunt>
    {
        HttpClient client;
        IEnumerable<Hunt> items;

        public HuntDataStore()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri($"{App.BackendUrl}/");

            items = new List<Hunt>();
        }

        public async Task<IEnumerable<Hunt>> GetItemsAsync(bool forceRefresh = false)
        {
            if (forceRefresh) // && CrossConnectivity.Current.IsConnected)
            {
                var json = await client.GetStringAsync($"api/hunts/");
                items = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Hunt>>(json));
            }

            return items;
        }

        public async Task<Hunt> GetItemAsync(string id)
        {
            if (id != null && CrossConnectivity.Current.IsConnected)
            {
                var json = await client.GetStringAsync($"api/hunt/{id}");
                return await Task.Run(() => JsonConvert.DeserializeObject<Hunt>(json));
            }
            return default(Hunt);
        }

        public async Task<bool> AddItemAsync(Hunt item)
        {
            if (item == null || !CrossConnectivity.Current.IsConnected)
                return false;

            var serializedItem = JsonConvert.SerializeObject(item);

            var response = await client.PostAsync($"api/hunt", new StringContent(serializedItem, Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateItemAsync(Hunt item)
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
    }
}
