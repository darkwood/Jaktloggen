using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Jaktloggen
{
    public class DogsViewModel : BaseViewModel
    {
        public ObservableCollection<DogViewModel> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        private bool isLoaded { get; set; }

        public DogsViewModel(INavigation navigation)
        {
            Navigation = navigation;
            Items = new ObservableCollection<DogViewModel>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<DogPage, DogViewModel>(this, "Save", async (obj, item) =>
            {
                if (!string.IsNullOrEmpty(item.ID))
                {
                    await App.DogDataStore.UpdateItemAsync(item.Item);
                }
                else
                {
                    await App.DogDataStore.AddItemAsync(item.Item);
                }

                await PopulateItems();
            });
        }

        public async Task OnAppearing()
        {
            if (!isLoaded)
            {
                await ExecuteLoadItemsCommand();
            }
            isLoaded = true;
        }

        public async Task DeleteItem(DogViewModel item)
        {
            await App.DogDataStore.DeleteItemAsync(item.ID);
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
                var items = await App.DogDataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(new DogViewModel(item, Navigation));
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
