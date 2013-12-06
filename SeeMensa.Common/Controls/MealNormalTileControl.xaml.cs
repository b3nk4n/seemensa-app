using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.IO;
using System.Diagnostics;
using System;
using PhoneKit.Framework.Core.Storage;
using System.Windows.Resources;

namespace SeeMensa.Common.Controls
{
    /// <summary>
    /// Represents the UI for an wide live tile.
    /// </summary>
    public partial class MealNormalTileControl : UserControl
    {
        /// <summary>
        /// Creates a MealNormalTileControl instance.
        /// </summary>
        public MealNormalTileControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Creates a MealNormalTileControl instance.
        /// </summary>
        /// <param name="title">The meal title.</param>
        /// <param name="description">The meal description text.</param>
        /// <param name="iconStream">The stream to the icon image</param>
        public MealNormalTileControl(string title, string description, string iconPath)
            : this()
        {
            this.Title.Text = title;
            this.Description.Text = description;
            
            // set image source in code, because the XAML implementation is asyc,
            // so it is not going to be rendered.
            BitmapImage bmi = new BitmapImage();
            bmi.CreateOptions = BitmapCreateOptions.None;

            Uri uri = new Uri("/SeeMensa.Common;component/" + iconPath, UriKind.Relative);
            StreamResourceInfo resourceInfo = Application.GetResourceStream(uri);
            BitmapImage bmp = new BitmapImage();
            bmp.SetSource(resourceInfo.Stream);
            Icon.Source = bmp;
        }
    }
}
