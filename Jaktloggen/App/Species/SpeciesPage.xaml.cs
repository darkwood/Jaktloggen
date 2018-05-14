using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jaktloggen.Services;

using Xamarin.Forms;

namespace Jaktloggen
{
    public partial class SpeciesPage : ContentPage
    {
        SpeciesViewModel viewModel;

        public SpeciesPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new SpeciesViewModel(Navigation);

            MessagingCenter.Subscribe<SpecieViewModel>(this, "Delete", async (item) =>
            {
				//await DeleteItem(item);
				await Task.Delay(500);
            });
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as SpecieViewModel;
            if (item == null)
                return;

            //await Navigation.PushAsync(new SpeciePage(item));
            item.Selected = !item.Selected;
            await viewModel.UpdateSelectedSpecie(item);
            // Manually deselect item
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SpeciePage(new SpecieViewModel(new Art(), Navigation)));
        }

        //async void OnDelete(object sender, EventArgs e)
        //{
        //    var mi = ((MenuItem)sender);
        //    var item = mi.CommandParameter as SpecieViewModel;
        //    var doit = await DisplayAlert("Bekreft sletting", "Art blir permanent slettet", "OK", "Avbryt");
        //    if (doit)
        //    {
        //        await viewModel.DeleteItem(item);
        //    }

        //}

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.OnAppearing();
        }

        //public async Task DeleteItem(SpecieViewModel item)
        //{
            //var doit = await DisplayAlert("Bekreft sletting", "Hund blir permanent slettet", "OK", "Avbryt");
            //if (doit)
            //{
            //    if (!string.IsNullOrWhiteSpace(item.ImageFilename))
            //    {
            //        FileService.Delete(item.ImageFilename);
            //    }
            //    //Todo: Remove from all logs and hunts
            //    await App.SpecieDataStore.DeleteItemAsync(item.ID);
            //    viewModel.Items.Remove(item);

            //    await Navigation.PopToRootAsync();
            //}
        //}
    }
}
