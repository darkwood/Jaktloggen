using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jaktloggen.Services;

using Xamarin.Forms;

namespace Jaktloggen
{
    public partial class HuntersPage : ContentPage
    {
        HuntersViewModel viewModel;

        public HuntersPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new HuntersViewModel(Navigation);

            MessagingCenter.Subscribe<HunterViewModel>(this, "Delete", async (item) =>
            {
                await DeleteItem(item);

            });
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as HunterViewModel;
            if (item == null)
                return;
            
            await Navigation.PushAsync(new HunterPage(item));

            // Manually deselect item
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HunterPage(new HunterViewModel(new Jeger(), Navigation)));
        }

        async void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var item = mi.CommandParameter as HunterViewModel;
            await DeleteItem(item);
        }
        public async Task DeleteItem(HunterViewModel item)
        {
            var doit = await DisplayAlert("Bekreft sletting", "Jeger blir permanent slettet", "OK", "Avbryt");
            if (doit)
            {
                if (!string.IsNullOrWhiteSpace(item.ImageFilename))
                {
                    FileService.Delete(item.ImageFilename);
                }
                //Todo: Remove from all logs and hunts
                await App.HunterDataStore.DeleteItemAsync(item.ID);
                viewModel.Items.Remove(item);
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.OnAppearing();
        }
    }
}
