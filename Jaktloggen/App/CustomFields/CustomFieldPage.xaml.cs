using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Jaktloggen
{
    public partial class CustomFieldPage : ContentPage
    {
        CustomFieldViewModel viewModel;

        public CustomFieldPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new CustomFieldViewModel(new CustomField(), null);
        }

        public CustomFieldPage(CustomFieldViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "Save", viewModel);
            await Navigation.PopAsync();
        }
    }
}
