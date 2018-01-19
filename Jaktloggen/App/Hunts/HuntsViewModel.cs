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
        private Command LoadHuntItemsCommand { get; set; }
        private bool isLoaded { get; set; }

        public HuntsViewModel(INavigation navigation)
        {
            Navigation = navigation;
            Title = "Jaktloggen";
            HuntItems = new ObservableCollection<HuntViewModel>();
            LoadHuntItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        private async Task PopulateItems()
        {
            HuntItems.Clear();

            await App.HunterDataStore.GetItemsAsync(true);
            await App.DogDataStore.GetItemsAsync(true);
            await App.LogDataStore.GetItemsAsync(true);
            await App.SpecieDataStore.GetItemsAsync(true);

            var items = await App.HuntDataStore.GetItemsAsync(true);
            foreach (var item in items.OrderByDescending(o => o.DatoFra))
            {
                HuntItems.Add(new HuntViewModel(item, Navigation));
            }
        }

        public async Task OnAppearing()
        {
            if (!isLoaded)
            {
                await ExecuteLoadItemsCommand();
            }
            isLoaded = true;
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
