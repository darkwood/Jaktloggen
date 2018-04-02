using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Jaktloggen
{
    public class CustomFieldsViewModel : BaseViewModel
    {
        public ObservableCollection<CustomFieldViewModel> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        private bool isLoaded { get; set; }
        public CustomFieldsViewModel(INavigation navigation)
        {
            Navigation = navigation;
            Items = new ObservableCollection<CustomFieldViewModel>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            Title = "Ekstra felter";

        }

        public async Task OnAppearing()
        {
            if (!isLoaded)
            {
                await ExecuteLoadItemsCommand();
            }
            isLoaded = true;
        }

        private async Task PopulateItems()
        {
            if (IsBusy) { return; }

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await App.CustomFieldDataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(new CustomFieldViewModel(item, Navigation));
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
