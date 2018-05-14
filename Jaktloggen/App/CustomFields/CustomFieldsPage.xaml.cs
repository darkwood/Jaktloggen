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
            var item = args.SelectedItem as CustomFieldViewModel;
            if (item == null)
                return;

            item.Selected = !item.Selected;
            viewModel.UpdateItem(item);
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
