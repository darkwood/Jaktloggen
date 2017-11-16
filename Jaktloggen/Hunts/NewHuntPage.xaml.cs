using System;

using Xamarin.Forms;

namespace Jaktloggen
{
    public partial class NewHuntPage : ContentPage
    {
        public Hunt Item { get; set; }

        public NewHuntPage()
        {
            InitializeComponent();

            Item = new Hunt
            {
                Location = "Location name",
                Notes = "This is an item note."
            };

            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "AddHunt", Item);
            await Navigation.PopToRootAsync();
        }
    }
}
