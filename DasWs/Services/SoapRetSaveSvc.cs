using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using das_common.SOAP;
using System.IO;
using das_settings;
using DasWs.Services.ExchangeDataType;
using Utilities.DataTypes.ExtensionMethods;
using Utilities.IO.ExtensionMethods;
using Utilities.Reflection.ExtensionMethods;
using Utilities.Math.ExtensionMethods;


namespace DasWs.Services
{
    public class SoapRetSaveSvc
    {

        private readonly Isap_push_to_das sapPushSettings;

         public SoapRetSaveSvc(Isap_push_to_das sapPushSettings)
         {         
            this.sapPushSettings = sapPushSettings;
         }

        public void SaveToLocal(SoapLabelPrintResult soapRet)
        {
            if (soapRet != null && soapRet.Files != null)
            {
                foreach (var item in soapRet.Files)
                {
                    if (IsFileFromFirstPrinter(item))
                    {
                        var pdfFullpath = Path.Combine(this.sapPushSettings.PushStoredFolder, item.FileName);
                        var xmlFullPath = pdfFullpath.ToLower().Replace(".pdf", ".xml");

                        File.WriteAllBytes(pdfFullpath, item.Content);

                        CLPBasicData basicData = new CLPBasicData { TimestampFormat = item.TimestampFormat };
                        File.WriteAllText(xmlFullPath, basicData.ToXML("", Encoding.UTF8), Encoding.UTF8);
                    }
                    else
                    {
                        //ignore
                    }
                }
            }
        }

        private bool IsFileFromFirstPrinter(SoapFileItem item)
        {
            return item.FileName.StartsWith("20_");
        }

    }
}
