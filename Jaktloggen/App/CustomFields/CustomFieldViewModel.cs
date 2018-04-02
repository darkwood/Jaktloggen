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
        public CustomField CustomField { get; }

        public CustomFieldViewModel(CustomField customField, INavigation navigation)
        {
            Navigation = navigation;
            CustomField = customField;
        }
    }
}
