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
using Microsoft.Phone.Tasks;

namespace SeeMensa
{
    /// <summary>
    /// The help page.
    /// </summary>
    public partial class HelpPage : PhoneApplicationPage
    {
        /// <summary>
        /// Creates a new help page.
        /// </summary>
        public HelpPage()
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

        private void SupportLinkClicked_Click(object sender, RoutedEventArgs e)
        {
            EmailComposeTask emailTask = new EmailComposeTask();
            emailTask.To = "apps@bsautermeister.de";
            emailTask.Subject = "seeMENSA-Support";
            emailTask.Show();
        }
    }
}