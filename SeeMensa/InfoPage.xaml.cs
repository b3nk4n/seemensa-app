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
    /// The info page.
    /// </summary>
    public partial class InfoPage : PhoneApplicationPage
    {
        /// <summary>
        /// Creates a new info page.
        /// </summary>
        public InfoPage()
        {
            InitializeComponent();
            //PageTransition.Completed += new EventHandler(PageTransition_Completed);

            if (!App.ViewModel.IsDataLoaded)
                App.ViewModel.CreateFromXml(App.ViewModel.Xml);

            //if (App.ViewModel.MensaIndex != -1)
            //    this.DataContext = App.ViewModel.Days[App.ViewModel.MensaIndex];
            this.DataContext = App.ViewModel;
        }

        /// <summary>
        /// When the Back-Key was pressed, go back with transition
        /// </summary>
        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Cancel the default navigation
            e.Cancel = true;

            goBack();
        }

        /// <summary>
        /// Goes back to the last page with transition
        /// </summary>
        private void goBack()
        {
            PhoneApplicationFrame root = (PhoneApplicationFrame)Application.Current.RootVisual;
            root.GoBack();
        }

        /// <summary>
        /// When the page transition is completed, go back to the last page
        /// </summary>
        void PageTransition_Completed(object sender, EventArgs e)
        {
            // Reset root frame to MainPage.xaml 
            PhoneApplicationFrame root = (PhoneApplicationFrame)Application.Current.RootVisual;
            root.GoBack();
        }
    }
}