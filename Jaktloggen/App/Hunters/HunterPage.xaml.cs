﻿using System;
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

            BindingContext = viewModel = new HunterViewModel(new Jeger(), null);
        }

        public HunterPage(HunterViewModel viewModel)
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
