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
using System.Xml.Linq;
using System.Collections.Generic;
using System.Text;

namespace SeeMensa.ViewModels
{
    /// <summary>
    /// ViewModel that represents a meal.
    /// </summary>
    public class MealViewModel : ViewModelBase
    {
        #region Members

        /// <summary>
        /// The category/ Title of the meal.
        /// </summary>
        private string _category;

        /// <summary>
        /// The title/description of the meal.
        /// </summary>
        private string _title;

        /// <summary>
        /// The description of the meal.
        /// </summary>
        private string _description;

        /// <summary>
        /// The tags of the meal.
        /// </summary>
        private string _kennzeichnungen;

        /// <summary>
        /// The dishes of the meal.
        /// </summary>
        private string _beilagen;

        /// <summary>
        /// The student price.
        /// </summary>
        private string _preis1;

        /// <summary>
        /// The employee price.
        /// </summary>
        private string _preis2;

        /// <summary>
        /// The guest price
        /// </summary>
        private string _preis3;

        /// <summary>
        /// The pupil price.
        /// </summary>
        private string _preis4;

        /// <summary>
        /// The unit of the meal.
        /// </summary>
        private string _einheit;

        /// <summary>
        /// The signs/type of the meal.
        /// </summary>
        private string _signs;

        #endregion

        #region Constructors

        /// <summary>
        /// Creats a MealViewModel by parsing a XElement
        /// </summary>
        /// <param name="xmlMeal">The Meal as ea XML Element.</param>
        public MealViewModel(XElement xmlMeal)
        {
            _category = xmlMeal.Element("category").Value;
            _title = xmlMeal.Element("title").Value;
            _description = xmlMeal.Element("description").Value;
            _kennzeichnungen = xmlMeal.Element("kennzeichnungen").Value;
            _beilagen = xmlMeal.Element("beilagen").Value;
            _preis1 = xmlMeal.Element("preis1").Value;
            _preis2 = xmlMeal.Element("preis2").Value;
            _preis3 = xmlMeal.Element("preis3").Value;
            _preis4 = xmlMeal.Element("preis4").Value;
            _einheit = xmlMeal.Element("einheit").Value;

            // Replace tasg in title (= meals description)
            if (!string.IsNullOrEmpty(_kennzeichnungen))
            {
                _title = _title.Replace(string.Format(" ({0})", _kennzeichnungen), "");
            }

            // Check title and pricing
            _title = _title.Replace("&quot;", "\"");
            if (_preis1.Equals("0,00"))
            {
                string end = _title.Substring(_title.Length - 4);
                float res;
                if (float.TryParse(end, out res))
                {
                    _preis1 = _preis2 = _preis3 = _preis4 = end;
                }

                _title = _title.Substring(0, _title.Length - 4);
            }
            _title = _title.TrimEnd();

            // complete "Eing" to "Eingang"
            if (_title.EndsWith("Uni-Eing"))
            {
                _title += "ang";
            }

            _signs = ReadSigns(ref _title);
        }

