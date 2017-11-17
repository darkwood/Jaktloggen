using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Jaktloggen.Utils.Converters
{
    public class DateTimeToShortDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var date = value as DateTime?;
            if (date != null)
            {
                return date.Value.ToString("D", new CultureInfo("nb-no"));
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DateTime.Parse(value as string);
        }
    }
}
