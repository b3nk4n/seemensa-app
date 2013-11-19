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
using Microsoft.Phone.Scheduler;
using SeeMensa.Common.ViewModels;
using SeeMensa.Common;

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

        private PeriodicTask periodicTask;
        private const string periodicTaskName = "seeMENSA Background Task";
        public bool agentsAreEnabled = true;

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

            switch (MainViewModel.PriceType)
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

            // set periodic toggle state
            if (IsPeriodicTaskActive(periodicTaskName))
                BackgroundAgentToggle.IsChecked = true;
            else
                BackgroundAgentToggle.IsChecked = false;
            BackgroundAgentToggle.Checked += (s, args) =>
            {
                StartPeriodicTask();
            };
            BackgroundAgentToggle.Unchecked += (s, args) =>
            {
                RemoveTask(periodicTaskName);
            };
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
            MainViewModel.PriceType = (PriceType) Enum.Parse(typeof(PriceType), (string)rb.Tag, true);
        }

        private bool IsPeriodicTaskActive(string agentName)
        {
            var task = ScheduledActionService.Find(agentName) as PeriodicTask;
            return task != null;
        }

        private void StartPeriodicTask()
        {
            // Variable for tracking enabled status of background agents for this app.
            agentsAreEnabled = true;

            // Obtain a reference to the period task, if one exists
            periodicTask = ScheduledActionService.Find(periodicTaskName) as PeriodicTask;

            // If the task already exists and background agents are enabled for the
            // application, you must remove the task and then add it again to update 
            // the schedule
            if (periodicTask != null)
            {
                RemoveTask(periodicTaskName);
            }

            periodicTask = new PeriodicTask(periodicTaskName);

            // The description is required for periodic agents. This is the string that the user
            // will see in the background services Settings page on the device.
            periodicTask.Description = "Updates the live tile periodically.";

            // Place the call to Add in a try block in case the user has disabled agents.
            try
            {
                ScheduledActionService.Add(periodicTask);
                //PeriodicStackPanel.DataContext = periodicTask;

                // If debugging is enabled, use LaunchForTest to launch the agent in one minute.
#if(DEBUG)
                ScheduledActionService.LaunchForTest(periodicTaskName, TimeSpan.FromSeconds(60));
#endif
            }
            catch (InvalidOperationException exception)
            {
                if (exception.Message.Contains("BNS Error: The action is disabled"))
                {
                    MessageBox.Show("Background agents for this application have been disabled by the user.");
                    agentsAreEnabled = false;
                    //PeriodicCheckBox.IsChecked = false;
                }

                if (exception.Message.Contains("BNS Error: The maximum number of ScheduledActions of this type have already been added."))
                {
                    // No user action required. The system prompts the user when the hard limit of periodic tasks has been reached.

                }
                BackgroundAgentToggle.IsChecked = false;
                //PeriodicCheckBox.IsChecked = false;
            }
            catch (SchedulerServiceException)
            {
                // No user action required.
                //PeriodicCheckBox.IsChecked = false;
                BackgroundAgentToggle.IsChecked = false;
            }
        }

        private void RemoveTask(string name)
        {
            try
            {
                ScheduledActionService.Remove(name);
            }
            catch (Exception)
            {
            }
        }
    }
}