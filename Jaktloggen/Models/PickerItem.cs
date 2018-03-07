using Xamarin.Forms;

namespace Jaktloggen.Models
{
    public class PickerItem : BaseViewModel
    {
        public string Details { get; set; }
        public ImageSource ImageSource { get; set; }

        private bool _selected;
        public bool Selected
        {
            get { return _selected; }
            set { _selected = value; OnPropertyChanged(nameof(Selected)); OnPropertyChanged(nameof(Color)); }
        }

        public Color Color => Selected ? Color.Green : Color.Black;
    }
}
