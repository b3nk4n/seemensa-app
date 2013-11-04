using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Windows.ApplicationModel.Store;
using Store = Windows.ApplicationModel.Store;
using System.Collections.ObjectModel;
using SeeMensa.InAppPurchases;

namespace SeeMensa
{
    public partial class InAppStorePage : PhoneApplicationPage
    {
        public InAppStorePage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// When the Back-Key was pressed, go back with transition
        /// </summary>
        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Cancel the default navigation
            e.Cancel = true;

            goBack();
        }

        /// <summary>
        /// Goes back to the last page with transition
        /// </summary>
        private void goBack()
        {
            PhoneApplicationFrame root = (PhoneApplicationFrame)Application.Current.RootVisual;
            root.GoBack();
        }

        public ObservableCollection<ProductItem> picItems = new ObservableCollection<ProductItem>();

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            RenderStoreItems();
            base.OnNavigatedTo(e);
        }

        private async void RenderStoreItems()
        {
            picItems.Clear();

            try
            {
                ListingInformation li = await Store.CurrentApp.LoadListingInformationAsync();

                if (li.ProductListings.Count > 0)
                {
                    foreach (string key in li.ProductListings.Keys)
                    {
                        ProductListing pListing = li.ProductListings[key];
                        System.Diagnostics.Debug.WriteLine(key);

                        string status = Store.CurrentApp.LicenseInformation.ProductLicenses[key].IsActive ? SeeMensa.Language.Language.StorePurchased : pListing.FormattedPrice;

                        string imageLink = string.Empty;
                        picItems.Add(
                            new ProductItem
                            {
                                ImgLink = key.Equals("ad_free") ? "/Images/ad_free.png" : "/Images/donate.png",
                                Name = pListing.Name,
                                Description = pListing.Description,
                                Status = status,
                                Key = key,
                                BuyNowButtonVisible = Store.CurrentApp.LicenseInformation.ProductLicenses[key].IsActive ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible
                            }
                        );
                    }

                    pics.ItemsSource = picItems;
                }
                else
                {
                    MessageBox.Show(SeeMensa.Language.Language.MessageBoxNoProducts);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
        }

        private async void ButtonBuyNow_Clicked(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            string key = btn.Tag.ToString();

            if (!Store.CurrentApp.LicenseInformation.ProductLicenses[key].IsActive)
            {
                ListingInformation li = await Store.CurrentApp.LoadListingInformationAsync();
                string pID = li.ProductListings[key].ProductId;

                string receipt = await Store.CurrentApp.RequestProductPurchaseAsync(pID, false);

                RenderStoreItems();
            }
        }
    }
}