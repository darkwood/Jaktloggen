using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace Jaktloggen
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "Om Jaktloggen";

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("http://www.jaktloggen.no")));
        }

        public ICommand OpenWebCommand { get; }
    }
}
