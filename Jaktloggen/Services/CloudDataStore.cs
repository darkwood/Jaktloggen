using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Plugin.Connectivity;

namespace Jaktloggen
{
    public class CloudDataStore<T> : IDataStore<T>
    {
        HttpClient client;
        IEnumerable<T> items;

        public CloudDataStore()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri($"{App.BackendUrl}/");

            items = new List<T>();
        }

        public async Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false)
        {
            if (forceRefresh && CrossConnectivity.Current.IsConnected)
            {
                var json = await client.GetStringAsync($"api/{typeof(T).Name.ToLower()}");
                items = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<T>>(json));
            }

            return items;
        }

        public async Task<T> GetItemAsync(string id)
        {
            if (id != null && CrossConnectivity.Current.IsConnected)
            {
                var json = await client.GetStringAsync($"api/{typeof(T).Name.ToLower()}/{id}");
                return await Task.Run(() => JsonConvert.DeserializeObject<T>(json));
            }
            return default(T);
        }

        public async Task<bool> AddItemAsync(T item)
        {
            if (item == null || !CrossConnectivity.Current.IsConnected)
                return false;

            var serializedItem = JsonConvert.SerializeObject(item);

            var response = await client.PostAsync($"api/{typeof(T).Name.ToLower()}", new StringContent(serializedItem, Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateItemAsync(T i)
        {
            Hunt item = i as Hunt; //TODO: replace with base class
            if (item == null || item.Id == null || !CrossConnectivity.Current.IsConnected)
                return false;

            var serializedItem = JsonConvert.SerializeObject(item);
            var buffer = Encoding.UTF8.GetBytes(serializedItem);
            var byteContent = new ByteArrayContent(buffer);

            var response = await client.PutAsync(new Uri($"api/{typeof(T).Name.ToLower()}/{item.Id}"), byteContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            if (string.IsNullOrEmpty(id) && !CrossConnectivity.Current.IsConnected)
                return false;

            var response = await client.DeleteAsync($"api/{typeof(T).Name.ToLower()}/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}
