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
using SeeMensa.ViewModels;

namespace SeeMensa.Converters
{
    /// <summary>
    /// Converter class for a opening.
    /// </summary>
    public class OpeningConverter : IValueConverter
    {
        /// <summary>
        /// Converts an OpeningViewModel to: Mo - Fr (1 - 5)
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //OpeningViewModel ovm = value as OpeningViewModel;

            //if (ovm != null)
            //{
            //    DateTime monday = new DateTime();
            //    DateTime startDay = monday.AddDays(ovm.StartDay - 1);
            //    DateTime endDay = monday.AddDays(ovm.EndDay - 1);

            //    return string.Format("{0:ddd}", startDay)
            //         + string.Format("-{0:ddd}:", endDay);
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