        /// <summary>
        /// Reads the signs in the title/meal description.
        /// </summary>
        /// <param name="title">The meal description</param>
        /// <returns> The comma seperated signs as string.</returns>
        private static string ReadSigns(ref string title)
        {
            List<string> signList = new List<string>();

            // Vegetarian
            if (title.Contains(" Veg "))
            {
                title = title.Replace(" Veg ", " ");
                signList.Add("Veg");
            }
            else if (title.Contains(" (Veg)"))
            {
                title = title.Replace(" (Veg)", "");
                signList.Add("Veg");
            }
            else if (title.Contains(" ( Veg)"))
            {
                title = title.Replace(" ( Veg)", "");
                signList.Add("Veg");
            }
            else if (title.EndsWith(" Veg"))
            {
                title = title.Substring(0, title.Length - 4);
                signList.Add("Veg");
            }

            // Pig
            if (title.Contains(" Sch "))
            {
                title = title.Replace(" Sch ", " ");
                signList.Add("S");
            }
            else if (title.Contains(" (Sch)"))
            {
                title = title.Replace(" (Sch)", "");
                signList.Add("S");
            }
            else if (title.EndsWith(" Sch"))
            {
                title = title.Substring(0, title.Length - 4);
                signList.Add("S");
            }
            else if (title.Contains(" P "))
            {
                title = title.Replace(" P ", " ");
                signList.Add("P");
            }
            else if (title.Contains(" (P)"))
            {
                title = title.Replace(" (P)", "");
                signList.Add("P");
            }
            else if (title.EndsWith(" P"))
            {
                title = title.Substring(0, title.Length - 2);
                signList.Add("P");
            }

            // Beef
            if (title.Contains(" R "))
            {
                title = title.Replace(" R ", " ");
                signList.Add("R");
            }
            else if (title.Contains(" (R)"))
            {
                title = title.Replace(" (R)", "");
                signList.Add("R");
            }
            else if (title.EndsWith(" R"))
            {
                title = title.Substring(0, title.Length - 2);
                signList.Add("R");
            }
            else if (title.Contains(" B "))
            {
                title = title.Replace(" B ", " ");
                signList.Add("B");
            }
            else if (title.Contains(" (B)"))
            {
                title = title.Replace(" (B)", "");
                signList.Add("B");
            }
            else if (title.EndsWith(" B"))
            {
                title = title.Substring(0, title.Length - 2);
                signList.Add("B");
            }
            // Fish
            if (title.Contains(" F "))
            {
                title = title.Replace(" F ", " ");
                signList.Add("F");
            }
            else if (title.Contains(" (F)"))
            {
                title = title.Replace(" (F)", "");
                signList.Add("F");
            }
            else if (title.EndsWith(" F"))
            {
                title = title.Substring(0, title.Length - 2);
                signList.Add("F");
            }
            // Lamb
            if (title.Contains(" L "))
            {
                title = title.Replace(" L ", " ");
                signList.Add("L");
            }
            else if (title.Contains(" (L)"))
            {
                title = title.Replace(" (L)", "");
                signList.Add("L");
            }
            else if (title.EndsWith(" L"))
            {
                title = title.Substring(0, title.Length - 2);
                signList.Add("L");
            }
            // Veal
            if (title.Contains(" K "))
            {
                title = title.Replace(" K ", " ");
                signList.Add("K");
            }
            else if (title.Contains(" (K)"))
            {
                title = title.Replace(" (K)", "");
                signList.Add("K");
            }
            else if (title.EndsWith(" K"))
            {
                title = title.Substring(0, title.Length - 2);
                signList.Add("K");
            }
            else if (title.Contains(" V "))
            {
                title = title.Replace(" V ", " ");
                signList.Add("V");
            }
            else if (title.Contains(" (V)"))
            {
                title = title.Replace(" (V)", "");
                signList.Add("V");
            }
            else if (title.EndsWith(" V"))
            {
                title = title.Substring(0, title.Length - 2);
                signList.Add("V");
            }
            // Poultry
            if (title.Contains(" G "))
            {
                title = title.Replace(" G ", " ");
                signList.Add("G");
            }
            else if (title.Contains(" (G)"))
            {
                title = title.Replace(" (G)", "");
                signList.Add("G");
            }
            else if (title.EndsWith(" G"))
            {
                title = title.Substring(0, title.Length - 2);
                signList.Add("G");
            }
            else if (title.Contains(" Po "))
            {
                title = title.Replace(" Po ", " ");
                signList.Add("Po");
            }
            else if (title.Contains(" (Po)"))
            {
                title = title.Replace(" (Po)", "");
                signList.Add("Po");
            }
            else if (title.EndsWith(" Po"))
            {
                title = title.Substring(0, title.Length - 2);
                signList.Add("Po");
            }

            // Pig/Beef
            if (title.Contains(" Sch/R "))
            {
                title = title.Replace(" (Sch/R) ", " ");
                signList.Add("S");
                signList.Add("R");
            }
            else if (title.Contains(" (Sch/R)"))
            {
                title = title.Replace(" (Sch/R)", "");
                signList.Add("S");
                signList.Add("R");
            }
            else if (title.EndsWith(" Sch/R"))
            {
                title = title.Substring(0, title.Length - 4);
                signList.Add("S");
                signList.Add("R");
            }
            else if (title.Contains(" P/B "))
            {
                title = title.Replace(" P/B ", " ");
                signList.Add("P");
                signList.Add("B");
            }
            else if (title.Contains(" (P/B)"))
            {
                title = title.Replace(" (P/B)", "");
                signList.Add("P");
                signList.Add("B");
            }
            else if (title.EndsWith(" P/B"))
            {
                title = title.Substring(0, title.Length - 2);
                signList.Add("P");
                signList.Add("B");
            }

            // Pig/Veg
            if (title.Contains(" Sch/Veg "))
            {
                title = title.Replace(" (Sch/Veg) ", " ");
                signList.Add("S");
                signList.Add("Veg");
            }
            else if (title.Contains(" (Sch/Veg)"))
            {
                title = title.Replace(" (Sch/Veg)", "");
                signList.Add("S");
                signList.Add("Veg");
            }
            else if (title.EndsWith(" Sch/Veg"))
            {
                title = title.Substring(0, title.Length - 4);
                signList.Add("S");
                signList.Add("Veg");
            }
            else if (title.Contains(" P/Veg "))
            {
                title = title.Replace(" P/Veg ", " ");
                signList.Add("P");
                signList.Add("Veg");
            }
            else if (title.Contains(" (P/Veg)"))
            {
                title = title.Replace(" (P/Veg)", "");
                signList.Add("P");
                signList.Add("Veg");
            }
            else if (title.EndsWith(" P/Veg"))
            {
                title = title.Substring(0, title.Length - 2);
                signList.Add("P");
                signList.Add("Veg");
            }

            // Veg/Pig
            if (title.Contains(" Veg/Sch "))
            {
                title = title.Replace(" (Veg/Sch) ", " ");
                signList.Add("Veg");
                signList.Add("S");
            }
            else if (title.Contains(" (Veg/Sch)"))
            {
                title = title.Replace(" (Veg/Sch)", "");
                signList.Add("Veg");
                signList.Add("S");
            }
            else if (title.EndsWith(" Veg/Sch"))
            {
                title = title.Substring(0, title.Length - 4);
                signList.Add("Veg");
                signList.Add("S");
            }
            else if (title.Contains(" Veg/P "))
            {
                title = title.Replace(" Veg/P ", " ");
                signList.Add("Veg");
                signList.Add("P");
            }
            else if (title.Contains(" (Veg/P)"))
            {
                title = title.Replace(" (Veg/P)", "");
                signList.Add("Veg");
                signList.Add("P");
            }
            else if (title.EndsWith(" Veg/P"))
            {
                title = title.Substring(0, title.Length - 2);
                signList.Add("Veg");
                signList.Add("P");
            }

            // Po/F
            if (title.Contains(" G/F "))
            {
                title = title.Replace(" (G/F) ", " ");
                signList.Add("G");
                signList.Add("F");
            }
            else if (title.Contains(" (G/F)"))
            {
                title = title.Replace(" (G/F)", "");
                signList.Add("G");
                signList.Add("F");
            }
            else if (title.EndsWith(" G/F"))
            {
                title = title.Substring(0, title.Length - 4);
                signList.Add("G");
                signList.Add("F");
            }
            else if (title.Contains(" Po/F "))
            {
                title = title.Replace(" Po/F ", " ");
                signList.Add("Po");
                signList.Add("F");
            }
            else if (title.Contains(" (Po/F)"))
            {
                title = title.Replace(" (Po/F)", "");
                signList.Add("Po");
                signList.Add("F");
            }
            else if (title.EndsWith(" Po/F"))
            {
                title = title.Substring(0, title.Length - 2);
                signList.Add("Po");
                signList.Add("F");
            }

            // Beef/Pig
            if (title.Contains(" R/Sch "))
            {
                title = title.Replace(" (R/Sch) ", " ");
                signList.Add("R");
                signList.Add("S");
            }
            else if (title.Contains(" (R/Sch)"))
            {
                title = title.Replace(" (R/Sch)", "");
                signList.Add("R");
                signList.Add("S");
            }
            else if (title.EndsWith(" R/Sch"))
            {
                title = title.Substring(0, title.Length - 4);
                signList.Add("R");
                signList.Add("S");
            }
            else if (title.Contains(" B/P "))
            {
                title = title.Replace(" R/P ", " ");
                signList.Add("B");
                signList.Add("P");
            }
            else if (title.Contains(" (B/P)"))
            {
                title = title.Replace(" (B/P)", "");
                signList.Add("B");
                signList.Add("P");
            }
            else if (title.EndsWith(" B/P"))
            {
                title = title.Substring(0, title.Length - 2);
                signList.Add("B");
                signList.Add("P");
            }

            var sb = new StringBuilder();

            for (int i = 0; i < signList.Count; ++i)
            {
                if (i != 0)
                {
                    sb.Append(",");
                }

                sb.Append(signList[i]);
            }

            return sb.ToString();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets ot sets the category.
        /// </summary>
        public string Category
        {
            get
            {
                return _category;
            }
            set
            {
                if (_category != value)
                {
                    RaisePropertyChanged("Category");
                    _category = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (_title != value)
                {
                    RaisePropertyChanged("Title");
                    _title = value;
                }
            }
        }

        /// <summary>
        /// gets ot sets the description.
        /// </summary>
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                if (_description != value)
                {
                    RaisePropertyChanged("Description");
                    _description = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        public string Kennzeichnungen
        {
            get
            {
                return _kennzeichnungen;
            }
            set
            {
                if (_kennzeichnungen != value)
                {
                    RaisePropertyChanged("Kennzeichnungen");
                    _kennzeichnungen = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the dishes.
        /// </summary>
        public string Beilagen
        {
            get
            {
                return _beilagen;
            }
            set
            {
                if (_beilagen != value)
                {
                    RaisePropertyChanged("Beilagen");
                    _beilagen = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the student price.
        /// </summary>
        public string Preis1
        {
            get
            {
                return _preis1;
            }
            set
            {
                if (_preis1 != value)
                {
                    RaisePropertyChanged("Preis1");
                    _preis1 = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the employee price.
        /// </summary>
        public string Preis2
        {
            get
            {
                return _preis2;
            }
            set
            {
                if (_preis2 != value)
                {
                    RaisePropertyChanged("Preis2");
                    _preis2 = value;
                }
            }
        }

        /// <summary>
        /// Gets ot sets the guest price.
        /// </summary>
        public string Preis3
        {
            get
            {
                return _preis3;
            }
            set
            {
                if (_preis3 != value)
                {
                    RaisePropertyChanged("Preis3");
                    _preis3 = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the pupil price.
        /// </summary>
        public string Preis4
        {
            get
            {
                return _preis4;
            }
            set
            {
                if (_preis4 != value)
                {
                    RaisePropertyChanged("Preis4");
                    _preis4 = value;
                }
            }
        }

        /// <summary>
        /// Gets the price by the selected type.
        /// </summary>
        public string DisplayPrice
        {
            get
            {
                string priceToReturn = "";

                switch (App.ViewModel.PriceType)
                {
                    case PriceType.Guest:
                        priceToReturn = _preis3;
                        break;

                    case PriceType.Employee:
                        priceToReturn = _preis2;
                        break;

                    case PriceType.Pupil:
                        priceToReturn = _preis4;
                        break;

                    default:
                        priceToReturn = _preis1;
                        break;
                }

                if (priceToReturn.Equals("0,00") || priceToReturn.Equals("0.00"))
                    return string.Empty;
                else
                    return priceToReturn + " €";
            }
        }

        /// <summary>
        /// Gets or sets the unit.
        /// </summary>
        public string Einheit
        {
            get
            {
                return _einheit;
            }
            set
            {
                if (_einheit != value)
                {
                    RaisePropertyChanged("Einheit");
                    _einheit = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the meal signs.
        /// </summary>
        public string Signs
        {
            get
            {
                return _signs;
            }
            set
            {
                if (_signs != value)
                {
                    RaisePropertyChanged("Signs");
                    _signs = value;
                }
            }
        }

        #endregion
    }
}
