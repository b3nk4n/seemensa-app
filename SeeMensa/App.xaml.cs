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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using System.Globalization;
using System.Threading;
using SeeMensa.Language;
using PhoneKit.Framework.Tile;
using PhoneKit.Framework.Core.Graphics;
using PhoneKit.Framework.Core.Storage;
using SeeMensa.Common.ViewModels;
using SeeMensa.Common;
using SeeMensa.Common.Controls;
using PhoneKit.Framework.Support;
using BugSense;
using BugSense.Core.Model;

namespace SeeMensa
{
    public partial class App : Application
    {
        private readonly IsolatedStorageSettings _settings = IsolatedStorageSettings.ApplicationSettings;

        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Initialize BugSense
            BugSenseHandler.Instance.InitAndStartSession(new ExceptionManager(Current), RootFrame, "e0151f5b");

            // Show graphics profiling information while debugging.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Display the current frame rate counters.
                //Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode, 
                // which shows areas of a page that are being GPU accelerated with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;
            }

            // Standard Silverlight initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

            // Laguage:
            if (!CultureInfo.CurrentCulture.TwoLetterISOLanguageName.Equals("de"))
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            }
        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            load(false);
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            if (e.IsApplicationInstancePreserved)
                return;

            // Ensure that application state is restored appropriately
            if (!MainViewModel.Instance.IsDataLoaded)
            {
                //MainViewModel.LoadData();
                load(true);
            }
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
            // Ensure that required application state is persisted here.
            save();
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
            save();
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            ErrorReportingManager.Instance.Save(e.Exception, Language.Language.ApplicationVersion, Language.Language.ResourceLanguage);

            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new TransitionFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;

            ErrorReportingManager.Instance.CheckAndReport(
                "apps@bsautermeister.de",
                "[seeMENSA] Error Report");
        }

        #endregion

        #region Load/Save

        /// <summary>
        /// Saves the settings.
        /// </summary>
        private void save()
        {
            if(!String.IsNullOrEmpty(MainViewModel.Instance.Xml))
            {
                // Xml string
                if (_settings.Contains("xml"))
                {
                    _settings["xml"] = MainViewModel.Instance.Xml;
                }
                else
                {
                    _settings.Add("xml", MainViewModel.Instance.Xml);
                }
                // Mensa index
                if (_settings.Contains("mensaIndex"))
                {
                    _settings["mensaIndex"] = MainViewModel.Instance.MensaIndex;
                }
                else
                {
                    _settings.Add("mensaIndex", MainViewModel.Instance.MensaIndex);
                }
                // Last update
                if (_settings.Contains("lastUpdate"))
                {
                    _settings["lastUpdate"] = MainViewModel.Instance.LastUpdate;
                }
                else
                {
                    _settings.Add("lastUpdate", MainViewModel.Instance.LastUpdate);
                }
                // Last tile update
                if (_settings.Contains("lastTileUpdate"))
                {
                    _settings["lastTileUpdate"] = MainViewModel.Instance.LastTileUpdate;
                }
                else
                {
                    _settings.Add("lastTileUpdate", MainViewModel.Instance.LastTileUpdate);
                }
                // Price type
                if (_settings.Contains("priceType"))
                {
                    _settings["priceType"] = MainViewModel.Instance.PriceType;
                }
                else
                {
                    _settings.Add("priceType", MainViewModel.Instance.PriceType);
                }
                // Panorama index
                if (_settings.Contains("panoramaIndex"))
                {
                    _settings["panoramaIndex"] = MainViewModel.Instance.PanoramaIndex;
                }
                else
                {
                    _settings.Add("panoramaIndex", MainViewModel.Instance.PanoramaIndex);
                }

                _settings.Save();
            }

        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        /// <param name="withPanoramaIndex">Indicates, wheather the panorama index should be loaded or not.</param>
        private void load(bool withPanoramaIndex)
        {
            if (_settings.Contains("xml"))
            {
                MainViewModel.Instance.Xml = (string)_settings["xml"];
            }
            if (_settings.Contains("mensaIndex"))
            {
                int mIndex = (int)_settings["mensaIndex"];

                // NOTE: Neccessary for the v1.1 Update, because the Themenpark-Mensa was
                //       combined with Uni Mensa.
                if (mIndex == 4)
                    mIndex = 1;

                MainViewModel.Instance.MensaIndex = mIndex;
            }
            if (_settings.Contains("lastUpdate"))
            {
                MainViewModel.Instance.LastUpdate = (DateTime)_settings["lastUpdate"];
            }
            if (_settings.Contains("lastTileUpdate"))
            {
                MainViewModel.Instance.LastTileUpdate = (DateTime)_settings["lastTileUpdate"];
            }
            if (_settings.Contains("priceType"))
            {
                MainViewModel.Instance.PriceType = (PriceType)_settings["priceType"];
            }
            if (withPanoramaIndex)
            {
                if (_settings.Contains("panoramaIndex"))
                {
                    MainViewModel.Instance.PanoramaIndex = (int)_settings["panoramaIndex"];
                }
            }
        }

        #endregion
    }
}