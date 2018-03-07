using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

using Jaktloggen.InputViews;
using Jaktloggen.Models;
using Plugin.Geolocator;

using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Jaktloggen
{
    public class HuntViewModel : BaseViewModel
    {
        public Jakt Item 
        { 
            get; 
            set; 
        }

        public override string ID
        {
            get { return Item.ID; }
            set { Item.ID = value; OnPropertyChanged(nameof(ID)); }
        }

        public string Location
        {
            get { return Item.Sted; }
            set { Item.Sted = value; OnPropertyChanged(nameof(Location)); }
        }

        public DateTime DateFrom
        {
            get { return Item.DatoFra; }
            set { Item.DatoFra = value; OnPropertyChanged(nameof(DateFrom)); }
        }

        public DateTime DateTo
        {
            get { return Item.DatoTil; }
            set { Item.DatoTil = value; OnPropertyChanged(nameof(DateTo)); }
        }

        public List<string> HunterIds
        {
            get { return Item.JegerIds ?? new List<string>(); }
            set { Item.JegerIds = value; OnPropertyChanged(nameof(HunterIds)); OnPropertyChanged(nameof(HuntersText));}
        }
        
        public List<string> DogIds 
        { 
            get{ return Item.DogIds ?? new List<string>(); } 
            set { Item.DogIds = value; OnPropertyChanged(nameof(DogIds)); OnPropertyChanged(nameof(DogsText));} 
        }

        public double Latitude
        {
            get {
                double lat = 0;
                double.TryParse(Item.Latitude, out lat);
                return lat; 
            }
            set { 
                Item.Latitude = value.ToString(); 
                OnPropertyChanged(nameof(Latitude)); 
                OnPropertyChanged(nameof(PositionInfo));
            }
        }

        public double Longitude
        {
            get { 
                double lon = 0;
                double.TryParse(Item.Longitude, out lon);
                return lon;
            }
            set { 
                Item.Longitude = value.ToString(); 
                OnPropertyChanged(nameof(Longitude));
                OnPropertyChanged(nameof(PositionInfo));
            }
        }
        public string PositionInfo
        {
            get
            {
                if(!string.IsNullOrEmpty(InfoMessage)){
                    return InfoMessage;
                }
                return Latitude <= 0 ? "" : $"{Latitude}, {Longitude}";
            }
        }
        public string ImageFilename 
        {
            //get => Utility.GetImageFileName(Item.ImagePath);
            get => Item.ImagePath;
            set { 
                Item.ImagePath = value; 
                OnPropertyChanged(nameof(ImageFilename)); 
                OnPropertyChanged(nameof(Image)); 
            }
        }

        public ImageSource Image
        {
            get
            {
                return Utility.GetImageSource(ImageFilename);
            }
        }

        public string Notes
        {
            get { return Item.Notes; }
            set { Item.Notes = value; OnPropertyChanged(nameof(Notes)); Save();}
        }

        string infoMessage;
        public string InfoMessage
        {
            get { return infoMessage; }
            set { SetProperty(ref infoMessage, value); OnPropertyChanged(nameof(PositionInfo)); }
        }

        List<Logg> _logs = new List<Logg>();
        public List<Logg> Logs
        {
            get => _logs;
            set {
                _logs = value; 
                OnPropertyChanged(nameof(Logs));
                OnPropertyChanged(nameof(LogsText));
            }
        }

        List<Jeger> _hunters = new List<Jeger>();
        public List<Jeger> Hunters
        {
            get => _hunters;
            set
            {
                _hunters = value;
                OnPropertyChanged(nameof(Hunters));
                OnPropertyChanged(nameof(HuntersText));
            }
        }

        List<Dog> _dogs = new List<Dog>();
        public List<Dog> Dogs
        {
            get => _dogs;
            set
            {
                _dogs = value;
                OnPropertyChanged(nameof(Dogs));
                OnPropertyChanged(nameof(DogsText));
            }
        }

        public string LogsText => $"{Logs.Count} loggføringer";
        public string HuntersText => string.Join(", ", Hunters.Where(s => HunterIds.Contains(s.ID)).Select(h => h.Fornavn));
        public string DogsText => string.Join(", ", Dogs.Where(s => DogIds.Contains(s.ID)).Select(h => h.Navn));

        public ICommand LocationCommand { protected set; get; }
        public ICommand PositionCommand { protected set; get; }
        public ICommand ImageCommand { protected set; get; }
        public ICommand DateFromCommand { protected set; get; }
        public ICommand DateToCommand { protected set; get; }
        public ICommand NotesCommand { protected set; get; }
        public ICommand HuntersCommand { protected set; get; }
        public ICommand DogsCommand { protected set; get; }
        public ICommand DeleteCommand { protected set; get; }
        public ICommand LogsCommand { protected set; get; }
        public ICommand NewLogCommand { protected set; get; }

        private bool isLoaded { get; set; }

        public HuntViewModel(Jakt item, INavigation navigation)
        {
            Item = item.DeepClone();
            Navigation = navigation;

            //Title = string.IsNullOrEmpty(item.ID) ? "Ny jakt" : Location; 
            CreateCommands(item);
        }

        public async Task OnAppearing()
        {   
            if (string.IsNullOrEmpty(Item.ID) && !isLoaded)
            {
                DateFrom = DateTime.Today;
                DateTo = DateTime.Today;

                await SetPositionAsync();
            }

            var allLogs = await App.LogDataStore.GetItemsAsync();
            Logs = allLogs.Where(l => l.JaktId == ID).ToList();
            Hunters = await App.HunterDataStore.GetItemsAsync();
            Dogs = await App.DogDataStore.GetItemsAsync();
            
            isLoaded = true;
        }
        
        private async Task SetPositionAsync()
        {
            InfoMessage = "Henter din posisjon...";
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 10L;
                var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10), null);

                InfoMessage = "";

                Latitude = position.Latitude;
                Longitude = position.Longitude;

                OnPropertyChanged(nameof(PositionInfo));

                Location = await GetLocationNameForPosition(position.Latitude, position.Longitude);
            }
            catch(Exception ex)
            {
                //todo: log this
                throw new Exception(ex.Message, ex);
            }
        }

        public static async Task<string> GetLocationNameForPosition(double latitude, double longitude)
        {
            string sted = string.Empty;
            try
            {
                var geoCoder = new Geocoder();
                var geoPos = new Position(latitude, longitude);
                var possibleAddresses = await geoCoder.GetAddressesForPositionAsync(geoPos);
                if (possibleAddresses.Any())
                {
                    sted = possibleAddresses.First();
                    int newLinePos = sted.IndexOf(Environment.NewLine, StringComparison.CurrentCultureIgnoreCase);
                    if (newLinePos > 0) //removes line 2
                    {
                        sted = sted.Substring(0, newLinePos);
                    }
                    if (sted.Length > 5 && Regex.IsMatch(sted, "^\\d{4}[\" \"]")) //removes zipcode
                    {
                        sted = sted.Substring(5);
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO: Log this
                throw new Exception(ex.Message, ex);
            }
            return sted;
        }

        private void CreateCommands(Jakt item)
        {
            ImageCommand = new Command(async () =>
            {
                await Navigation.PushAsync(new InputImage("Bilde", ImageFilename, async obj => {
                    ImageFilename = obj.ImageFilename;
                    await SaveAsync();
                }));
            });

            LocationCommand = new Command(async () =>
            {
                await Navigation.PushAsync(new InputEntry("Sted", Location, async obj => {
                    Location = obj.Value;
                    await SaveAsync();
                }));
            });

            PositionCommand = new Command(async () =>
            {
                await Navigation.PushAsync(new InputPosition(Latitude, Longitude, async obj => {
                    Latitude = obj.Latitude;
                    Longitude = obj.Longitude;
                    await SaveAsync();
                }));
            });

            DateFromCommand = new Command(async () =>
            {
                await Navigation.PushAsync(new InputDate("Dato fra", DateFrom, async obj => {
                    DateFrom = obj.Value;
                    if (DateTo < DateFrom){
                        DateTo = DateFrom;
                    }
                    await SaveAsync();
                }));
            });

            DateToCommand = new Command(async () =>
            {
                await Navigation.PushAsync(new InputDate("Dato til", DateTo, async obj =>
                {
                    DateTo = obj.Value;
                    if (DateFrom > DateTo)
                    {
                        DateFrom = DateTo;
                    }

                    await SaveAsync();
                }));
            });

            HuntersCommand = new Command(async () =>
            {
                var inputView = new InputPicker(
                    "Velg jegere", 
                    async obj =>
                    {
                        HunterIds = obj.PickerItems.Where(p => p.Selected).Select(s => s.ID).ToList();
                        await SaveAsync();
                    }, async () => {

                        Hunters = await App.HunterDataStore.GetItemsAsync();
                        var huntersVM = Hunters.Select(h => new HunterViewModel(h, Navigation));
                        var items = huntersVM.Select(h => new PickerItem
                        {
                            ID = h.ID,
                            Title = h.Name,
                            ImageSource = h.Image,
                            Selected = HunterIds.Contains(h.ID)
                        }).ToList();
                        return items;
                        }
                    );
                inputView.CanSelectMany = true;

                await Navigation.PushAsync(inputView);
            });

            DogsCommand = new Command(async () =>
            {
                var inputView = new InputPicker(
                    "Velg hunder",
                    async obj =>
                    {
                        DogIds = obj.PickerItems.Where(p => p.Selected).Select(s => s.ID).ToList();
                        await SaveAsync();
                }, async () => {

                        Dogs = await App.DogDataStore.GetItemsAsync();
                        var dogsVM = Dogs.Select(h => new DogViewModel(h, Navigation));
                        var items = dogsVM.Select(h => new PickerItem
                        {
                            ID = h.ID,
                            Title = h.Name,
                            ImageSource = h.Image,
                            Selected = DogIds.Contains(h.ID)
                        }).ToList();
                        return items;
                    }
                    );
                inputView.CanSelectMany = true;

                await Navigation.PushAsync(inputView);
            });

            NotesCommand = new Command(async () =>
            {
                var inputView = new InputEntry("Notater", Notes, async obj =>
                {
                    Notes = obj.Value;
                    await SaveAsync();
                });
                inputView.Multiline = true;
                await Navigation.PushAsync(inputView);
            });

            LogsCommand = new Command(async () =>
            {
                await Navigation.PushAsync(new LogsPage(new LogsViewModel(this, Navigation)));
            });

            NewLogCommand = new Command(async () =>
            {
                if (string.IsNullOrWhiteSpace(ID))
                {
                    await SaveAsync();
                }

                await Navigation.PushAsync(
                    new LogPage(
                        new LogViewModel(
                            this, new Logg(), Navigation)));
            });

            DeleteCommand = new Command( () => {
                MessagingCenter.Send(this, "Delete");
            });
        }

        void Save()
        {
            Task.Run(SaveAsync);
        }

        async Task SaveAsync()
        {
            if (!string.IsNullOrEmpty(ID))
            {
                await App.HuntDataStore.UpdateItemAsync(Item);
            }
            else
            {
                await App.HuntDataStore.AddItemAsync(Item);
            }
        }
    }
}
