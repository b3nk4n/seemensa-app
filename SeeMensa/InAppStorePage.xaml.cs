using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Windows.ApplicationModel.Store;
using Store = Windows.ApplicationModel.Store;
using System.Collections.ObjectModel;
using SeeMensa.InAppPurchases;

namespace SeeMensa
{
    public partial class InAppStorePage : PhoneApplicationPage
    {
        public InAppStorePage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// When the Back-Key was pressed, go back with transition
        /// </summary>
        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Cancel the default navigation
            e.Cancel = true;
        }
    }
}