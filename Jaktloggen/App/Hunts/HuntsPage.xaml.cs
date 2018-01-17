using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jaktloggen.Services;
using Xamarin.Forms;

namespace Jaktloggen
{
    public partial class HuntsPage : ContentPage
    {
        HuntsViewModel viewModel;

        public HuntsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new HuntsViewModel(Navigation);

            MessagingCenter.Subscribe<HuntViewModel>(this, "Delete", async (item) =>
            {
                await DeleteItem(item);

            });
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as HuntViewModel;
            if (item == null)
                return;

            await Navigation.PushAsync(new HuntPage(item));
            ItemsListView.SelectedItem = null;
        }

        async void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var item = mi.CommandParameter as HuntViewModel;
            await DeleteItem(item);
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HuntPage(new HuntViewModel(new Hunt(), Navigation)));
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.OnAppearing();
        }


        public async Task DeleteItem(HuntViewModel item)
        {
            var doit = await DisplayAlert("Bekreft sletting", "Jakta blir permanent slettet", "OK", "Avbryt");
            if (doit)
            {
                if(!string.IsNullOrWhiteSpace(item.ImageFilename))
                {
                    FileService.Delete(item.ImageFilename);
                }
                await App.HuntDataStore.DeleteItemAsync(item.ID);
                viewModel.HuntItems.Remove(item);

                await Navigation.PopToRootAsync();
            }
        }
    }
}
