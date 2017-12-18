using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Jaktloggen
{
    public partial class HunterPage : ContentPage
    {
        HunterViewModel viewModel;

        public HunterPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new HunterViewModel(new Hunter(), null);
        }

        public HunterPage(HunterViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

        }
    }
}
