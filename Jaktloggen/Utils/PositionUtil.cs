using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Plugin.Geolocator;
using Plugin.Permissions;
using Microsoft.AppCenter.Crashes;

namespace Jaktloggen
{
    public static class PositionUtil
    {
        public static async Task<Position> GetPosition(double desiredAccuracy = 50)
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50L;
            var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10), null);

            return new Position(position.Latitude, position.Longitude);
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
                    if (sted.IndexOf(Environment.NewLine) > 0) //removes line 2
                    {
                        sted = sted.Substring(0, sted.IndexOf(Environment.NewLine));
                    }
                    if (sted.Length > 5 && Regex.IsMatch(sted, "^\\d{4}[\" \"]")) //removes zipcode
                    {
                        sted = sted.Substring(5);
                    }
                }
            }
            catch (Exception)
            {
                //TODO: Log this and move on with life
            }
            return sted;
        }
    }
}

