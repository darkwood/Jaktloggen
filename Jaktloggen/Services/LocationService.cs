using System;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;

namespace Jaktloggen.Services
{
    public static class LocationService
    {
        public static async Task<Position> GetPositionAsync()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 10L;
            var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10), null);
            return position;

        }
    }
}
