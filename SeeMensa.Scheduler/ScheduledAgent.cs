using System.Diagnostics;
using System.Windows;
using Microsoft.Phone.Scheduler;
using System;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using SeeMensa.Common.ViewModels;
using System.Globalization;
using System.Threading;
using SeeMensa.Common.LiveTile;
using System.Net;
using Microsoft.Phone.Net.NetworkInformation;

namespace SeeMensa.Scheduler
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        /// <summary>
        /// The isolated storage settings.
        /// </summary>
        private readonly IsolatedStorageSettings _settings = IsolatedStorageSettings.ApplicationSettings;

        /// <remarks>
        /// ScheduledAgent constructor, initializes the UnhandledException handler
        /// </remarks>
        static ScheduledAgent()
        {
            // Subscribe to the managed exception handler
            Deployment.Current.Dispatcher.BeginInvoke(delegate
            {
                Application.Current.UnhandledException += UnhandledException;
            });
        }

        /// Code to execute on Unhandled Exceptions
        private static void UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }

        /// <summary>
        /// Agent that runs a scheduled task
        /// </summary>
        /// <param name="task">
        /// The invoked task
        /// </param>
        /// <remarks>
        /// This method is called when a periodic or resource intensive task is invoked
        /// </remarks>
        protected override void OnInvoke(ScheduledTask task)
        {
            load();

            if (isRefreshRequired())
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    WebClient _client = new WebClient();
                    _client.DownloadStringCompleted += (s, e) =>
                        {
                            if (e.Error == null)
                            {
                                // Replace Euro-Symbols
                                string xml = e.Result.Replace((char)128, '€');

                                MainViewModel.Instance.LastUpdate = DateTime.Now;
                                MainViewModel.Instance.Xml = xml;

                                UpdateLiveTile();
                                save();
                            }

                            // If debugging is enabled, launch the agent again in one minute.
#if DEBUG
                            ScheduledActionService.LaunchForTest(task.Name, TimeSpan.FromSeconds(60));
#endif

                            NotifyComplete();
                        };

                    // refresh
                    if (!_client.IsBusy)
                    {
                        Uri uri;
                        if (CultureInfo.CurrentCulture.TwoLetterISOLanguageName.Equals("de"))
                        {
                            uri = MainViewModel.Instance.CurrentMensaItem.DeUri;
                        }
                        else
                        {
                            uri = MainViewModel.Instance.CurrentMensaItem.EnUri;
                        }

                        _client.DownloadStringAsync(uri);
                    }
                }
                else
                {
                    // just end the background task, if no internet is available
#if DEBUG
                    ScheduledActionService.LaunchForTest(task.Name, TimeSpan.FromSeconds(60));
#endif

                    NotifyComplete();
                }
            }
            else
            {
                DateTime now = DateTime.Now;
                DateTime lastUpdate = MainViewModel.Instance.LastTileUpdate;

                TimeSpan delay = now.Subtract(lastUpdate);

                // Update the live tile
                if (delay.TotalHours >= 12 || lastUpdate.Day != now.Day)
                {
                    UpdateLiveTile();
                    save();
                }

                // If debugging is enabled, launch the agent again in one minute.
#if DEBUG
                ScheduledActionService.LaunchForTest(task.Name, TimeSpan.FromSeconds(60));
#endif

                NotifyComplete();
            }
        }

        private void UpdateLiveTile()
        {
            MainViewModel.Instance.CreateFromXml(MainViewModel.Instance.Xml, true);

            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {

                SeeMensaLiveTileHelper.UpdateLiveTiles();
                MainViewModel.Instance.LastTileUpdate = DateTime.Now;
            });
        }

        /// <summary>
        /// Load data for the ViewModel Items.
        /// </summary>
        private bool isRefreshRequired()
        {
            if (!string.IsNullOrEmpty(MainViewModel.Instance.Xml))
            {
                DateTime now = DateTime.Now;
                DateTime lastUpdate = MainViewModel.Instance.LastUpdate;

                TimeSpan delay = now.Subtract(lastUpdate);

                if (delay.TotalDays >= 7)
                {
                    return true;
                }
            }
            else
            {
                return true;
            }

            return false;
        }

        #region Load/Save

        /// <summary>
        /// Saves the settings.
        /// </summary>
        private void save()
        {
            if (!String.IsNullOrEmpty(MainViewModel.Instance.Xml))
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

                _settings.Save();
            }

        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        private void load()
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
        }

        #endregion
    }
}