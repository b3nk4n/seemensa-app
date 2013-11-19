using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.IO;

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
        public MealNormalTileControl(string title, string description, string iconPath)
            : this()
        {
            this.Title.Text = title;
            this.Description.Text = description;

            // set image source in code, because the XAML implementation is asyc,
            // so it is not going to be rendered.
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
