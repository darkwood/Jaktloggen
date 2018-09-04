using System;

using Xamarin.Forms;

namespace Jaktloggen.Controls
{
    public class HeaderImage : ContentPage
    {
        public HeaderImage()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Hello ContentPage" }
                }
            };
        }
    }
}

