using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Jaktloggen
{
    public class HuntsViewModel : BaseViewModel
    {
        public ObservableCollection<Hunt> HuntItems { get; set; }
        public Command LoadHuntItemsCommand { get; set; }

        public HuntsViewModel()
        {
            Title = "Jaktloggen";
            HuntItems = new ObservableCollection<Hunt>();
            LoadHuntItemsCommand = new Command(async () => await ExecuteLoadHuntItemsCommand());

            MessagingCenter.Subscribe<HuntDetailPage, Hunt>(this, "SaveHunt", async (obj, item) =>
            {
                if (!string.IsNullOrEmpty(item.Id))
                {
                    await App.HuntDataStore.UpdateItemAsync(item);
                }
                else
                {
                    HuntItems.Add(item);
                    await App.HuntDataStore.AddItemAsync(item);
                    SortItems();
                }
            });
        }

        private void SortItems()
        {
            HuntItems.Clear();
            var items = HuntItems.ToArray().OrderByDescending(o => o.DateFrom);
            foreach (var item in items)
            {
                HuntItems.Add(item);
            }
            
        }

        async Task ExecuteLoadHuntItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                HuntItems.Clear();
                var items = await App.HuntDataStore.GetItemsAsync(true);
                foreach (var item in items.OrderByDescending(o => o.DateFrom))
                {
                    HuntItems.Add(item);
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
