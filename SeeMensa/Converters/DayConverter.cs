using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;
using System.Globalization;

namespace SeeMensa.Converters
{
    /// <summary>
    /// Converter class for a day.
    /// </summary>
    public class DayConverter : IValueConverter
    {
        /// <summary>
        /// Converts a DateTime into the correct localalized format for a day of week.
        /// The day now will be shown as "today" or "heute".
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DateTime day = (DateTime)value;

            CultureInfo ci = CultureInfo.CurrentCulture;

            if (DateTime.Now.Date == day.Date)
            {
                if (ci.TwoLetterISOLanguageName.Equals("de"))
                {
                    return "Heute";
                }
                else
                {
                    return "Today";
                }
            }
            return string.Format("{0:dddd}",value);
        }

        /// <summary>
        /// ConvertBack not supported.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
