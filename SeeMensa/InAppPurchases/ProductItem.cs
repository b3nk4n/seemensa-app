using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeMensa.InAppPurchases
{
    public class ProductItem
    {
        public string imgLink { get; set; }
        public string Status { get; set; }
        public string Name { get; set; }
        public string key { get; set; }
        public System.Windows.Visibility BuyNowButtonVisible { get; set; }
    }
}
