using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jaktloggen.Services;
using Xamarin.Forms;

namespace Jaktloggen
{
    public partial class LogsPage : ContentPage
    {
        LogsViewModel viewModel;

        public LogsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new LogsViewModel(null, Navigation);
            viewModel.Items.Add(new LogViewModel(null, new Logg{
                Dato = DateTime.Now,
                Notes = "Lorem ipsum"
            }, null));
        }

        public LogsPage(LogsViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;

            MessagingCenter.Subscribe<LogViewModel>(this, "Delete", async (item) =>
            {
                await DeleteItem(item);

            });
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as LogViewModel;
            if (item == null)
                return;

            await Navigation.PushAsync(new LogPage(item));
            ItemsListView.SelectedItem = null;
        }

        async void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var item = mi.CommandParameter as LogViewModel;
            await DeleteItem(item);
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Utility.AnimateButton(sender as VisualElement);
            await viewModel.ShowNewItem();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.OnAppearing();
        }


        public async Task DeleteItem(LogViewModel item)
        {
            var doit = await DisplayAlert("Bekreft sletting", "Loggføringen blir permanent slettet", "OK", "Avbryt");
            if (doit)
            {
                if (!string.IsNullOrWhiteSpace(item.ImageFilename))
                {
                    FileService.Delete(item.ImageFilename);
                }
                await App.LogDataStore.DeleteItemAsync(item.ID);
                viewModel.Items.Remove(item);

                //await Navigation.PopAsync();
            }
        }
    }
}
