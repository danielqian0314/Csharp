using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using das_settings;
using System.IO;
using Utilities.DataTypes.ExtensionMethods;
using Utilities.IO.ExtensionMethods;
using Utilities.Reflection.ExtensionMethods;
using Utilities.Math.ExtensionMethods;
using das_common.SOAP;
using DasWs.Services.ExchangeDataType;

namespace DasWs.Services
{
    public class PamUpdateRequestHandler
    {
        private readonly Isap_push_to_das sapPushSettings;
        private readonly SoapClient soapClient;
        private readonly SoapRetSaveSvc saveSvc;

        public PamUpdateRequestHandler(Isap_push_to_das sapPushSettings, SoapClient soapClient, SoapRetSaveSvc saveSvc)
        {
            this.saveSvc = saveSvc;
            this.soapClient = soapClient;
            this.sapPushSettings = sapPushSettings;
        }

        public string GetUpdateXml(string pn)
        {
            SoapLabelPrintResult soapRet = null;
            Exception soapExp = null;
            
            try
            {
                soapRet = soapClient.GetLabelPrintResult(pn);
            }
            catch (Exception exp)
            {
                soapExp = exp;
            }

            UpdateResult ret = null;

            if (soapRet != null && soapRet.IsNoError)
            {
                ret = GenUpdateResultFromSoapResult(soapRet);
                this.saveSvc.SaveToLocal(soapRet);
                
            }
            else
            {
                ret = this.GetDasCacheUpdateResult(pn);
                ret.SoapErrorMsg = GetSoapErrorMsg(soapExp, soapRet);
            }

            return ret.ToXML("", Encoding.ASCII);
        }

  
        private string GetSoapErrorMsg(Exception soapExp, SoapLabelPrintResult soapRet)
        {
            StringBuilder builder = new StringBuilder(512);

            if (soapRet != null && !soapRet.IsNoError)
            {
                builder.AppendLine("SoapErrorType:" + soapRet.ErrorType);
                builder.AppendLine("SoapErrorMsg:" + soapRet.ErrorMessage);                
            }

            if (soapExp != null)
            {
                builder.AppendLine(soapExp.ToString());

                builder.AppendLine(soapExp.GetDetailErrorText());
            }

            return builder.ToString();
        }

        private UpdateResult GenUpdateResultFromSoapResult(SoapLabelPrintResult soapRet)
        {
            UpdateResult ret = new UpdateResult();
            ret.IsOK = soapRet.IsNoError;

            ret.ErrorCode = "";
            ret.LabelUpdateSource = "SAP";

            ret.SoapErrorMsg = "";
           
            if (!soapRet.Files.IsNullOrEmpty())
            {
                var fileItem = soapRet.Files.First();
                ret.LabelFileName = fileItem.FileName;
                ret.CLPBasicData = new CLPBasicData() { TimestampFormat = fileItem.TimestampFormat };
                ret.LabelFileBase64Content = fileItem.Content.ToBase64String();
            }
            
            return ret;
        }

        internal UpdateResult GetDasCacheUpdateResult(string pn)
        {   
            UpdateResult ret = new UpdateResult();

            string pdfFullPath = GetTargetPDF(pn);
            string xmlFullPath = GetBasicDataXml(pdfFullPath);

            bool isBothFileOk = !pdfFullPath.IsNullOrEmpty() && !xmlFullPath.IsNullOrEmpty();

            if (isBothFileOk)
            {
                ret.LabelFileName = Path.GetFileName(pdfFullPath);
                ret.LabelUpdateSource = "DAS";
                var bytesContent = File.ReadAllBytes(pdfFullPath);
                ret.LabelFileBase64Content = bytesContent.ToBase64String();

                var xmlContent = File.ReadAllText(xmlFullPath);
                ret.CLPBasicData = xmlContent.XMLToObject<CLPBasicData>(Encoding.UTF8);
                ret.IsOK = true;
            }
            else
            {
                ret.LabelFileName = "N/A";
                ret.LabelUpdateSource = "DAS";
                ret.IsOK = false;
                ret.ErrorCode = "";
                if (pdfFullPath.IsNullOrEmpty()) ret.ErrorCode += " LABEL_FILE_NOT_FOUND"; 
                if (xmlFullPath.IsNullOrEmpty()) ret.ErrorCode += " BASIC_DATA_FILE_NOT_FOUND"; 
            }

            return ret;
                    

        }

        private static string GetBasicDataXml(string pdfFullPath)
        {
            if (pdfFullPath == null) return null;

            var path =  pdfFullPath.ToLower().Replace(".pdf", ".xml");

            if (File.Exists(path))
            {
                return path;
            }
            else
            {
                return null;
            }
        }

        private string GetTargetPDF(string pn)
        {
            string dir = this.sapPushSettings.PushStoredFolder;
            var targetFiles = Directory.GetFiles(dir, "*" + pn + "*.pdf").ToList();

            if (targetFiles.IsNullOrEmpty()) return null;

            return targetFiles.Select(x => new FileInfo(x))
                              .OrderByDescending(x => x.LastWriteTime)
                              .First().FullName;
                                        
        }
    }

   
}
