using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Jaktloggen
{
    public class LogsViewModel : BaseViewModel
    {
        public HuntViewModel Hunt { get; set; }
        public ObservableCollection<Log> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public LogsViewModel(HuntViewModel hunt = null)
        {
            Hunt = hunt;
            Title = Hunt?.Location;
            Items = new ObservableCollection<Log>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            //MessagingCenter.Subscribe<LogDetailPage, Log>(this, "AddLog", async (obj, item) =>
            //{
            //    var _item = item as Log;
            //    Items.Add(_item);
            //    await App.LogDataStore.AddItemAsync(_item);
            //});
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await App.LogDataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
