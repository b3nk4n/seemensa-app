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
using PhoneKit.Framework.Graphics;
using PhoneKit.Framework.Storage;
using SeeMensa.Common.ViewModels;
using SeeMensa.Common;
using SeeMensa.Common.Controls;

namespace SeeMensa
{
    public partial class App : Application
    {
        private static MainViewModel viewModel = null;

        private readonly IsolatedStorageSettings _settings = IsolatedStorageSettings.ApplicationSettings;

        /// <summary>
        /// A static ViewModel used by the views to bind against.
        /// </summary>
        /// <returns>The MainViewModel object.</returns>
        public static MainViewModel ViewModel
        {
            get
            {
                // Delay creation of the view model until necessary
                if (viewModel == null)
                    viewModel = new MainViewModel();

                return viewModel;
            }
        }

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
            // Global handler for uncaught exceptions. 
            UnhandledException += Application_UnhandledException;

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
            // Ensure that application state is restored appropriately
            if (!App.ViewModel.IsDataLoaded)
            {
                //App.ViewModel.LoadData();
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

        /// <summary>
        /// Updates the live tile.
        /// </summary>
        public static void UpdateLiveTiles()
        {
            LiveTileHelper.ClearStorage();

            IList<Uri> images = CreateLiveTileImages();
            if (images.Count > 0)
            {
                LiveTileHelper.UpdateDefaultTile(new CycleTileData
                    {
                        CycleImages = images,
                        SmallBackgroundImage = App.ViewModel.CurrentMensaItem.ImageUri
                    });
            }
        }

        /// <summary>
        /// Creates live tile images from mensa data.
        /// </summary>
        /// <returns>The list of images.</returns>
        public static IList<Uri> CreateLiveTileImages()
        {
            IList<Uri> images = new List<Uri>();
            var day = App.ViewModel.Days[0];

            LiveTileHelper.ClearStorage();

            for (int i = 0; i < App.ViewModel.Days[0].Meals.Count; ++i)
            {
                var image = GraphicsHelper.Create(new MealNormalTileControl(day.Meals[i].Category, day.Meals[i].Title, App.ViewModel.CurrentMensaItem.ImageUri.OriginalString));
                images.Add(StorageHelper.SaveJpeg(LiveTileHelper.SHARED_SHELL_CONTENT_PATH + string.Format("livetile{0}.jpeg", i), image));
            }

            return images;
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
                // REMARK: Sometimes crashes when debugger is attached. But works fine in live mode. idky.
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
        }

        #endregion

        #region Load/Save

        /// <summary>
        /// Saves the settings.
        /// </summary>
        private void save()
        {
            if(!String.IsNullOrEmpty(App.ViewModel.Xml))
            {
                // Xml string
                if (_settings.Contains("xml"))
                {
                    _settings["xml"] = App.ViewModel.Xml;
                }
                else
                {
                    _settings.Add("xml", App.ViewModel.Xml);
                }
                // Mensa index
                if (_settings.Contains("mensaIndex"))
                {
                    _settings["mensaIndex"] = App.ViewModel.MensaIndex;
                }
                else
                {
                    _settings.Add("mensaIndex", App.ViewModel.MensaIndex);
                }
                // Last update
                if (_settings.Contains("lastUpdate"))
                {
                    _settings["lastUpdate"] = App.ViewModel.LastUpdate;
                }
                else
                {
                    _settings.Add("lastUpdate", App.ViewModel.LastUpdate);
                }
                // Price type
                if (_settings.Contains("priceType"))
                {
                    _settings["priceType"] = MainViewModel.PriceType;
                }
                else
                {
                    _settings.Add("priceType", MainViewModel.PriceType);
                }
                // Panorama index
                if (_settings.Contains("panoramaIndex"))
                {
                    _settings["panoramaIndex"] = App.ViewModel.PanoramaIndex;
                }
                else
                {
                    _settings.Add("panoramaIndex", App.ViewModel.PanoramaIndex);
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
                App.ViewModel.Xml = (string)_settings["xml"];
            }
            if (_settings.Contains("mensaIndex"))
            {
                int mIndex = (int)_settings["mensaIndex"];

                // NOTE: Neccessary for the v1.1 Update, because the Themenpark-Mensa was
                //       combined with Uni Mensa.
                if (mIndex == 4)
                    mIndex = 1;

                App.ViewModel.MensaIndex = mIndex;
            }
            if (_settings.Contains("lastUpdate"))
            {
                App.ViewModel.LastUpdate = (DateTime)_settings["lastUpdate"];
            }
            if (_settings.Contains("priceType"))
            {
                MainViewModel.PriceType = (PriceType)_settings["priceType"];
            }
            if (withPanoramaIndex)
            {
                if (_settings.Contains("panoramaIndex"))
                {
                    App.ViewModel.PanoramaIndex = (int)_settings["panoramaIndex"];
                }
            }
        }

        #endregion
    }
}