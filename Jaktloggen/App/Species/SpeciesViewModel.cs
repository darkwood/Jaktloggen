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
    public class SpecieGrouping : ObservableCollection<SpecieViewModel>
    {
        public String Name { get; private set; }
        public String ShortName { get; private set; }

        public SpecieGrouping(String Name, String ShortName)
        {
            this.Name = Name;
            this.ShortName = ShortName;
        }
    }
    public class SpeciesViewModel : BaseViewModel
    {
        public ObservableCollection<SpecieGrouping> GroupedItems { get; set; }
        public Command LoadItemsCommand { get; set; }
        private bool isLoaded { get; set; }
        private List<string> _selectedIds { get; set; }

        public SpeciesViewModel(INavigation navigation)
        {
            Navigation = navigation;
            Title = "Arter";
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            GroupedItems = new ObservableCollection<SpecieGrouping>();
            _selectedIds = new List<string>();

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

        public async Task UpdateSelectedSpecie(SpecieViewModel specie)
        {
            if(specie.Selected)
            {
                if(!_selectedIds.Any(s => s == specie.ID)){
                    _selectedIds.Add(specie.ID);
                    _selectedIds.SaveToLocalStorage(App.FILE_SELECTED_SPECIE_IDS);
                }
            } else{
                var i = _selectedIds.IndexOf(specie.ID);
                if(i > -1){
                    _selectedIds.RemoveAt(i);
                    _selectedIds.SaveToLocalStorage(App.FILE_SELECTED_SPECIE_IDS);
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

        public async Task DeleteItem(SpecieViewModel item)
        {
            await App.SpecieDataStore.DeleteItemAsync(item.ID);
            await PopulateItems();
        }

        private async Task PopulateItems()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                GroupedItems.Clear();
                var speciesGroups = await App.SpecieGroupDataStore.GetItemsAsync(true);
                var species = await App.SpecieDataStore.GetItemsAsync(true);
                _selectedIds = await FileService.LoadFromLocalStorage<List<string>>(App.FILE_SELECTED_SPECIE_IDS);
                foreach (var g in speciesGroups)
                {
                    var speciesInGroup = species.Where(a => a.GroupId == g.ID).ToList();

                    if (speciesInGroup.Any())
                    {
                        var ag = new SpecieGrouping(g.Navn, string.Empty);

                        foreach (var specie in speciesInGroup)
                        {
                            SpecieViewModel specieVm = new SpecieViewModel(specie, Navigation);
                            specieVm.Selected = _selectedIds.Any(s => s == specie.ID);
                            ag.Add(specieVm);
                        }

                        GroupedItems.Add(ag);
                    }
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
