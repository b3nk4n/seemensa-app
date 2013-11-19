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
    /// Converter class for a time range of an OpeningViewModel.
    /// </summary>
    public class TimeRangeConverter : IValueConverter
    {
        /// <summary>
        /// Converts an OpeningViewModel to: 11:00 - 13:45
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //OpeningViewModel ovm = value as OpeningViewModel;

            //if (ovm != null)
            //{
            //    return string.Format("{0:t}", ovm.StartTime)
            //         + string.Format(" - {0:t}", ovm.EndTime);
            //}

            return string.Empty;
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
