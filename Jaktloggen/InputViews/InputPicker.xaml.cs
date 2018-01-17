using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Jaktloggen.InputViews
{
    public partial class InputPicker : ContentPage
    {
        private Action<InputPicker> _callback { get; set; }
        public ObservableCollection<PickerItem> PickerItems { get; set; }

        public bool CanSelectMany { get; set; }

        public InputPicker(string title, List<PickerItem> pickerItems, Action<InputPicker> callback)
        {
            PickerItems = new ObservableCollection<PickerItem>();
            pickerItems.ForEach(p => PickerItems.Add(p));
            Title = title;
            _callback = callback;

            BindingContext = this;
            InitializeComponent();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e == null || e.SelectedItem == null) return;

            var item = ((PickerItem)e.SelectedItem);

            if(!CanSelectMany)
            {
                PickerItems.ForEach(p => p.Selected = false);
                item.Selected = true;
                await Done();
            } 
            else
            {
                item.Selected = !item.Selected;
            }

            ((ListView)sender).SelectedItem = null;
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

    public class PickerItem : BaseViewModel
    {
        public string Details { get; set; }
        public ImageSource ImageSource { get; set; }

        private bool _selected;
        public bool Selected {
            get { return _selected; }
            set { _selected = value; OnPropertyChanged(nameof(Selected)); OnPropertyChanged(nameof(Color)); }
        }

        public Color Color => Selected ? Color.Green : Color.Black;
    }
}
