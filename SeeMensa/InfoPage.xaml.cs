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

            if (!App.ViewModel.IsDataLoaded)
                App.ViewModel.CreateFromXml(App.ViewModel.Xml);

            this.DataContext = App.ViewModel;
        }
    }
}