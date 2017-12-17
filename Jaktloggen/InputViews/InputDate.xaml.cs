using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Jaktloggen.InputViews
{
    public partial class InputDate : ContentPage, INotifyPropertyChanged
    {
        private DateTime? _value;
        public DateTime Value
        {
            get { return _value ?? DateTime.Today; }
            set { _value = value; OnPropertyChanged(nameof(Value)); }
        }
        private Action<InputDate> _callback { get; set; }

        public InputDate(){InitializeComponent();}
        public InputDate(string title, DateTime value, Action<InputDate> callback)
        {
            Title = title;
            _callback = callback;
            _value = value;
            BindingContext = this;
            InitializeComponent();
        }
        async void Done_Clicked(object sender, EventArgs e)
        {
            await Done();
        }

        public async Task Done()
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
