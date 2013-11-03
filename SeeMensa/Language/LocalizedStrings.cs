using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SeeMensa.Language
{
    /// <summary>
    /// This class manages the localization.
    /// </summary>
    public class LocalizedStrings
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public LocalizedStrings()
        {
        }

        /// <summary>
        /// The localized string.
        /// </summary>
        private static Language _localizedString = new Language();

        /// <summary>
        /// Gets the localized string.
        /// </summary>
        public Language LocalizedString
        {
            get
            {
                return _localizedString;
            }
        }
    }
}
