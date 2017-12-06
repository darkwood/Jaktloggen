using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as HuntViewModel;
            if (item == null)
                return;

            await Navigation.PushAsync(new HuntPage(item));
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HuntPage(new HuntViewModel(new Hunt(), Navigation)));
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if(viewModel.HuntItems.Count == 0)
                await viewModel.ExecuteLoadHuntItemsCommand();
        }

    }
}
