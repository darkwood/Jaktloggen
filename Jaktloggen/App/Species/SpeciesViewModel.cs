using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

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

        public SpeciesViewModel(INavigation navigation)
        {
            Navigation = navigation;
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
            await PopulateItems();
        }

        private async Task PopulateItems()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                GroupedItems = new ObservableCollection<SpecieGrouping>();
                var speciesGroups = await App.SpecieGroupDataStore.GetItemsAsync();
                var species = await App.SpecieDataStore.GetItemsAsync(true);
                foreach (var g in speciesGroups)
                {
                    var speciesInGroup = species.Where(a => a.GroupId == g.ID).ToList();

                    if (speciesInGroup.Any())
                    {
                        var ag = new SpecieGrouping(g.Navn, string.Empty);

                        foreach (var specie in speciesInGroup)
                        {
                            //specie.Selected = selectedArtIds.Any(s => s == specie.ID);
                            ag.Add(new SpecieViewModel(specie, Navigation));
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
