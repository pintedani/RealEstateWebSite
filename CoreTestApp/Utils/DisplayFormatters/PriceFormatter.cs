using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imobiliare.UI.Utils
{
    public static class PriceFormatter
    {
        public static string FormatPrice(int price)
        {
            return string.Format("{0:0,0}", price);
        }
    }
}