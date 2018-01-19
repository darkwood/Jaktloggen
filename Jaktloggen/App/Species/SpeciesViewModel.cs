using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Jaktloggen
{
    public class SpeciesViewModel : BaseViewModel
    {
        public ObservableCollection<SpecieViewModel> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        private bool isLoaded { get; set; }

        public SpeciesViewModel(INavigation navigation)
        {
            Navigation = navigation;
            Items = new ObservableCollection<SpecieViewModel>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<SpeciePage, SpecieViewModel>(this, "Save", async (obj, item) =>
            {
                if (!string.IsNullOrEmpty(item.ID))
                {
                    await App.SpecieDataStore.UpdateItemAsync(item.Item);
                }
                else
                {
                    await App.SpecieDataStore.AddItemAsync(item.Item);
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

        public async Task DeleteItem(SpecieViewModel item)
        {
            await App.SpecieDataStore.DeleteItemAsync(item.ID);
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
                var items = await App.SpecieDataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(new SpecieViewModel(item, Navigation));
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
