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
            pickerItems?.ForEach(p => PickerItems.Add(p));
            Title = title;
            _callback = callback;

            BindingContext = this;
            InitializeComponent();
        }

        async void Handle_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = ((PickerItem)e.SelectedItem);
            item.Selected = !item.Selected;

            if(!CanSelectMany)
            {
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

    public class PickerItem
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public ImageSource ImageSource { get; set; }
        public bool Selected { get; set; }
    }
}
