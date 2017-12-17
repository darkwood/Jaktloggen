using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Jaktloggen.InputViews;
using Jaktloggen.Interfaces;
using Jaktloggen.Services;
using Plugin.Geolocator;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Jaktloggen
{
    public class HuntViewModel : BaseViewModel
    {
        public Hunt Item 
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

        public List<int> HunterIds
        {
            get { return Item.JegerIds; }
            set { Item.JegerIds = value; OnPropertyChanged(nameof(HunterIds)); }
        }
        
        public List<int> DogIds 
        { 
            get{ return Item.DogIds; } 
            set { Item.DogIds = value; OnPropertyChanged(nameof(DogIds)); } 
        }

        public string Latitude
        {
            get { return Item.Latitude; }
            set { Item.Latitude = value; OnPropertyChanged(nameof(Latitude)); }
        }

        public string Longitude
        {
            get { return Item.Longitude; }
            set { Item.Longitude = value; OnPropertyChanged(nameof(Longitude)); }
        }
        public string PositionInfo
        {
            get
            {
                if(!string.IsNullOrEmpty(InfoMessage)){
                    return InfoMessage;
                }
                return string.IsNullOrEmpty(Latitude) ? "" : $"{Latitude}, {Longitude}";
            }
        }
        public string ImageFilename 
        {
            get {
                var f = Item.ImagePath;
                if(!string.IsNullOrEmpty(f) && f.Contains("/")){
                    f = f.Substring(f.LastIndexOf("/", StringComparison.CurrentCulture)+1);
                }
                return f; 
            }
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

        string notes;
        public string Notes
        {
            get { return notes; }
            set { SetProperty(ref notes, value); OnPropertyChanged(nameof(Notes)); }
        }

        string infoMessage;
        public string InfoMessage
        {
            get { return infoMessage; }
            set { SetProperty(ref infoMessage, value); OnPropertyChanged(nameof(PositionInfo)); }
        }
        public ICommand LocationCommand { protected set; get; }
        public ICommand ImageCommand { protected set; get; }
        public ICommand DateFromCommand { protected set; get; }
        public ICommand DateToCommand { protected set; get; }
        public ICommand NotesCommand { protected set; get; }
        public ICommand HuntersCommand { protected set; get; }
        public ICommand DeleteCommand { protected set; get; }

        private bool isLoaded { get; set; }

        public HuntViewModel(Hunt item, INavigation navigation)
        {
            Item = item.DeepClone();
            Navigation = navigation;

            Title = string.IsNullOrEmpty(item.ID) ? "Ny jakt" : Location; 
            CreateCommands(item);
        }

        public async Task OnAppearing()
        {
            if (string.IsNullOrEmpty(Item.ID) && !isLoaded)
            {
                DateFrom = DateTime.Today;
                DateTo = DateTime.Today;

                await SetPositionAsync();

                isLoaded = true;
            }
        }
        
        private async Task SetPositionAsync()
        {
            InfoMessage = "Henter din posisjon...";
            try
            {
                await Task.Delay(3000);
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 10L;
                var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10), null);

                InfoMessage = "";

                Latitude = position.Latitude.ToString();
                Longitude = position.Longitude.ToString();

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
                var geoPos = new Xamarin.Forms.Maps.Position(latitude, longitude);
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

        private void CreateCommands(Hunt item)
        {
            ImageCommand = new Command(async () =>
            {
                await Navigation.PushAsync(new InputImage("Bilde", ImageFilename, (InputImage obj) => {
                    ImageFilename = obj.ImageFilename;
                }));
            });

            LocationCommand = new Command(async () =>
            {
                await Navigation.PushAsync(new InputEntry("Sted", Location, (InputEntry obj) => {
                    Location = obj.Value;
                }));
            });

            DateFromCommand = new Command(async () =>
            {
                await Navigation.PushAsync(new InputDate("Dato fra", DateFrom, (InputDate obj) => {
                    DateFrom = obj.Value;
                    if (DateTo == null || DateTo < DateFrom){
                        DateTo = DateFrom;
                    }
                }));
            });

            DateToCommand = new Command(async () =>
            {
                await Navigation.PushAsync(new InputDate("Dato til", DateTo, (InputDate obj) => {
                    DateTo = obj.Value;
                    if (DateFrom == null || DateFrom > DateTo)
                    {
                        DateFrom = DateTo;
                    }
                }));
            });

            HuntersCommand = new Command(async () =>
            {
                var items = new[] { "1", "2", "3", "4" };
                var inputView = new InputPicker(
                    "Velg jegere", 
                    HunterIds?.Select(h => h.ToString()).ToArray(), 
                    items, 
                    (InputPicker obj) =>
                {
                    HunterIds = obj.SelectedItems.Select(i => int.Parse(i)).ToList();
                });

                await Navigation.PushAsync(inputView);
            });

            NotesCommand = new Command(async () =>
            {
                var inputView = new InputEntry("Notater", Notes, (InputEntry obj) =>
                {
                    Notes = obj.Value;
                });
                inputView.Multiline = true;
                await Navigation.PushAsync(inputView);
            });
        }
    }
}
