using Microsoft.Phone.Shell;
using PhoneKit.Framework.Graphics;
using PhoneKit.Framework.Storage;
using PhoneKit.Framework.Tile;
using SeeMensa.Common.Controls;
using SeeMensa.Common.ViewModels;
using System;
using System.Collections.Generic;

namespace SeeMensa.Common.LiveTile
{
    /// <summary>
    /// The seemensa live tile helper.
    /// </summary>
    public static class SeeMensaLiveTileHelper
    {
        /// <summary>
        /// Updates the live tile.
        /// </summary>
        public static void UpdateLiveTiles()
        {
            LiveTileHelper.ClearStorage();

            IList<Uri> images = CreateLiveTileImages();
            if (images.Count > 0)
            {
                LiveTileHelper.UpdateDefaultTile(new CycleTileData
                {
                    CycleImages = images,
                    SmallBackgroundImage = MainViewModel.Instance.CurrentMensaItem.ImageUri
                });
            }
        }

        /// <summary>
        /// Creates live tile images from mensa data.
        /// </summary>
        /// <returns>The list of images.</returns>
        private static IList<Uri> CreateLiveTileImages()
        {
            IList<Uri> images = new List<Uri>();
            var day = MainViewModel.Instance.Days[0];

            LiveTileHelper.ClearStorage();

            for (int i = 0; i < MainViewModel.Instance.Days[0].Meals.Count; ++i)
            {
                var image = GraphicsHelper.Create(
                    new MealNormalTileControl(
                        day.Meals[i].Category,
                        day.Meals[i].Title,
                        MainViewModel.Instance.CurrentMensaItem.ImageUri.OriginalString));
                images.Add(StorageHelper.SaveJpeg(
                    LiveTileHelper.SHARED_SHELL_CONTENT_PATH + string.Format("livetile{0}.jpeg", i), image));
            }

            return images;
        }
    }
}
