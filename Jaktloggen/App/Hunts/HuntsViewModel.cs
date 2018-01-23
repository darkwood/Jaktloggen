using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;

namespace Jaktloggen
{
    public class HuntsViewModel : BaseViewModel
    {
        public ObservableCollection<HuntViewModel> HuntItems { get; set; }
        private Command LoadItemsCommand { get; }
        private bool isLoaded { get; set; }

        public HuntsViewModel(INavigation navigation)
        {
            Navigation = navigation;
            Title = "Jaktloggen";
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand(), () => !IsBusy);
            HuntItems = new ObservableCollection<HuntViewModel>();
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

        public async void OnAppearing()
        {
            if (!isLoaded)
            {
                LoadItemsCommand.Execute(null);
            }
            isLoaded = true;
        }

        private async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;
            try { await PopulateItems(); }
            catch (Exception ex) { Debug.WriteLine(ex); }
            IsBusy = false;
        }
    }
}
