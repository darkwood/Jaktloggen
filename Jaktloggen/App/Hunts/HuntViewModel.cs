using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Jaktloggen.InputViews;
using Plugin.Geolocator;
using Xamarin.Forms;

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
                return string.IsNullOrEmpty(Latitude) ? "" : $"{Latitude}, {Longitude}";
            }
        }
        public string ImagePath { get; set; }

        string notes;
        public string Notes
        {
            get { return notes; }
            set { SetProperty(ref notes, value); }
        }

        string infoMessage;
        public string InfoMessage
        {
            get { return infoMessage; }
            set { SetProperty(ref infoMessage, value); }
        }
        public ICommand LocationCommand { protected set; get; }
        public ICommand DateFromCommand { protected set; get; }
        public ICommand DateToCommand { protected set; get; }
        public ICommand NotesCommand { protected set; get; }
        public ICommand DeleteCommand { protected set; get; }

        public HuntViewModel(Hunt item, INavigation navigation)
        {
            Item = item.DeepClone();
            Navigation = navigation;

            Title = Location; 
            CreateCommands(item);
        }

        public async Task OnAppearing()
        {
            if (string.IsNullOrEmpty(Item.ID))
            {
                DateFrom = DateTime.Today;
                DateTo = DateTime.Today;

                await SetPositionAsync();
            }
        }

        private async Task SetPositionAsync()
        {
            InfoMessage = "Henter din posisjon...";

            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 20L;
            var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10), null);

            InfoMessage = "";

            Latitude = position.Latitude.ToString();
            Longitude = position.Longitude.ToString();

            OnPropertyChanged(nameof(PositionInfo));
        }

        private void CreateCommands(Hunt item)
        {
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
