using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Jaktloggen.Services;
using Xamarin.Forms;

namespace Jaktloggen
{
    public class CustomFieldsViewModel : BaseViewModel
    {
        public ObservableCollection<CustomFieldViewModel> Items { get; set; }

        private List<string> _selectedLoggTypeIdList = new List<string>();

        public Command LoadItemsCommand { get; set; }
        private bool isLoaded { get; set; }
        public List<string> _selectedIds { get; private set; }

        public CustomFieldsViewModel(INavigation navigation)
        {
            Navigation = navigation;
            Items = new ObservableCollection<CustomFieldViewModel>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            Title = "Ekstra felter";

        }

        internal void UpdateItem(CustomFieldViewModel item)
        {
            if (item.Selected)
            {
                if (!_selectedIds.Any(s => s == item.ID))
                {
                    _selectedIds.Add(item.ID);
                    _selectedIds.SaveToLocalStorage(App.FILE_SELECTED_LOGGTYPEIDS);
                }
            }
            else
            {
                var i = _selectedIds.IndexOf(item.ID);
                if (i > -1)
                {
                    _selectedIds.RemoveAt(i);
                    _selectedIds.SaveToLocalStorage(App.FILE_SELECTED_LOGGTYPEIDS);
                }
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

        private async Task PopulateItems()
        {
            if (IsBusy) { return; }

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await App.CustomFieldDataStore.GetItemsAsync(true);
                _selectedIds = await FileService.LoadFromLocalStorage<List<string>>(App.FILE_SELECTED_LOGGTYPEIDS);
            
                foreach (var item in items)
                {
                    var vm = new CustomFieldViewModel(item, Navigation);
                    vm.Selected = _selectedIds.Contains(vm.ID);
                    Items.Add(vm);
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
