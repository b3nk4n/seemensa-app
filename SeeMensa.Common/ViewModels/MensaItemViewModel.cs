using PhoneKit.Framework.Core.MVVM;
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

namespace SeeMensa.Common.ViewModels
{
    public class MensaItemViewModel : ViewModelBase
    {
        #region Members

        /// <summary>
        /// The name of the mensa.
        /// </summary>
        private string _name;

        /// <summary>
        /// The German uri.
        /// </summary>
        private Uri _deUri;

        /// <summary>
        /// The English uri.
        /// </summary>
        private Uri _enUri;

        /// <summary>
        /// First address line.
        /// </summary>
        private string _address1;

        /// <summary>
        /// Second address line.
        /// </summary>
        private string _address2;

        /// <summary>
        /// The live tile image uri.
        /// </summary>
        private Uri _imageUri;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an MensaItemViewModel.
        /// </summary>
        /// <param name="name">The name of the mensa.</param>
        /// <param name="de">The German uri.</param>
        /// <param name="en">The English uri.</param>
        /// <param name="imagePath">The live tile image path.</param>
        public MensaItemViewModel(string name, Uri de, Uri en,
                                  string address1, string address2, string imagePath)
        {
            _name = name;
            _deUri = de;
            _enUri = en;
            _address1 = address1;
            _address2 = address2;
            _imageUri = new Uri(imagePath, UriKind.Relative);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Overrides for displaying the mensa name in the ListPicker control.
        /// </summary>
        /// <returns>The name of the mensa.</returns>
        public override string ToString()
        {
            return Name;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        /// <summary>
        /// Gets or sets the German Uri.
        /// </summary>
        public Uri DeUri
        {
            get
            {
                return _deUri;
            }
            set
            {
                if (value != _deUri)
                {
                    _deUri = value;
                    NotifyPropertyChanged("DeUri");
                }
            }
        }

        /// <summary>
        /// Gets or sets the English Uri.
        /// </summary>
        public Uri EnUri
        {
            get
            {
                return _enUri;
            }
            set
            {
                if (value != _enUri)
                {
                    _enUri = value;
                    NotifyPropertyChanged("EnUri");
                }
            }
        }

        /// <summary>
        /// Gets or sets the first address line.
        /// </summary>
        public string Address1
        {
            get
            {
                return _address1;
            }
            set
            {
                if (value != _address1)
                {
                    _address1 = value;
                    NotifyPropertyChanged("Address1");
                }
            }
        }

        /// <summary>
        /// Gets or sets the second address line.
        /// </summary>
        public string Address2
        {
            get
            {
                return _address2;
            }
            set
            {
                if (value != _address2)
                {
                    _address2 = value;
                    NotifyPropertyChanged("Address2");
                }
            }
        }

        /// <summary>
        /// Gets or sets the live tile background image.
        /// </summary>
        public Uri ImageUri
        {
            get
            {
                return _imageUri;
            }
            set
            {
                if (value != _imageUri)
                {
                    _imageUri = value;
                    NotifyPropertyChanged("ImageUri");
                }
            }
        }

        #endregion
    }
}
