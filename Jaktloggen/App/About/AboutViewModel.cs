using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Jaktloggen.Services;

using Newtonsoft.Json;

using Xamarin.Forms;

namespace Jaktloggen
{
    public class AboutViewModel : BaseViewModel
    {
        private HttpClient client;

        public ICommand ImportDataCommand { get; }
        public ICommand ExportDataCommand { get; }
        public ICommand OpenWebCommand { get; }
        public int ExportCount { get; set; }
        public HttpStatusCode ImportStatusCode { get; set; }
        private string[] _fileList { get; }

        private string _servicePath = "/JaktDataService/api/dataservice?filename=";
        public AboutViewModel()
        {
            Title = "Om Jaktloggen";

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("http://www.jaktloggen.no")));
            ImportDataCommand = new Command(async () => await ImportData());
            ExportDataCommand = new Command(async () => await ExportData());
            
            client = new HttpClient();
            client.BaseAddress = new Uri("http://tom-zb15.dips.local");
            _fileList = new[] { "jakt.json", "logger.json", "jegere.json", "dogs.json" };
        }

        public async Task ExportData()
        {
            ExportCount = 0;
            foreach (var filename in _fileList)
            {
                var data = FileService.ReadFile(filename);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(_servicePath + filename, content);
                if (response.IsSuccessStatusCode)
                {
                    ExportCount++;
                }
            }
        }

        public async Task ImportData()
        {
            foreach (var filename in _fileList)
            {
                HttpResponseMessage response = await client.GetAsync(_servicePath + filename);
                ImportStatusCode = response.StatusCode;

                var data = await response.Content.ReadAsStringAsync();
                if (data != null)
                {
                    SaveToDisk<Jakt>(filename, data);
                }
            }
        }

        private static void SaveToDisk<T>(string filename, string data)
        {
            List<T> typedItem = JsonConvert.DeserializeObject<List<T>>(data);
            typedItem.SaveToLocalStorage(filename);
        }
    }
}
