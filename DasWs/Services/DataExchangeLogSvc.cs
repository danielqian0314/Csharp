using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Utilities.DataTypes.ExtensionMethods;
using Utilities.IO.ExtensionMethods;
using Utilities.Reflection.ExtensionMethods;
using Utilities.Math.ExtensionMethods;
using das_common.WebSocket;
using System.Diagnostics;


namespace DasWs.Services
{
    public class DataExchangeLogSvc
    {
        private static List<DataExchangeLogItem> list = new List<DataExchangeLogItem>();
        private volatile static int Id = 1;
        
        static DataExchangeLogSvc()
        {
            AddDummyTestItem();
        }

        [Conditional("DEBUG")]
        private static void AddDummyTestItem()
        {
            DataExchangeLogItem item = new DataExchangeLogItem
            {
                ID = 0,
                Time = DateTime.Now,
                IP = "127.0.0.1",
                Type = "PUSH",
                Parameter = "123"
            };

            list.Add(item);
        }

        private WebSocketHost webSocket = null;

        public DataExchangeLogSvc(WebSocketHost webSocket)
        {
            this.webSocket = webSocket;            
        }

        public void LogRequest(IPAddress ip, Dictionary<string, string> dictionary, string RequestType = "REQUEST")
        {
            var txtparam = string.Join("&",dictionary.Select(x => x.Key + "=" + x.Value));
            DataExchangeLogItem item = new DataExchangeLogItem
            {
                Time = DateTime.Now,
                IP = ip.ToString(),
                Type = RequestType,
                Parameter = txtparam
            };

            AddToList(item);

            webSocket.PushMessageAllClinet("NEW_REQUEST");
        }

        public void LogPush(IPAddress ip, string requestBody,string filePath)
        {
            var length = (requestBody == null) ? 0 : requestBody.Length;

            DataExchangeLogItem item = new DataExchangeLogItem
            {
                Time = DateTime.Now,
                IP = ip.ToString(),
                Type = "PUSH",
                Parameter = length + " bytes received ==> " + filePath
            };

            AddToList(item);

            webSocket.PushMessageAllClinet("NEW_PUSH");
        }


        private void AddToList(DataExchangeLogItem item)
        {
            lock (list)
            {
                item.ID = Id;
                IDSelfIncrease();
                list.Add(item);

                if (list.Count > 100)
                {
                    list.RemoveAt(0);
                }
            }            
        }

        private static void IDSelfIncrease()
        {
            Id++;
            if (Id == int.MaxValue)
            {
                Id = 0;
            }
        }

        public List<DataExchangeLogItem> GetLogItems()
        {           
            var retList = new List<DataExchangeLogItem>(list);
            return retList;
        }

        internal void LogRequest(HttpListenerRequest httpRequest)
        {
            DataExchangeLogItem item = new DataExchangeLogItem
            {
                Time = DateTime.Now,
                IP = httpRequest.RemoteEndPoint.ToString(),
                Type = "REQUEST",
                Parameter = httpRequest.RawUrl.SubStringAfter("?",1)
            };

            AddToList(item);

            webSocket.PushMessageAllClinet("NEW_REQUEST");
        }
    }

    public class DataExchangeLogItem
    {
        public int ID { get; set; }

        public DateTime Time { get; set; }

        public string IP { get; set; }

        public string Type { get; set; }

        public string Parameter { get; set; }

    }
}
