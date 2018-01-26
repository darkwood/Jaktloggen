using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Jaktloggen.InputViews
{
    public partial class InputPicker : ContentPage
    {
        private Action<InputPicker> _finished { get; set; }
        public ObservableCollection<PickerItem> PickerItems { get; set; }

        public bool CanSelectMany { get; set; }
        public Func<Task<List<PickerItem>>> Populate { get; }
        public InputPicker(string title, Action<InputPicker> finished, Func<Task<List<PickerItem>>> populate)
        {
            Populate = populate;
            Title = title;
            _finished = finished;
            PickerItems = new ObservableCollection<PickerItem>();

            BindingContext = this;
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            await LoadItems();
            base.OnAppearing();
        }
        private async Task LoadItems()
        {
            var items = await Populate();
            PickerItems.Clear();
            items.ForEach(p => PickerItems.Add(p));
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
            _finished(this);
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
