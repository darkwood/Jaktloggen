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

            BindingContext = viewModel = new LogViewModel(new Log(), null);
        }

        public LogPage(LogViewModel viewModel)
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
