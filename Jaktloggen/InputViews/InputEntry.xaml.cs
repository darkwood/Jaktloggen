using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Jaktloggen.InputViews
{
    public partial class InputEntry : ContentPage
    {
        private string _value;
        public string Value
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_value))
                {
                    return string.Empty;
                }
                var arr = _value.ToCharArray();
                arr[0] = Char.ToUpperInvariant(arr[0]);
                return new String(arr);

            }
            set { _value = value; }
        }

        private Action<InputEntry> _callback { get; set; }
        public bool Multiline { get; set; }

        public InputEntry(string title, string value, Action<InputEntry> callback)
        {
            Title = title;
            _callback = callback;
            _value = value;
            BindingContext = this;
            InitializeComponent();
        }
        async void Done_Clicked(object sender, EventArgs e)
        {
            _callback(this);
            await Navigation.PopAsync();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            entry.Focus();

        }
    }
}
