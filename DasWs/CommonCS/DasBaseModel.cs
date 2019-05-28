using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebServer.Infrasturcture.MVC;
using Microsoft.Practices.Unity;
using WebServer.Infrasturcture;
using System.Reflection;
using Utilities.DataTypes.ExtensionMethods;
using Utilities.IO.ExtensionMethods;
using Utilities.Reflection.ExtensionMethods;
using Utilities.Math.ExtensionMethods;
using System.IO;
using Utilities.IO;
using das_settings;


namespace DasWs.CommonCS
{
    public class DasBaseModel : ModelBase
    {
        private static string VersionNumber = "";
        private static string TimestampCode = "0";

        static DasBaseModel()
        {
            VersionNumber = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            string versionDat = DirectoryHelper.CombineWithCurrentExeDir("version.dat");
            if (File.Exists(versionDat))
            {
                TimestampCode = File.ReadAllText(DirectoryHelper.CombineWithCurrentExeDir("version.dat"));
            }
        }

        private string  moduleEtag;

        private Dictionary<string, string> dict = new Dictionary<string, string>();

        public DasBaseModel()
        {
            moduleEtag = typeof(DasBaseModel).Assembly.GetName().Version.ToString();
        }

        private cbb_http_web_server webServerConfig;

        public string MainPage { get; set; }

        [Dependency]
        public cbb_http_web_server WebServerConfig
        {
            get { return webServerConfig; }
            set { webServerConfig = value; }
        }

        [Dependency]
        public Idas_common das_common_setting { get; set; }

        public string DasVersion
        {
            get { return VersionNumber + "." + TimestampCode; }

        }

        public string DasName
        {
            get { return das_common_setting.DasName; }
        }

        public string DasID
        {
            get { return das_common_setting.DasID; }
        }

        public string GetActiveStatus(string controllerName)
        {
            return controllerName.IsSameAs(MainPage) ? "class='active'" : "";
        }

       
    }
}
