using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jaktloggen.Services;

using Xamarin.Forms;

namespace Jaktloggen
{
    public partial class CustomFieldsPage : ContentPage
    {
        CustomFieldsViewModel viewModel;

        public CustomFieldsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new CustomFieldsViewModel(Navigation);

        }

         void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as CustomField;
            if (item == null)
                return;
            
            //await Navigation.PushAsync(new CustomFieldPage(item));

            // Manually deselect item
            ItemsListView.SelectedItem = null;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.OnAppearing();
        }
    }
}
