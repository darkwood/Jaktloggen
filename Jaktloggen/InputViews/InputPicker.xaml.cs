using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Jaktloggen.InputViews
{
    public partial class InputPicker : ContentPage
    {
        private string[] _selectedItems;
        public string[] SelectedItems
        {
            get => _selectedItems;
            set { _selectedItems = value; OnPropertyChanged(nameof(SelectedItems));}
        }

        private Action<InputPicker> _callback { get; set; }

        private string[] _items;
        public string[] Items
        {
            get => _items;

            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        public bool CanSelectMany { get; set; }

        public InputPicker(string title, string[] selectedItems, string[] items, Action<InputPicker> callback)
        {
            Items = items;
            Title = title;
            _callback = callback;
            _selectedItems = selectedItems;

            BindingContext = this;
            InitializeComponent();
        }

        async void Handle_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if(!CanSelectMany)
            {
                SelectedItems = new string[]{(string)e.SelectedItem};
                await Done();
            }
        }

        async void Done_Clicked(object sender, EventArgs e)
        {
            await Done();
        }

        private async Task Done()
        {
            _callback(this);
            await Navigation.PopAsync();
        }
    }
}
