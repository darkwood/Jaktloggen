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
        public ObservableCollection<HuntViewModel> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        private bool isLoaded { get; set; }

        public HuntsViewModel(INavigation navigation)
        {
            Navigation = navigation;
            Title = "Jaktloggen";
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand(), () => !IsBusy);
            Items = new ObservableCollection<HuntViewModel>();
        }

        private async Task PopulateItems()
        {
            Items.Clear();

            await App.HunterDataStore.GetItemsAsync(true);
            await App.DogDataStore.GetItemsAsync(true);
            await App.LogDataStore.GetItemsAsync(true);
            await App.SpecieDataStore.GetItemsAsync(true);

            var items = await App.HuntDataStore.GetItemsAsync(true);
            foreach (var item in items.OrderByDescending(o => o.DatoFra))
            {
                Items.Add(new HuntViewModel(item, Navigation));
            }
        }

        public async Task OnAppearing()
        {
            await ExecuteLoadItemsCommand();
        }

        public async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy) { return; }

            IsBusy = true;
            try { await PopulateItems(); }
            catch (Exception ex) { Debug.WriteLine(ex); }
            finally { IsBusy = false; }
        }
    }
}
