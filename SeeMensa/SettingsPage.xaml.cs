using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace SeeMensa
{
    /// <summary>
    /// The settings page.
    /// </summary>
    public partial class SettingsPage : PhoneApplicationPage
    {
        /// <summary>
        /// The selected mensa index, when opening the settings menu.
        /// </summary>
        private int _initialMensaIndex;

        /// <summary>
        /// Creates a new settings page.
        /// </summary>
        public SettingsPage()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(SettingsPage_Loaded);

            if (!App.ViewModel.IsDataLoaded)
                App.ViewModel.CreateFromXml(App.ViewModel.Xml);

            this.DataContext = App.ViewModel;
        }

        /// <summary>
        /// When the settings page is loaded, select the current mensa.
        /// </summary>
        void SettingsPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.lpMensas.SelectedIndex = App.ViewModel.MensaIndex;

            switch (App.ViewModel.PriceType)
            {
                case PriceType.Student:
                    rbStudent.IsChecked = true;
                    break;

                case PriceType.Guest:
                    rbGuest.IsChecked = true;
                    break;

                case PriceType.Employee:
                    rbEmployee.IsChecked = true;
                    break;

                case PriceType.Pupil:
                    rbPupil.IsChecked = true;
                    break;
            } 
        }

        /// <summary>
        /// Saves the last mensa index when the SettingsPage is navigated to.
        /// </summary>
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _initialMensaIndex = App.ViewModel.MensaIndex;
        }

        /// <summary>
        /// Checks if the selected mensa has changed.
        /// </summary>
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            if (_initialMensaIndex != App.ViewModel.MensaIndex)
                //App.ViewModel.NeedsRefresh = true;
                App.ViewModel.Xml = string.Empty;
        }

        /// <summary>
        /// Work arround counter for: lpMensas_SelectionChanged
        /// </summary>
        private int ctr = 2;

        /// <summary>
        /// Updates the current mensa, when the selection of the ListPicker has changed.
        /// Note: Using work arround counter, because this event is fired twice at NavigatingTo...
        /// </summary>
        private void lpMensas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ctr > 0)
                ctr--;
            else
            {
                ListPicker picker = sender as ListPicker;
                App.ViewModel.MensaIndex = this.lpMensas.SelectedIndex;
            }
        }

        /// <summary>
        /// Change the price type, if the selection of the RadioButtons has changed.
        /// </summary>
        private void PriceChecked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            App.ViewModel.PriceType = (PriceType) Enum.Parse(typeof(PriceType), (string)rb.Tag, true);
        }
    }
}