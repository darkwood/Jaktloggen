using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Jaktloggen.InputViews
{
    public class InputDateViewModel : BaseViewModel
    {
        private DateTime? _value;
        public DateTime Value
        {
            get { return _value ?? DateTime.Today; }
            set { SetProperty(ref _value, value); }
        }
        public InputDateViewModel(DateTime date)
        {
            _value = Value;
        }
    }
    public partial class InputDate : ContentPage
    {
        public InputDateViewModel VM
        {
            get;
            set;
        }
        private Action<InputDate> _callback { get; set; }

        public InputDate(){InitializeComponent();}
        public InputDate(string title, DateTime value, Action<InputDate> callback)
        {
            Title = title;
            _callback = callback;
            BindingContext = VM = new InputDateViewModel(value);
            InitializeComponent();
        }
        async void Done_Clicked(object sender, EventArgs e)
        {
            _callback(this);
            await Navigation.PopAsync();
        }

        void Handle_Tapped(object sender, System.EventArgs e)
        {
            date.Focus();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            date.Focus();

        }
    }
}
