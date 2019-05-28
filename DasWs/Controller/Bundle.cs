using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebServer.Infrasturcture;
using WebServer.Infrasturcture.MVC;
using DasWs.CommonCS;
using WebServer.Infrasturcture.IO;
using Utilities.DataTypes.ExtensionMethods;
using Utilities.IO.ExtensionMethods;
using Utilities.Reflection.ExtensionMethods;
using WebServerConfig;
using System.Reflection;
using System.IO;
using WebServer.Infrasturcture.DataModel;

namespace DasWs.Controller
{
    public class Bundle : BundleBase<BundleModel,object>
    {
        private cbb_http_web_server webServerConfig;

        public Bundle(cbb_http_web_server webServerConfig)
        {
            this.webServerConfig = webServerConfig;        
        }

        [AllowAnonymous]
        [AllowCache]
        public ActionRet CommonCssBundle()
        {
            List<string> bundleList = new List<string>() 
            {
                "/css/bootstrap.min.css",
                "/css/das.css",
            };

            string key = "CommonCssBundle";
            return GetCssBunlde(bundleList, key);
        }

        /*..*/


        [AllowAnonymous]
        [AllowCache]
        public ActionRet CommonJsBundle()
        {
            List<string> bundleList = new List<string>() 
            {
                "/js/jquery-1.11.1.min.js",
                "/js/bootstrap.min.js",
                "/js/mustache.min.js",
                "/js/jquery-ajax-setting.js"
            };
           
            string key = "CommonJsBundle";
           
            return GetJsBundle(bundleList, key);
        }

        protected override bool IsDevelopMode
        {
            get  { return this.webServerConfig.IsDevelopModeEnabled; }           
        }
    }

    public class BundleModel : ModelBase
    {

      
    }
}
