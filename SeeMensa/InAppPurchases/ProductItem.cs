using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeMensa.InAppPurchases
{
    public class ProductItem
    {
        public string ImgLink { get; set; }
        public string Status { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Key { get; set; }
        public System.Windows.Visibility BuyNowButtonVisible { get; set; }
    }
}
