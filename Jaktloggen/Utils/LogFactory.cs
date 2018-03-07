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
            var favouriteSpecies = FileService.LoadFromLocalStorage<List<string>>(App.FILE_SELECTED_SPECIE_IDS);
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
