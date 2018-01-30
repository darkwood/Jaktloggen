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
            get { return _value ?? DateTime.Now; }
            set { _value = value; OnPropertyChanged(nameof(Value)); }
        }
        private Action<InputDate> _callback { get; set; }

        private bool _timeVisible;
        public bool TimeVisible
        {
            get { return _timeVisible; }
            set { _timeVisible = value; OnPropertyChanged(nameof(TimeVisible)); }
        }

        public InputDate(){InitializeComponent();}
        public InputDate(string title, DateTime value, Action<InputDate> callback, bool timeVisible = false)
        {
            Title = title;
            _callback = callback;
            _value = value;
            TimeVisible = timeVisible;

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

        void Date_Tapped(object sender, System.EventArgs e)
        {
            date.Focus();
        }

        void Time_Tapped(object sender, System.EventArgs e)
        {
            time.Focus();
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

            if(TimeVisible){
                time.Focus();
            } else {
                date.Focus();
            }

        }
    }
}
