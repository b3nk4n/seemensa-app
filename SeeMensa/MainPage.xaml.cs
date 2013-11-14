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
using System.Xml.Linq;
using System.Globalization;
using SeeMensa.Language;
using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.IO;
using System.Windows.Navigation;
using SeeMensa.Ad;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;

namespace SeeMensa
{
    public partial class MainPage : PhoneApplicationPage
    {
        /// <summary>
        /// The web client to access the mensa menu of Seezeit.
        /// </summary>
        private readonly WebClient _client;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);

            _client = new WebClient();

            _client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(_client_DownloadStringCompleted);

            this.localizeAppBar();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
        }

        /// <summary>
        /// Localizes the application bar.
        /// </summary>
        private void localizeAppBar()
        {
            ((Microsoft.Phone.Shell.ApplicationBarIconButton)this.ApplicationBar.Buttons[0]).Text = SeeMensa.Language.Language.AppBarRefresh;
            ((Microsoft.Phone.Shell.ApplicationBarIconButton)this.ApplicationBar.Buttons[1]).Text = SeeMensa.Language.Language.SettingsTitle;
            ((Microsoft.Phone.Shell.ApplicationBarIconButton)this.ApplicationBar.Buttons[2]).Text = SeeMensa.Language.Language.AppBarMensaInfo;
            ((Microsoft.Phone.Shell.ApplicationBarMenuItem)this.ApplicationBar.MenuItems[0]).Text = SeeMensa.Language.Language.InAppStoreTitle;
            ((Microsoft.Phone.Shell.ApplicationBarMenuItem)this.ApplicationBar.MenuItems[1]).Text = SeeMensa.Language.Language.AppBarWin8;
            ((Microsoft.Phone.Shell.ApplicationBarMenuItem)this.ApplicationBar.MenuItems[2]).Text = SeeMensa.Language.Language.AboutTitle;
        }

        /// <summary>
        /// Updates the ApplicationBar buttons. These must disabled if there is no loaded data.
        /// </summary>
        private void updateAppBar()
        {
            if (App.ViewModel.IsDataLoaded)
            {
                ((Microsoft.Phone.Shell.ApplicationBarIconButton)ApplicationBar.Buttons[1]).IsEnabled = true;
                ((Microsoft.Phone.Shell.ApplicationBarIconButton)ApplicationBar.Buttons[2]).IsEnabled = true;
            }
            else
            {
                ((Microsoft.Phone.Shell.ApplicationBarIconButton)ApplicationBar.Buttons[1]).IsEnabled = false;
                ((Microsoft.Phone.Shell.ApplicationBarIconButton)ApplicationBar.Buttons[2]).IsEnabled = false;
            }
        }

        /// <summary>
        /// Stores the curren panorama index, when the page is navigated from.
        /// </summary>
        protected override void OnNavigatingFrom(System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            App.ViewModel.PanoramaIndex = panMeals.SelectedIndex;
        }

        /// <summary>
        /// Load data for the ViewModel Items.
        /// </summary>
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(App.ViewModel.Xml))
            {
                DateTime now = DateTime.Now;
                DateTime lastUpdate = App.ViewModel.LastUpdate;

                TimeSpan delay = now.Subtract(lastUpdate);

                if (delay.Days >= 7)
                {
                    this.refresh();
                }
                else
                {
                    this.updatePanaroamaControl(App.ViewModel.Xml);

                    this.DataContext = App.ViewModel;

                    // Sets the default/current item (new in Version 1.3)
                    if (App.ViewModel.PanoramaIndex >= 0 &&
                        App.ViewModel.PanoramaIndex < panMeals.Items.Count)
                    {
                        panMeals.DefaultItem = panMeals.Items[App.ViewModel.PanoramaIndex];
                    }

                    this.updateAppBar();
                }
            }
            else
            {
                this.DataContext = null;
                this.refresh();
            }

            // LoadCustomBammer
            LoadWebBanner();

            // Update the live tile
            App.UpdateLiveTiles();
        }

        /// <summary>
        /// Starts the refresh proccess by async downloading the xml string.
        /// </summary>
        async private void refresh()
        {
            if (!_client.IsBusy)
            {
                Uri uri;
                if (CultureInfo.CurrentCulture.TwoLetterISOLanguageName.Equals("de"))
                {
                    uri = App.ViewModel.CurrentMensaItem.DeUri;
                }
                else
                {
                    uri = App.ViewModel.CurrentMensaItem.EnUri;
                }

                // Start system tray progress bar
                StartSystemTrayProgressBar();
                
                _client.DownloadStringAsync(uri);
            }
        }

        /// <summary>
        /// Starts the system tray progress bar.
        /// </summary>
        private void StartSystemTrayProgressBar()
        {
            var progressIndicator = new Microsoft.Phone.Shell.ProgressIndicator
            {
                IsVisible = true,
                IsIndeterminate = true,
                Text = SeeMensa.Language.Language.UpdateStatusText
            };

            Microsoft.Phone.Shell.SystemTray.SetProgressIndicator(this, progressIndicator);
        }

        /// <summary>
        /// Stops the system tray progress bar.
        /// </summary>
        private void StopSystemTrayProgressBar()
        {
            Microsoft.Phone.Shell.SystemTray.ProgressIndicator.IsVisible = false;
        }

        /// <summary>
        /// Updates the panroama control. Disables the control if there are no meals to show.
        /// </summary>
        /// <param name="xml">The xml string.</param>
        private void updatePanaroamaControl(string xml)
        {
            App.ViewModel.CreateFromXml(xml);

            if (App.ViewModel.HasMeals())
            {
                panMeals.IsEnabled = true;
            }
            else
            {
                panMeals.IsEnabled = false;
                MessageBox.Show(SeeMensa.Language.Language.MessageBoxNoDataText,
                                SeeMensa.Language.Language.MessageBoxAttention,
                                MessageBoxButton.OK);
            }
        }

        /// <summary>
        /// When the async download is completed, parse the xml und update the panroama control.
        /// </summary>
        void _client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                // Replace Euro-Symbols
                string xml = e.Result.Replace((char)128, '€');

                this.updatePanaroamaControl(xml);

                this.DataContext = App.ViewModel;

                App.ViewModel.LastUpdate = DateTime.Now;
            }
            else
            {
                MessageBox.Show(SeeMensa.Language.Language.MessageBoxNoConnectivityText,
                                SeeMensa.Language.Language.MessageBoxAttention,
                                MessageBoxButton.OK);
            }

            // Stop system tray progress bar
            StopSystemTrayProgressBar();

            this.updateAppBar();
        }

        /// <summary>
        /// Navigate to the in app store page.
        /// </summary>
        private void abmStore_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/InAppStorePage.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Navigate to the about page.
        /// </summary>
        private void abmAbout_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Navigate to the help page.
        /// </summary>
        private void abmHelp_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/HelpPage.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Opens the marketplace.
        /// </summary>
        private void abmMore_Click(object sender, EventArgs e)
        {
            var marketplace = new MarketplaceSearchTask();
            marketplace.SearchTerms = "Benjamin Sautermeister";
            marketplace.Show();
        }

        /// <summary>
        /// Opens the seemensa windows page in the browser.
        /// </summary>
        private void abmWin8_Click(object sender, EventArgs e)
        {
            var browser = new WebBrowserTask();
            browser.Uri = new Uri("http://bsautermeister.de/seemensawindows");
            browser.Show();
        }

        /// <summary>
        /// Refresh the canteen plans..
        /// </summary>
        private void abiRefresh_Click(object sender, EventArgs e)
        {
            this.refresh();
        }

        /// <summary>
        /// Navigate to the settings page.
        /// </summary>
        private void abiSettings_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Navigate to the info page.
        /// </summary>
        private void abiInfo_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/InfoPage.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Loads the StudiCluster Web-Banner
        /// </summary>
        private void LoadWebBanner()
        {
            var productLicences = CurrentApp.LicenseInformation.ProductLicenses;
            var adFreeLicense = productLicences["ad_free"];

            if (adFreeLicense.IsActive)
            {
                // hide banner
                WebBanner.Visibility = Visibility.Collapsed;
            }
            else
            {
                // load advertisment
                AdBanner adBanner = new AdBanner(WebBanner,
                    new Uri("http://bsautermeister.de/seemensa/advertising/seemensa_sc.html"),
                    () => {
                        WebBanner.Visibility = System.Windows.Visibility.Visible;
                        AdInTransition.Begin();
                    });
                adBanner.ScrollDisabled = true;
            }
        }
    }
}