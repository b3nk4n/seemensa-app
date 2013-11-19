using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.IO;

namespace SeeMensa.Controls
{
    /// <summary>
    /// Represents the UI for an wide live tile.
    /// </summary>
    public partial class MealTileControl : UserControl
    {
        /// <summary>
        /// Creates a MealTileControl instance.
        /// </summary>
        public MealTileControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Creates a MealTileControl instance.
        /// </summary>
        /// <param name="title">The meal title.</param>
        /// <param name="description">The meal description text.</param>
        public MealTileControl(string title, string description, string iconPath)
            : this()
        {
            this.Title.Text = title;
            this.Description.Text = description;

            BitmapImage bmi = new BitmapImage();
            bmi.CreateOptions = BitmapCreateOptions.None;



            using (var file = new FileStream(iconPath, FileMode.Open))
            {
                bmi.SetSource(file);
                Icon.Source = bmi;
            }
        }
    }
}
