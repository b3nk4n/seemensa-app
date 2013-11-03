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
using System.Text;
using Microsoft.Phone.Tasks;

namespace SeeMensa
{
    /// <summary>
    /// The about page.
    /// </summary>
    public partial class AboutPage : PhoneApplicationPage
    {
        private MarketplaceReviewTask reviewTask = new MarketplaceReviewTask();

        /// <summary>
        /// Creates a new about page.
        /// </summary>
        public AboutPage()
        {
            InitializeComponent();

            this.loadVersion();
        }

        /// <summary>
        /// Loads the current version from assembly.
        /// </summary>
        private void loadVersion()
        {
            System.Reflection.AssemblyName an = new System.Reflection.AssemblyName(System.Reflection.Assembly
                                                                                   .GetExecutingAssembly()
                                                                                   .FullName);
            this.tbVersion.Text = new StringBuilder().Append(an.Version.Major)
                                                     .Append('.')
                                                     .Append(an.Version.Minor)
                                                     .ToString();
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

        private void HubTile_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            reviewTask.Show();
        }
    }
}