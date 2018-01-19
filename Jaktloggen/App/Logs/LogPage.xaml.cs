using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Jaktloggen
{
    public partial class LogPage : ContentPage
    {
        LogViewModel viewModel;

        public LogPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new LogViewModel(null, new Logg(), null);
        }

        public LogPage(LogViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.OnAppearing();
        }
    }
}
