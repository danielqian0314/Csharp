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
using WebServerConfig;
using das_common.WebSocket;



namespace DasWs.Controller
{
    public class Status : DasControllerBase<StatusModel,StatusSession>
    {
        private IUnityContainer container;
        private readonly DataExchangeLogSvc dataExchangeLogSvc;
        private readonly Icbb_http_web_server serverSetting;
        private readonly SecureWebSocketHelper secureWebSocketHelper;
        private readonly das_settings.Iweb_socket_server wsconfig;

        public Status(IUnityContainer container,
                      DataExchangeLogSvc dataExchangeLogSvc,
                      Icbb_http_web_server serverSetting,
                       das_settings.Iweb_socket_server wsconfig,
                      SecureWebSocketHelper secureWebSocketHelper
            )
        {

            this.wsconfig = wsconfig;
            this.secureWebSocketHelper = secureWebSocketHelper;
            this.serverSetting = serverSetting;
            this.dataExchangeLogSvc = dataExchangeLogSvc;
            this.container = container;
        }

        public override void OnLoad()
        {
            base.OnLoad();
            this.Model.MainPage = "Status";
            this.Model.WsPort = wsconfig.Port;
            this.Model.IsWssEnabled = this.secureWebSocketHelper.IsWssEnabled();
        }

        [AllowAnonymous]
        public ActionRet Main()
        {
            return View("Status", this.Model);
        }

        [AllowAnonymous]
        public ActionRet AjaxGetExchangeLogJson()
        {
            var list = dataExchangeLogSvc.GetLogItems()
                       .Select(x => new DataExchangeLogItemVM(x))
                       .ToList();
            
            return OutputText("{ \"array\" : " + list.ToJSON() + "}");
        }

    }

    public class DataExchangeLogItemVM
    {
        public string ID { get; set; }
        public string IP { get; set; }
        public string Time { get; set; }
        public string Type { get; set; }
        public string Parameter { get; set; }

        public DataExchangeLogItemVM()
        {

        }

        public DataExchangeLogItemVM(DataExchangeLogItem rawItem)
        {
            ID = rawItem.ID.ToString().PadLeft(3, '0');
            Time = rawItem.Time.ToString(Culture.UI);

            if (rawItem.IP == "::1")
            {
                IP = "localhost".PadLeft(15, ' ');
            }
            else
            {
                IP = rawItem.IP.PadLeft(15,' ');
            }
            Type = rawItem.Type.PadLeft(15,' ');
            Parameter = rawItem.Parameter;
        }
    }

    public class StatusModel : DasBaseModel
    {
        public bool IsWssEnabled { get; set; }

        public string WsPort { get; set; }

        public string WebSocketSchema
        {
            get { return IsWssEnabled ? "wss" : "ws"; }
        }
    }

    public class StatusSession
    {
     
    }
   
}

