using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LinqToVisualTree;
using Microsoft.Phone.Controls;
using System;
using System.Windows.Navigation;
using System.Diagnostics;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Net.NetworkInformation;

namespace SeeMensa.Ad
{
    public class AdBanner
    {
        /// <summary>
        /// The banner web browser.
        /// </summary>
        private WebBrowser _browser;

        /// <summary>
        /// The banner uri.
        /// </summary>
        private Uri _bannerUri;

        /// <summary>
        /// The ad loaded/rendered callback.
        /// </summary>
        private Action _loaded;

        /// <summary>
        /// Gets or sets whether to suppress the scrolling of
        /// the WebBrowser control;
        /// </summary>
        public bool ScrollDisabled { get; set; }

        public AdBanner(WebBrowser browser, Uri bannerUri, Action loaded)
        {
            _browser = browser;
            _browser.Loaded += new RoutedEventHandler(browser_Loaded);
            _browser.IsScriptEnabled = true;
            _browser.Navigating += browser_Navigating;
            _browser.ScriptNotify += _browser_ScriptNotify;
            _bannerUri = bannerUri;
            _loaded = loaded;

            if (NetworkInterface.GetIsNetworkAvailable())
            {
                _browser.Navigate(_bannerUri);
            }
        }

        /// <summary>
        /// Calls the add-banner-loaded callback, when the banner was rendered.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _browser_ScriptNotify(object sender, NotifyEventArgs e)
        {
            if (e.Value.Equals("loaded"))
            {
                if (_loaded != null)
                    _loaded();
            }
        }

        /// <summary>
        /// Cancels the navigation to any page except the defined banner page.
        /// </summary>
        void browser_Navigating(object sender, NavigatingEventArgs e)
        {
            if (e.Uri != _bannerUri)
            {
                e.Cancel = true;

                WebBrowserTask fsBrowser = new WebBrowserTask();
                fsBrowser.Uri = e.Uri;
                fsBrowser.Show();
            }
        }

        private void browser_Loaded(object sender, RoutedEventArgs e)
        {
            var border = _browser.Descendants<Border>().Last() as Border;
            
            border.ManipulationDelta += Border_ManipulationDelta;
            border.ManipulationCompleted += Border_ManipulationCompleted;
        }

        /// <summary>
        /// Stops the zoom and scroll interaction of the browser.
        /// </summary>
        private void Border_ManipulationCompleted(object sender,
                                                  ManipulationCompletedEventArgs e)
        {
            // suppress zoom
            if (e.FinalVelocities.ExpansionVelocity.X != 0.0 ||
                e.FinalVelocities.ExpansionVelocity.Y != 0.0 ||
                (ScrollDisabled && e.IsInertial))
                e.Handled = true;
        }

        /// <summary>
        /// Stops the zoom and scroll interaction of the browser.
        /// </summary>
        private void Border_ManipulationDelta(object sender,
                                              ManipulationDeltaEventArgs e)
        {
            // suppress zoom
            if (e.DeltaManipulation.Scale.X != 0.0 ||
                e.DeltaManipulation.Scale.Y != 0.0)
                e.Handled = true;

            // optionally suppress scrolling
            if (ScrollDisabled)
            {
                if (e.DeltaManipulation.Translation.X != 0.0 ||
                  e.DeltaManipulation.Translation.Y != 0.0)
                    e.Handled = true;
            }
        }
    }
}
