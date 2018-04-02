using System;

using Xamarin.Forms;

namespace Jaktloggen
{
    public partial class StatsPage : ContentPage
    {
        StatsViewModel viewModel;

        public StatsPage()
        {
            InitializeComponent();
            viewModel = new StatsViewModel(Navigation);
            BindingContext = viewModel;
        }

		protected override async void OnAppearing()
		{
            base.OnAppearing();
            await viewModel.OnAppearing();
		}
	}
}
