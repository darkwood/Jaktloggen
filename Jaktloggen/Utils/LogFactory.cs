using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jaktloggen.Models;
using Jaktloggen.Services;

namespace Jaktloggen.Utils
{
    public static class LogFactory
    {
        public static async Task<List<PickerItem>> CreatePickerSpecies(string selectedSpecieId, int max = int.MaxValue)
        {
            var pickerItems = new List<PickerItem>();

            var species = await App.SpecieDataStore.GetItemsAsync();
            var favouriteSpecies = await FileService.LoadFromLocalStorage<List<string>>(App.FILE_SELECTED_SPECIE_IDS);
            var selectedItem = species.SingleOrDefault(i => i.ID == selectedSpecieId);
            if (selectedItem != null)
            {
                pickerItems.Add(LogFactory.CreatePickerItem(selectedSpecieId, selectedItem));
            }

            var remainingSpots = max - pickerItems.Count;
            foreach (var i in species
                    .Where(j => favouriteSpecies.Contains(j.ID) && j.ID != selectedSpecieId)
                    .Take(remainingSpots))
            {
                pickerItems.Add(LogFactory.CreatePickerItem(selectedSpecieId, i));
            }

            remainingSpots = max - pickerItems.Count;

            if(remainingSpots > 0)
            {
                foreach (var i in species)
                {
                    if (pickerItems.Any(p => p.ID == i.ID)) { continue; }

                    pickerItems.Add(LogFactory.CreatePickerItem(selectedSpecieId, i));
                    remainingSpots --;

                    if (remainingSpots <= 0) { break; }
                }
            }


            return pickerItems;
        }

        internal static async Task<string> CreateLogSummary(Logg l)
		{
            var desc = "";

            if (!string.IsNullOrEmpty(l.ArtId))
            {
                var arter = await App.SpecieDataStore.GetItemsAsync();
                var art = arter.SingleOrDefault(a => a.ID == l.ArtId);
                if (art != null)
                {
                    desc += art.Navn + ". ";
                }
            }

            if (l.Skudd > 0)
                desc += l.Skudd + " skudd. " + l.Treff + " treff. ";

            if (l.Sett > 0)
                desc += l.Sett + " sett. ";
            
            if (!string.IsNullOrEmpty(l.JegerId))
            {
                var jegere = await App.HunterDataStore.GetItemsAsync();
                var jeger = jegere.SingleOrDefault(j => j.ID == l.JegerId);
                if(jeger != null) 
                {
                    desc += "Av " + jeger.Fornavn;
                }
            }

            return desc;
		}

		private static PickerItem CreatePickerItem(string selectedSpecieId, Art art)
        {
            return new PickerItem
            {
                ID = art.ID,
                Title = art.Navn,
                Selected = art.ID == selectedSpecieId
            };
        }

        public static async Task<List<PickerItem>> CreatePickerHunters(string selectedJegerId, int max = int.MaxValue)
		{
            var items = new List<PickerItem>();

            var hunters = await App.HunterDataStore.GetItemsAsync();

            var selectedItem = hunters.SingleOrDefault(i => i.ID == selectedJegerId);
            if (selectedItem != null)
            {
                items.Add(LogFactory.CreatePickerItem(selectedJegerId, selectedItem));
            }

            var remainingSpots = max - items.Count;
            foreach (var i in hunters
                     .Where(j => j.ID != selectedJegerId)
                    .Take(remainingSpots))
            {
                items.Add(LogFactory.CreatePickerItem(selectedJegerId, i));
            }

            return items;
		}

        private static PickerItem CreatePickerItem(string selectedId, Dog item)
        {
            var dogVM = new DogViewModel(item, null);
            return new PickerItem
            {
                ID = dogVM.ID,
                Title = dogVM.Name,
                ImageSource = dogVM.Image,
                Selected = dogVM.ID == selectedId
            };
        }

        public static async Task<List<PickerItem>> CreatePickerDogs(string selectedDogId, int max = int.MaxValue)
        {
            var items = new List<PickerItem>();

            var dogs = await App.DogDataStore.GetItemsAsync();

            var selectedItem = dogs.SingleOrDefault(i => i.ID == selectedDogId);
            if (selectedItem != null)
            {
                items.Add(LogFactory.CreatePickerItem(selectedDogId, selectedItem));
            }

            var remainingSpots = max - items.Count;
            foreach (var i in dogs
                     .Where(j => j.ID != selectedDogId)
                    .Take(remainingSpots))
            {
                items.Add(LogFactory.CreatePickerItem(selectedDogId, i));
            }

            return items;
        }

        private static PickerItem CreatePickerItem(string selectedId, Jeger jeger)
        {
            var hunterVm = new HunterViewModel(jeger, null);
            return new PickerItem
            {
                ID = hunterVm.ID,
                Title = hunterVm.Firstname,
                ImageSource = hunterVm.Image,
                Selected = hunterVm.ID == selectedId
            };
        }
	}
}
