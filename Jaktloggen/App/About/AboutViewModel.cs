using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace Jaktloggen
{
    public class AboutViewModel : BaseViewModel
    {
        void ImportData(object obj)
        {
        }


        public AboutViewModel()
        {
            Title = "Om Jaktloggen";

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("http://www.jaktloggen.no")));
            ImportDataCommand = new Command(ImportData);
        }

        public ICommand OpenWebCommand { get; }
        public ICommand ImportDataCommand { get; }
        public ICommand ExportDataCommand { get; }
    }
}
