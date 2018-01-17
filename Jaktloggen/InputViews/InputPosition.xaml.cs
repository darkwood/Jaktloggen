using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Jaktloggen.InputViews
{
    public partial class InputPosition : ContentPage
    {
        public Position Position => new Position(Latitude, Longitude);
        public Action<InputPosition> Callback { get; }

        double latitude;
        public double Latitude
        {
            get
            {
                return latitude;
            }

            set
            {
                latitude = value;
                OnPropertyChanged(nameof(Latitude));
                OnPropertyChanged(nameof(PositionText));
            }
        }

        double longitude;
        public double Longitude
        {
            get
            {
                return longitude;
            }

            set
            {
                longitude = value;
                OnPropertyChanged(nameof(Longitude));
                OnPropertyChanged(nameof(PositionText));
            }
        }

        public string PositionText => Latitude > 0 ? $"{Latitude}, {longitude}" : string.Empty;

        bool loading;
        public bool Loading
        {
            get{ return loading; }
            set{ loading = value; OnPropertyChanged(nameof(Loading)); }
        }
        public InputPosition(){
            Longitude = 73.234234234;
            Latitude = 134.323423424;
            Loading = true;
            BindingContext = this;
            InitializeComponent();
        }
        public InputPosition(double latitude, double longitude, Action<InputPosition> callback)
        {
            Longitude = longitude;
            Latitude = latitude;
            Callback = callback;

            BindingContext = this;
            InitializeComponent();

            SetMapPosition();
            MyMap.Tap += MyMap_Tap;
        }

        private void SetMapPosition()
        {
            var distance = MyMap.VisibleRegion == null ? Distance.FromMiles(1) : MyMap.VisibleRegion.Radius;

            MyMap.MoveToRegion(
                MapSpan.FromCenterAndRadius(
                    Position, distance));

            var pin = new Pin()
            {
                Position = Position,
                Label = "Valgt posisjon", 
                Type=PinType.SearchResult
            };
            MyMap.Pins.Clear();
            MyMap.Pins.Add(pin);
        }

        void MyMap_Tap(object sender, Controls.TapEventArgs e)
        {
            Latitude = e.Position.Latitude;
            Longitude = e.Position.Longitude;
            SetMapPosition();
        }

        async void OnGetPositionClicked(object sender, System.EventArgs e)
        {
            Loading = true;
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 10L;
            var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10), null);
            Latitude = position.Latitude;
            Longitude = position.Longitude;
            SetMapPosition();
            Loading = false;
        }

        void OnSetPositionClicked(object sender, System.EventArgs e)
        {
            var pin = new Pin()
            {
                Position = Position,
                Label = "Ny posisjon valgt",
                Type = PinType.SavedPin
            };
            MyMap.Pins.Clear();
            MyMap.Pins.Add(pin);
        }

        void OnDeletePositionClicked(object sender, EventArgs e)
        {
            Latitude = Longitude = 0;
        }

        async void Done_Clicked(object sender, EventArgs e)
        {
            Callback(this);
            await Navigation.PopAsync();
        }
    }
}
