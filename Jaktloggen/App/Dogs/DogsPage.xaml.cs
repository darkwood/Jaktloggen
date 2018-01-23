using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Jaktloggen.Services;

using Xamarin.Forms;
using Xamarin.Forms.Internals;

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
            await DeleteItem(item);
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
                IsBusy = true;
                try
                {
                    if (!string.IsNullOrWhiteSpace(item.ImageFilename))
                    {
                        FileService.Delete(item.ImageFilename);
                    }
                    
                    foreach (var log in App.LogDataStore.GetCachedItems().Where(x => x.DogId == item.ID).ToList())
                    {
                        log.DogId = string.Empty;
                        await App.LogDataStore.UpdateItemAsync(log);
                    }

                    foreach (var hunt in App.HuntDataStore.GetCachedItems().Where(x => x.DogIds.Contains(item.ID)).ToList())
                    {
                        hunt.DogIds.Remove(item.ID);
                        await App.HuntDataStore.UpdateItemAsync(hunt);
                    }

                    await App.DogDataStore.DeleteItemAsync(item.ID);
                    viewModel.Items.Remove(item);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    await DisplayAlert("Feil ved sletting", ex.Message, "OK da");
                }
                IsBusy = false;
            }
        }
    }
}
