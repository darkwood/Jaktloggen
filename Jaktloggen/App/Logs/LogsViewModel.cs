using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Jaktloggen
{
    public class LogsViewModel : BaseViewModel
    {
        public HuntViewModel Hunt { get; set; }
        public ObservableCollection<LogViewModel> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        private bool isLoaded { get; set; }

        bool _hasItems;
        public bool HasItems
        {
            get => _hasItems;
            set { _hasItems = value; OnPropertyChanged(nameof(HasItems)); }
        }

        public LogsViewModel(HuntViewModel hunt, INavigation navigation)
        {
            Hunt = hunt;
            Navigation = navigation;
            Title = Hunt?.Location;
            Items = new ObservableCollection<LogViewModel>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
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
            if (IsBusy) { return; }

            IsBusy = true;
            try { await PopulateItems(); }
            catch (Exception ex) { Debug.WriteLine(ex); }
            finally { IsBusy = false; }
        }

        private async Task PopulateItems()
        {
            Items.Clear();
            var logs = await App.LogDataStore.GetItemsAsync();
            await App.HunterDataStore.GetItemsAsync();
            await App.DogDataStore.GetItemsAsync();
            var items = logs.Where(l => l.JaktId == Hunt.ID).OrderByDescending(o => o.Dato).ThenByDescending(d => d.Created).ToList();
            foreach (var item in items)
            {
                Items.Add(new LogViewModel(Hunt, item, Navigation));
            }
            HasItems = items.Count > 0;
        }
    }
}
