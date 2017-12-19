using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Jaktloggen
{
    public class HuntersViewModel : BaseViewModel
    {
        public ObservableCollection<HunterViewModel> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public HuntersViewModel()
        {
            Items = new ObservableCollection<HunterViewModel>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<HunterPage, HunterViewModel>(this, "Save", async (obj, item) =>
            {
                if (!string.IsNullOrEmpty(item.ID))
                {
                    await App.HunterDataStore.UpdateItemAsync(item.Item);
                }
                else
                {
                    await App.HunterDataStore.AddItemAsync(item.Item);
                }

                await PopulateItems();
            });

            MessagingCenter.Subscribe<HunterPage, HunterViewModel>(this, "Delete", async (obj, item) =>
            {
                await DeleteItem(item);
            });
        }

        public async Task DeleteItem(HunterViewModel item)
        {
            await App.HunterDataStore.DeleteItemAsync(item.ID);
            Items.Remove(item);
        }

        private async Task PopulateItems()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {

                Items.Clear();
                var items = await App.HunterDataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(new HunterViewModel(item, Navigation));
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

        public async Task ExecuteLoadItemsCommand()
        {
            await PopulateItems();
        }
    }
}
