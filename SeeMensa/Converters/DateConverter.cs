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

namespace SeeMensa.Converters
{
    /// <summary>
    /// Converter class for a date.
    /// </summary>
    public class DateConverter : IValueConverter
    {
        /// <summary>
        /// Converts a date to the correct localized format.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return string.Format("{0:dd}. {0:MMMM}", value, value);
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
