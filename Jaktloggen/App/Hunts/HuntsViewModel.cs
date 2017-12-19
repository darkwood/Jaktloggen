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
        public ObservableCollection<HuntViewModel> HuntItems { get; set; }
        public Command LoadHuntItemsCommand { get; set; }

        public HuntsViewModel(INavigation navigation)
        {
            Navigation = navigation;
            Title = "Jaktloggen";
            HuntItems = new ObservableCollection<HuntViewModel>();
            LoadHuntItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<HuntPage, HuntViewModel>(this, "Save", async (obj, item) =>
            {
                if (!string.IsNullOrEmpty(item.ID))
                {
                    await App.HuntDataStore.UpdateItemAsync(item.Item);
                }
                else
                {
                    await App.HuntDataStore.AddItemAsync(item.Item);
                }

                await PopulateItems();
            });

            MessagingCenter.Subscribe<HuntPage, HuntViewModel>(this, "Delete", async (obj, item) =>
            {
                await DeleteItem(item);
            });
        }

        public async Task DeleteItem(HuntViewModel item)
        {
            await App.HuntDataStore.DeleteItemAsync(item.ID);
            HuntItems.Remove(item);
        }

        private async Task PopulateItems()
        {
            HuntItems.Clear();
            var items = await App.HuntDataStore.GetItemsAsync(true);
            foreach (var item in items.OrderByDescending(o => o.DatoFra))
            {
                HuntItems.Add(new HuntViewModel(item, Navigation));
            }
        }

        public async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                await PopulateItems();
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
