using System;
using System.Collections.Generic;

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

            BindingContext = viewModel = new HuntersViewModel();
        }

        public HuntersPage(HuntersViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Hunter;
            if (item == null)
                return;
            
            await Navigation.PushAsync(new HunterPage(new HunterViewModel(item, Navigation)));

            // Manually deselect item
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HunterPage(new HunterViewModel(new Hunter(), Navigation)));
        }

        async void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var item = mi.CommandParameter as HunterViewModel;
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

            if (viewModel.Items.Count == 0)
                await viewModel.ExecuteLoadItemsCommand();
        }
    }
}
