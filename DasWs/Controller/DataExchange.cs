using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebServer.Infrasturcture.MVC;
using Utilities.IO.Logging.Interfaces;
using Utilities.DataTypes.ExtensionMethods;
using Utilities.IO.ExtensionMethods;
using Utilities.Reflection.ExtensionMethods;
using WebServer.Infrasturcture;
using Microsoft.Practices.Unity;
using DasWs.CommonCS;
using WebServer.Infrasturcture.Status;
using System.IO;
using WebServer.Infrasturcture.IO;
using Utilities.IO;
using WebServer.Infrasturcture.Validation;
using Utilities.Serialization;

using DasWs.Services;

using System.Globalization;
using System.Reflection;
using System.Diagnostics;
using das_common.Utility;
using das_settings;


namespace DasWs.Controller
{
    public class DataExchange : DasControllerBase<DataExchangeModel, DataExchangeSession>
    {
        private IUnityContainer container;
        private readonly RuntimeLibLoader loader = new RuntimeLibLoader();
        private readonly DataExchangeLogSvc dataExchangeLogSvc;
        private readonly RequestToSapSvc requestToSapSvc;
        private readonly Isap_push_to_das sapPushSettings;
        private readonly PamUpdateRequestHandler pamUpdateHandler;


        public DataExchange(IUnityContainer container,
                            DataExchangeLogSvc dataExchangeLogSvc,
                            RequestToSapSvc requestToSapSvc,
                            Isap_push_to_das sapPushSettings,
                            PamUpdateRequestHandler pamUpdateHandler
                            )
        {
            this.pamUpdateHandler = pamUpdateHandler;
            this.sapPushSettings = sapPushSettings;
            this.requestToSapSvc = requestToSapSvc;
            this.container = container;
            this.dataExchangeLogSvc = dataExchangeLogSvc;
        }

        public override void OnLoad()
        {
            base.OnLoad();
        }

        [AllowAnonymous]
        public ActionRet GetUpdate()
        {
            var ip = HttpListenerContext.Request.RemoteEndPoint.Address;
            this.dataExchangeLogSvc.LogRequest(ip,RequestParameter.QueryDict);

            var pn = this.GetParameter<string>("pn", "");

            var xmlContent = pamUpdateHandler.GetUpdateXml(pn);

            return ForceDownloadFile(pn +".xml", xmlContent);                       
        }

        [AllowAnonymous]
        public ActionRet Push()
        {
            try
            {
                var ip = HttpListenerContext.Request.RemoteEndPoint.Address;
                var filePath = SaveRequestBody(this.RequestParameter.RequestBody);

                this.dataExchangeLogSvc.LogPush(ip, this.RequestParameter.RequestBody, filePath);

                return OutputText(this.RequestParameter.RequestBody);
            }
            catch 
            {
                return OutputText("ERROR");
            }
        }

        [AllowAnonymous]
        public ActionRet AjaxRequestToSAP()
        {
            return OutputText(this.requestToSapSvc.Request());
        }

        private Dictionary<string, string> GetBase64ParamDict()
        {
            string requestBody = this.RequestParameter.RequestBody;

            SaveRequestBody(requestBody);

            var pairArray = requestBody.Split('&');

            Dictionary<string, string> retDict = new Dictionary<string, string>();


            foreach (var pairItem in pairArray)
            {
                var itemArray = pairItem.Split('=');

                if (itemArray.Length == 2)
                {
                    retDict.Add(itemArray[0], itemArray[1]);
                }
            }

            return retDict;
        }
        
        private string SaveRequestBody(string requestBody)
        {
            string dir = this.sapPushSettings.PushStoredFolder;

            if (!Directory.Exists(dir)) { Directory.CreateDirectory(dir); }

            string filePath = Path.Combine(dir, Guid.NewGuid().ToString() + ".txt");            

            File.WriteAllText(filePath, requestBody);

            return filePath;
        }


    }

    public class DataExchangeModel : DasBaseModel
    {
       
    }

    public class DataExchangeSession
    {
     
    }
   
}

