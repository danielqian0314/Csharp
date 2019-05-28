using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using das_settings;
using System.IO;
using System.Net;
using Utilities.DataTypes.ExtensionMethods;
using Utilities.IO.ExtensionMethods;
using Utilities.Reflection.ExtensionMethods;
using Utilities.Math.ExtensionMethods;
using System.Threading;
using System.Diagnostics;


namespace DasWs.Services
{
    public class RequestToSapSvc
    {

        public RequestToSapSvc()
        {

        }

        public string Request()
        {
            DebugSleep(1000);



            return "";
        }

        [Conditional("DEBUG")]
        private void DebugSleep(int ms)
        {
            Thread.Sleep(ms);
        }

    }
}
