using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Jaktloggen.InputViews
{
    public partial class InputDate : ContentPage
    {
        private DateTime? _value;
        public DateTime Value
        {
            get { return _value ?? DateTime.Today; }
            set
            {
                SetProperty(ref _value, value);
            }
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

        protected bool SetProperty<T>(ref T backingStore, T value,
                                      [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
