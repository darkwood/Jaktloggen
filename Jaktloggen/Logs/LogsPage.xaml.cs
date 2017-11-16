using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Jaktloggen
{
    public partial class LogsPage : ContentPage
    {
        LogsViewModel viewModel;

        public LogsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new LogsViewModel();
        }

        public LogsPage(LogsViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Log;
            if (item == null)
                return;

            //await Navigation.PushAsync(new HuntDetailPage(new LogsViewModel(item)));

            // Manually deselect item
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new NewHuntPage());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}
