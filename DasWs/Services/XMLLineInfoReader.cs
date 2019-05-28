using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DasWs.Services
{
    public class XMLLineInfoReader:Controller.ILineInfo
    {
        public XmlDocument doc;
        public XMLLineInfoReader() 
        {
            this.doc = new XmlDocument();
            doc.Load("LineInfo.xml");
        }
        
        public List<int> getRegisteredLineNos(int WerkNo) 
        {
            List<int> RegisteredLineNos = new List<int>();
            XmlNode werkNode = doc.SelectSingleNode("//Werk[@werkNo='"+WerkNo+"']");
            XmlNodeList lineNodeList = werkNode.ChildNodes;
            foreach (XmlNode lineNode in lineNodeList) 
            {
                XmlElement lineElement = (XmlElement)lineNode;
                int LineNo = Int32.Parse(lineElement.GetAttribute("lineNo").ToString());
                RegisteredLineNos.Add(LineNo);           
            }
            return RegisteredLineNos;
        }

        public int getUMK_Total(int LineNo)
        {
            XmlNode UMKNode = doc.SelectSingleNode("//Line[@lineNo='" + LineNo + "']/UMK_Total");

            return Int32.Parse(UMKNode.InnerText);
 

        }

        public string getAssignment(int LineNo)
        {
            XmlNode AssignmentNode = doc.SelectSingleNode("//Line[@lineNo='" + LineNo +"']/Assignment");
            
            return AssignmentNode.InnerText;
        }



        public string getItem(int LineNo)
        {
            XmlNode ItemNode = doc.SelectSingleNode("//Line[@lineNo='" + LineNo + "']/Item");
            return ItemNode.InnerText;
        }

        public string getItemDescription(int LineNo)
        {
            XmlNode ItemDescriptionNode = doc.SelectSingleNode("//Line[@lineNo='" + LineNo + "']/ItemDescription");

            return ItemDescriptionNode.InnerText;
        }

        public string getManager(int LineNo) 
        {
            XmlNode AdministratorNode = doc.SelectSingleNode("//Line[@lineNo='" + LineNo + "']/Administrator");

            return AdministratorNode.InnerText;
        }
    }
}
