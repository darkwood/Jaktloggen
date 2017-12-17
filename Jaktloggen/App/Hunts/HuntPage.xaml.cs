using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Jaktloggen
{
    public partial class HuntPage : ContentPage
    {
        HuntViewModel viewModel;

        // Note - The Xamarin.Forms Previewer requires a default, parameterless constructor to render a page.
        public HuntPage()
        {
            InitializeComponent();

            var item = new Hunt
            {
                ID = "1",
                Sted = "Høylandet",
                Notes = "Høstjakta 2017 med gutta krutt. 3 dager i telt langt inne på vidda. Kødda. Kos på hytta som vanlig."
            };

            viewModel = new HuntViewModel(item, Navigation);
            BindingContext = viewModel;
        }

        public HuntPage(HuntViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }
        
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            await viewModel.OnAppearing();
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "Save", viewModel);
            
            await Navigation.PopAsync();
        }
    }
}
