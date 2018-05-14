using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Jaktloggen.InputViews;
using Xamarin.Forms;

namespace Jaktloggen
{
    public class LogFieldImage : ILogField
    {
        private Task<Action> _saveAction { get; set; }
        public string FieldLabel => "Bilde";
        private LogViewModel _logViewModel { get; set; }
        ICommand TapCommand { get; set; }
        public INavigation Navigation { get; }
        string ILogField.FieldLabel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        ICommand ILogField.TapCommand { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public LogFieldImage(LogViewModel logViewModel, Task<Action> saveAction, INavigation navigation)
        {
            Navigation = navigation;
            _logViewModel = logViewModel;
            _saveAction = saveAction;
            TapCommand = new Command(async (arg) =>
            {
                InputImage page = new InputImage(FieldLabel, _logViewModel.ImageFilename, async obj =>
                {
                    _logViewModel.ImageFilename = obj.ImageFilename;
                    await _saveAction;
                });
                await Navigation.PushAsync(page);
                await page.Initialize(arg as string);
            });
        }
    
    }
}
