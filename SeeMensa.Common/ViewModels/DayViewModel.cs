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
using System.Xml.Linq;
using System.Collections.ObjectModel;
using PhoneKit.Framework.MVVM;

namespace SeeMensa.Common.ViewModels
{
    public class DayViewModel : ViewModelBase
    {
        #region Members

        /// <summary>
        /// The day.
        /// </summary>
        private DateTime _day;

        /// <summary>
        /// The meals of the day.
        /// </summary>
        public ObservableCollection<MealViewModel> Meals { get; private set; }

        /// <summary>
        /// Indicates whether the day is valid (today or one of the next five days)
        /// </summary>
        public bool IsValid { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a DayViewModel by parsing an XElement.
        /// </summary>
        /// <param name="xmlDay">The XML element of a day.</param>
        public DayViewModel(XElement xmlDay)
        {
            string timestamp = xmlDay.Attribute("timestamp").Value;
            _day = convertTimestampToDate(timestamp);

            // Do not process items of the past.
            // These days will be deleted after parsing the xml file.
            if (_day.Date >= DateTime.Now.Date)
            {
                Meals = new ObservableCollection<MealViewModel>();

                foreach (XElement xmlMeal in xmlDay.Elements("item"))
                {
                    this.Meals.Add(new MealViewModel(xmlMeal));
                }

                IsValid = true;
            }
            else
            {
                IsValid = false;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Converts a UNIX timestamp in a DateTime object.
        /// </summary>
        /// <param name="timestamp">The timepsamp as a string.</param>
        /// <returns>The converted DateTime object.</returns>
        private DateTime convertTimestampToDate(string timestamp)
        {
            //  gerechnet wird ab der UNIX Epoche (+12h and +2h for GMT+2)
            DateTime dateTime = new DateTime(1970, 1, 1, 14, 0, 0, 0);
            // den Timestamp addieren           
            dateTime = dateTime.AddSeconds(Int32.Parse(timestamp));

            return dateTime;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the day.
        /// </summary>
        public DateTime Day
        {
            get
            {
                return _day;
            }
            set
            {
                if (value != _day)
                {
                    _day = value;
                    NotifyPropertyChanged("Day");
                }
            }
        }

        #endregion
    }
}
