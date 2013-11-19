using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.IO;
using PhoneKit.Framework.MVVM;
using System.Globalization;


namespace SeeMensa.Common.ViewModels
{
    /// <summary>
    /// The main view model.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region Members

        /// <summary>
        /// The number of displayed days.
        /// </summary>
        public const int MAX_DISPLAY_DAYS = 5;

        /// <summary>
        /// A collection for Days objects.
        /// </summary>
        public ObservableCollection<DayViewModel> Days { get; private set; }

        /// <summary>
        /// The index of the current mensa.
        /// </summary>
        private int _mensaIndex;

        /// <summary>
        /// Indicates wheather the data is loaded or not.
        /// </summary>
        public bool IsDataLoaded { get; private set; }

        /// <summary>
        /// The downloaded xml.
        /// </summary>
        public string Xml { get; set; }

        /// <summary>
        /// The date of the last update.
        /// </summary>
        public DateTime LastUpdate { get; set; }

        /// <summary>
        /// The selected price type.
        /// </summary>
        private static PriceType _priceType;

        /// <summary>
        /// The selected index in the panorama control.
        /// </summary>
        private int _panoramaIndex = -1;

        /// <summary>
        /// The mensa items.
        /// </summary>
        private ObservableCollection<MensaItemViewModel> _mensaItems;

        /// <summary>
        /// The current selected mensa name.
        /// </summary>
        private string _currentMensaName;

        /// <summary>
        /// Flag which indicates a refresh of data.
        /// </summary>
        private bool _needsRefresh = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new MainViewModel.
        /// </summary>
        public MainViewModel()
        {
            this.Days = new ObservableCollection<DayViewModel>();

            string uniknTitle;
            string htwgknTitle;
            string phwgTitle;
            string unifnTitle;

            // get localized mensa titles (because currently not clear how to access the Laguage.resx from here)
            if (CultureInfo.CurrentCulture.TwoLetterISOLanguageName.Equals("de"))
            {
                uniknTitle = "Menseria Universität Konstanz";
                htwgknTitle = "Mensa HTWG Konstanz";
                phwgTitle = "Mensa HS/PH Weingarten";
                unifnTitle = "Mensa Friedrichshafen";
            }
            else
            {
                uniknTitle = "Menseria University of Constance";
                htwgknTitle = "Canteen HTWG Constance";
                phwgTitle = "Canteen HS/PH Weingarten";
                unifnTitle = "Canteen Friedrichshafen";
            }

            _mensaItems = new ObservableCollection<MensaItemViewModel>();
            _mensaItems.Add(new MensaItemViewModel(uniknTitle,
                                                   new Uri("http://www.max-manager.de/daten-extern/seezeit/xml/mensa_giessberg/speiseplan.xml", UriKind.Absolute),
                                                   new Uri("http://www.max-manager.de/daten-extern/seezeit/xml/mensa_giessberg/speiseplan_en.xml", UriKind.Absolute),
                                                   "Universitätsstraße 10",
                                                   "78464 Konstanz",
                                                   "Images/unikn.png"));
            _mensaItems.Add(new MensaItemViewModel(htwgknTitle,
                                                   new Uri("http://www.max-manager.de/daten-extern/seezeit/xml/mensa_htwg/speiseplan.xml", UriKind.Absolute),
                                                   new Uri("http://www.max-manager.de/daten-extern/seezeit/xml/mensa_htwg/speiseplan_en.xml", UriKind.Absolute),
                                                   "Alfred-Wachtel-Straße 12",
                                                   "78462 Konstanz",
                                                   "Images/htwgkn.png"));
            _mensaItems.Add(new MensaItemViewModel(phwgTitle,
                                                   new Uri("http://www.max-manager.de/daten-extern/seezeit/xml/mensa_weingarten/speiseplan.xml", UriKind.Absolute),
                                                   new Uri("http://www.max-manager.de/daten-extern/seezeit/xml/mensa_weingarten/speiseplan_en.xml", UriKind.Absolute),
                                                   "Doggenriedstraße 28",
                                                   "88250 Weingarten",
                                                   "Images/phwg.png"));
            _mensaItems.Add(new MensaItemViewModel(unifnTitle,
                                                   new Uri("http://www.max-manager.de/daten-extern/seezeit/xml/mensa_friedrichshafen/speiseplan.xml", UriKind.Absolute),
                                                   new Uri("http://www.max-manager.de/daten-extern/seezeit/xml/mensa_friedrichshafen/speiseplan_en.xml", UriKind.Absolute),
                                                   "Fallenbrunnen 2",
                                                   "88045 Friedrichshafen",
                                                   "Images/unifn.png"));

            MensaIndex = 0;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the MainViewModel by parsing an XML file.
        /// </summary>
        /// <param name="xmlDoc">The XML document of the whole menu.</param>
        public void CreateFromXml(string xml)
        {
            XDocument xmlDoc = XDocument.Parse(xml);

            this.Days.Clear();

            int validDaysCounter = 0;

            foreach (XElement xmlDay in xmlDoc.Elements("speiseplan").Elements("tag"))
            {
                var day = new DayViewModel(xmlDay);

                if (day.IsValid)
                {
                    this.Days.Add(day);
                    ++validDaysCounter;
                }

                if (validDaysCounter >= MAX_DISPLAY_DAYS)
                {
                    break;
                }
            }

            this.Xml = xml;
            
            if (_mensaIndex == -1)
                this.IsDataLoaded = false;
            else
                this.IsDataLoaded = true;

            CurrentMensaName = CurrentMensaItem.Name;
        }

        /// <summary>
        /// Checks whether the mensa has meals or not.
        /// </summary>
        /// <returns>Boolean value whether the selected mensa has meals or not.</returns>
        public bool HasMeals()
        {
            return Days.Count > 0;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the index of the current mensa.
        /// </summary>
        public int MensaIndex
        {
            set
            {
                if(value < 0)
                {
                    throw new IndexOutOfRangeException("Index out of range.");
                }
                _mensaIndex = value;
            }
            get
            {
                return _mensaIndex;
            }
        }

        /// <summary>
        /// Gets or sets the current selected price type.
        /// </summary>
        public static PriceType PriceType
        {
            set
            {
                _priceType = value;
            }
            get
            {
                return _priceType;
            }
        }

        /// <summary>
        /// Gets or sets the current panorama index.
        /// </summary>
        public int PanoramaIndex
        {
            set
            {
                _panoramaIndex = value;
            }
            get
            {
                return _panoramaIndex;
            }
        }

        /// <summary>
        /// Gets the mensa items.
        /// </summary>
        public ObservableCollection<MensaItemViewModel> MensaItems
        {
            get
            {
                return _mensaItems;
            }
        }

        /// <summary>
        /// Gets the current mensa item.
        /// </summary>
        public MensaItemViewModel CurrentMensaItem
        {
            get
            {
                return MensaItems[MensaIndex];
            }
        }

        /// <summary>
        /// Gets or sets the current mensa name.
        /// </summary>
        public string CurrentMensaName
        {
            get
            {
                return _currentMensaName;
            }
            set
            {
                if (value != _currentMensaName)
                {
                    _currentMensaName = value;
                    NotifyPropertyChanged("CurrentMensaName");
                }
            }
        }

        /// <summary>
        /// Gets or sets the refresh flag.
        /// </summary>
        public bool NeedsRefresh
        {
            get
            {
                return _needsRefresh;
            }
            set
            {
                _needsRefresh = value;
            }
        }

        #endregion
    }
}