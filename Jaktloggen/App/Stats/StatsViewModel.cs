using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Jaktloggen.InputViews;
using Jaktloggen.Utils;
using Xamarin.Forms;

namespace Jaktloggen
{
    public class StatsViewModel : BaseViewModel
    {
        private const string ALL_HUNTERS = "Alle";
        private IEnumerable<Logg> _logs = new List<Logg>();
        private IEnumerable<Logg> _filteredLogs = new List<Logg>();
        public ICommand PickHunterCommand { get; set; }
        public ICommand MapCommand { get; set; }

        private string _hunterId;
        public string HunterId 
        {
            get { return _hunterId; }
            set
            {
                _hunterId = value;
                Update();
            }
        }

        string _hunterName;
        public string HunterName
        {
            get { return _hunterName; }
            set { _hunterName = value; OnPropertyChanged(nameof(HunterName)); }
        }
        DateTime _dateFrom = DateTime.Today.AddYears(-1);
        public DateTime DateFrom
        {
            get { return _dateFrom; }
            set { _dateFrom = value; Update(); }
        }

        DateTime _dateTo = DateTime.Today;
        public DateTime DateTo
        {
            get { return _dateTo; }
            set { _dateTo = value; Update(); }
        }

        public StatsViewModel(INavigation navigation)
        {
            Navigation = navigation;
            PickHunterCommand = new Command(ShowHunterPicker);
            MapCommand = new Command(ShowMap);
            HunterName = ALL_HUNTERS;
        }

        private async void ShowMap(object obj)
        {
            var mapView = new MapPage();
            await Navigation.PushAsync(mapView);
        }

        private async void ShowHunterPicker(object obj)
        {
            var inputView = new InputPicker(
                        "Velg jeger",
                finished: page =>
                {
                    var selected = page.PickerItems.SingleOrDefault(p => p.Selected);
                    HunterId = selected?.ID;
                    HunterName = selected?.Title ?? ALL_HUNTERS;
                },
                populate: async () =>
                {
                    return await LogFactory.CreatePickerHunters(HunterId);
                });

            await Navigation.PushAsync(inputView);
        }

        public double Hits => _filteredLogs.Sum(l => l.Treff);
        public double Shots => _filteredLogs.Sum(l => l.Skudd);
        public double Observed => _filteredLogs.Sum(l => l.Sett);

        public string HitsPercent
        {
            get
            {
                if (Shots == 0) { return ""; }
                var hitrate = (Hits / Shots) * 100;
                return String.Format("{0:F1} %", hitrate);
            }
        }

        public async Task OnAppearing()
        {
            _logs = await App.LogDataStore.GetItemsAsync();

            if (_logs.Any())
            {
                DateFrom = _logs.Min(x => x.Dato);
                DateTo = _logs.Max(x => x.Dato);
            }
            Update();
        }

        private void Update()
        {
            


            _filteredLogs = _logs;
            if(!string.IsNullOrEmpty(_hunterId))
            {
                _filteredLogs = _filteredLogs.Where(l => l.JegerId == _hunterId);
            }
            if(DateFrom != null)
            {
                _filteredLogs = _filteredLogs.Where(l => l.Dato.Date >= DateFrom.Date);
            }
            if (DateTo != null)
            {
                _filteredLogs = _filteredLogs.Where(l => l.Dato.Date <= DateTo.Date);
            }
            OnPropertyChanged(string.Empty);
        }
    }
}
