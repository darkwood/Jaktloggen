using System;

using Xamarin.Forms;

namespace Jaktloggen
{
    public partial class HuntDetailPage : ContentPage
    {
        HuntDetailViewModel viewModel;

        // Note - The Xamarin.Forms Previewer requires a default, parameterless constructor to render a page.
        public HuntDetailPage()
        {
            InitializeComponent();

            var item = new Hunt
            {
                Location = "Høylandet",
                Notes = "Høstjakta 2017 med gutta krutt. 3 dager i telt langt inne på vidda. Kødda. Kos på hytta som vanlig."
            };

            viewModel = new HuntDetailViewModel(item);
            BindingContext = viewModel;
        }

        public HuntDetailPage(HuntDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }
    }
}
