using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;

namespace Jaktloggen
{
    public class JaktGroup : ObservableCollection<HuntViewModel>
    {
        public String Name { get; private set; }
        public String ShortName { get; private set; }

        public JaktGroup(String Name, String ShortName)
        {
            this.Name = Name;
            this.ShortName = ShortName;
        }
    }

    public class HuntsViewModel : BaseViewModel
    {
        public ObservableCollection<JaktGroup> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        private bool isLoaded { get; set; }

        public HuntsViewModel(INavigation navigation)
        {
            Navigation = navigation;
            Title = "Jaktloggen";
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand(), () => !IsBusy);
            Items = new ObservableCollection<JaktGroup>();
        }

        private async Task PopulateItems()
        {
            Items.Clear();

            await App.HunterDataStore.GetItemsAsync(true);
            await App.DogDataStore.GetItemsAsync(true);
            await App.LogDataStore.GetItemsAsync(true);
            await App.SpecieDataStore.GetItemsAsync(true);
            await App.CustomFieldDataStore.GetItemsAsync(true);

            var items = await App.HuntDataStore.GetItemsAsync(true);
            var groups = items.GroupBy(g => g.DatoFra.Year).OrderByDescending(o => o.Key);
            foreach (var g in groups)
            {
                var jg = new JaktGroup(g.Key.ToString(), "");
                foreach(var hunt in g.OrderByDescending(o => o.DatoFra))
                {
                    jg.Add(new HuntViewModel(hunt, Navigation));
                }
                Items.Add(jg);
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
