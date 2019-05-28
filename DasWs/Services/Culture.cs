using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace DasWs.Services
{
    public static class Culture
    {

        private static CultureInfo De = CultureInfo.GetCultureInfo("de-DE");

        private static string _ddMMyyHHmm = "dd.MM.yy HH:mm";
        private static string _F2 = "F2";

        public static string F2
        {
            get { return Culture._F2; }
        }


        public static CultureInfo UI
        {
            get { return De; }
        }

        public static string ddMMyyHHmm
        {
            get { return _ddMMyyHHmm; }
        }
    }
}
