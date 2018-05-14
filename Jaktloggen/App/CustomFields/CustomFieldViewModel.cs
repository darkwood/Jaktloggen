using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Jaktloggen.InputViews;
using Plugin.Geolocator;
using Xamarin.Forms;

namespace Jaktloggen
{
    public class CustomFieldViewModel : BaseViewModel
    {
        private LoggType _customField { get; }

        public string Name => _customField.Navn;
        public string Description => _customField.Beskrivelse;
        public string GroupId => _customField.GroupId;

        bool selected;
        public bool Selected
        {
            get => selected;
            set { selected = value; OnPropertyChanged(nameof(Selected)); }
        }

        public CustomFieldViewModel(LoggType customField, INavigation navigation)
        {
            Navigation = navigation;
            _customField = customField;
            ID = _customField.Key;
        }
    }
}
