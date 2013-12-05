using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.IO;
using System.Diagnostics;
using System;
using PhoneKit.Framework.Core.Storage;

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

            // load the image stream from isolated storage
            using (var iconStream = StorageHelper.GetFileStream(iconPath))
            {
                if (iconStream == null)
                    return;

                try
                {
                    bmi.SetSource(iconStream);
                    Icon.Source = bmi;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Setting the tiles icon image failed. Error: " + e.Message);
                }
            }
        }
    }
}
