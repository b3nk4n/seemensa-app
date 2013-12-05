using PhoneKit.Framework.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeMensa.Controls
{
    /// <summary>
    /// The localized in-app store control.
    /// </summary>
    class LocalizedInAppStoreControl : InAppStoreControlBase
    {
        /// <summary>
        /// Localizes the user control content and texts.
        /// </summary>
        protected override void LocalizeContent()
        {
            InAppStoreLoadingText = SeeMensa.Language.Language.InAppStoreLoading;
            InAppStoreNoProductsText = SeeMensa.Language.Language.InAppStoreNoProducts;
            InAppStorePurchasedText = SeeMensa.Language.Language.InAppStorePurchased;
            SupportedProductIds = "ad_free,donate";
        }
    }
}
