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
        public static async Task<List<PickerItem>> CreatePickerSpecies(string selectedSpecieId)
        {
            var items = new List<PickerItem>();

            var species = await App.SpecieDataStore.GetItemsAsync();
            var favouriteSpecies = await FileService.LoadFromLocalStorage<List<string>>(App.FILE_SELECTED_SPECIE_IDS);
            var selectedItem = species.SingleOrDefault(i => i.ID == selectedSpecieId);
            if (selectedItem != null)
            {
                items.Add(LogFactory.CreatePickerItem(selectedSpecieId, selectedItem));
            }

            var remainingSpots = 4 - items.Count;
            foreach (var i in species
                    .Where(j => favouriteSpecies.Contains(j.ID) && j.ID != selectedSpecieId)
                    .Take(remainingSpots))
            {
                items.Add(LogFactory.CreatePickerItem(selectedSpecieId, i));
            }

            return items;
        }

        public static PickerItem CreatePickerItem(string selectedSpecieId, Art art)
        {
            return new PickerItem
            {
                ID = art.ID,
                Title = art.Navn,
                Selected = art.ID == selectedSpecieId
            };
        }

		public static async Task<List<PickerItem>> CreatePickerHunters(string selectedJegerId)
		{
            var items = new List<PickerItem>();

            var hunters = await App.HunterDataStore.GetItemsAsync();

            var selectedItem = hunters.SingleOrDefault(i => i.ID == selectedJegerId);
            if (selectedItem != null)
            {
                items.Add(LogFactory.CreatePickerItem(selectedJegerId, selectedItem));
            }

            var remainingSpots = 4 - items.Count;
            foreach (var i in hunters
                     .Where(j => j.ID != selectedJegerId)
                    .Take(remainingSpots))
            {
                items.Add(LogFactory.CreatePickerItem(selectedJegerId, i));
            }

            return items;
		}

        public static PickerItem CreatePickerItem(string selectedId, Dog item)
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

        public static async Task<List<PickerItem>> CreatePickerDogs(string selectedDogId)
        {
            var items = new List<PickerItem>();

            var dogs = await App.DogDataStore.GetItemsAsync();

            var selectedItem = dogs.SingleOrDefault(i => i.ID == selectedDogId);
            if (selectedItem != null)
            {
                items.Add(LogFactory.CreatePickerItem(selectedDogId, selectedItem));
            }

            var remainingSpots = 4 - items.Count;
            foreach (var i in dogs
                     .Where(j => j.ID != selectedDogId)
                    .Take(remainingSpots))
            {
                items.Add(LogFactory.CreatePickerItem(selectedDogId, i));
            }

            return items;
        }

        public static PickerItem CreatePickerItem(string selectedId, Jeger jeger)
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
