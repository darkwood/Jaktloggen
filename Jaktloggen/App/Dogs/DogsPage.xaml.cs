using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jaktloggen.Services;

using Xamarin.Forms;

namespace Jaktloggen
{
    public partial class DogsPage : ContentPage
    {
        DogsViewModel viewModel;

        public DogsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new DogsViewModel(Navigation);

            MessagingCenter.Subscribe<DogViewModel>(this, "Delete", async (item) =>
            {
                await DeleteItem(item);

            });
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as DogViewModel;
            if (item == null)
                return;
            
            await Navigation.PushAsync(new DogPage(item));

            // Manually deselect item
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DogPage(new DogViewModel(new Dog(), Navigation)));
        }

        async void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var item = mi.CommandParameter as DogViewModel;
            var doit = await DisplayAlert("Bekreft sletting", "Jeger blir permanent slettet", "OK", "Avbryt");
            if (doit)
            {
                FileService.Delete(item.ImageFilename);
                await viewModel.DeleteItem(item);
            }

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.OnAppearing();
        }

        public async Task DeleteItem(DogViewModel item)
        {
            var doit = await DisplayAlert("Bekreft sletting", "Hund blir permanent slettet", "OK", "Avbryt");
            if (doit)
            {
                if (!string.IsNullOrWhiteSpace(item.ImageFilename))
                {
                    FileService.Delete(item.ImageFilename);
                }
                //Todo: Remove from all logs and hunts
                await App.DogDataStore.DeleteItemAsync(item.ID);
                viewModel.Items.Remove(item);

                await Navigation.PopToRootAsync();
            }
        }
    }
}
