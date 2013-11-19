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
using SeeMensa.Common.ViewModels;

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

            if (!MainViewModel.Instance.IsDataLoaded)
                MainViewModel.Instance.CreateFromXml(MainViewModel.Instance.Xml);

            this.DataContext = MainViewModel.Instance;
        }
    }
}