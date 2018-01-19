using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Jaktloggen
{
    public partial class SpeciePage : ContentPage
    {
        SpecieViewModel viewModel;

        public SpeciePage()
        {
            InitializeComponent();

            BindingContext = viewModel = new SpecieViewModel(new Art(), null);
        }

        public SpeciePage(SpecieViewModel viewModel)
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
