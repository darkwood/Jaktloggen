using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jaktloggen.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Jaktloggen
{
    public partial class MapPage : ContentPage
    {
        public MapPage()
        {
            InitializeComponent();
            BindingContext = this;

        }

		protected override async void OnAppearing()
		{
            await AddPins();
            ZoomToShowAllPins();

            base.OnAppearing();
		}

		private async Task AddPins()
        {
            MyMap.Pins.Clear();

            var logs = await App.LogDataStore.GetItemsAsync();
            foreach (var log in logs)
            {
                if (!string.IsNullOrWhiteSpace(log.Latitude))
                {
                    MyMap.Pins.Add(new Pin
                    {
                        Position = new Position(double.Parse(log.Latitude), double.Parse(log.Longitude)),
                        Type = PinType.Generic,
                        Address = await LogFactory.CreateLogSummary(log),
                        Label = log.Dato.ToString("G")
                    });
                }
            }
        }

        protected void ZoomToShowAllPins()
        {
            if (MyMap.Pins.Count > 0)
            {
                var southWest = new Position(MyMap.Pins[0].Position.Latitude, MyMap.Pins[0].Position.Longitude);
                var northEast = southWest;

                foreach (var pin in MyMap.Pins)
                {
                    southWest = new Position(Math.Min(southWest.Latitude, pin.Position.Latitude),
                                             Math.Min(southWest.Longitude, pin.Position.Longitude)
                                            );

                    northEast = new Position(Math.Max(northEast.Latitude, pin.Position.Latitude),
                                             Math.Max(northEast.Longitude, pin.Position.Longitude)
                                            );

                }

                var spanLatitudeDelta = Math.Abs(northEast.Latitude - southWest.Latitude) * 2;
                var spanLongitudeDelta = Math.Abs(northEast.Longitude - southWest.Longitude) * 2;

                var regionCenterLatitude = (southWest.Latitude + northEast.Latitude) / 2;
                var regionCenterLongitude = (southWest.Longitude + northEast.Longitude) / 2;

                var radius = new Distance(Math.Max(spanLatitudeDelta, spanLongitudeDelta) * 20000);
                var center = new Position(regionCenterLatitude, regionCenterLongitude);

                MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(center, radius));
            }
        }
	}
}
