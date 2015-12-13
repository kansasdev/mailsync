using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EAS.Protocol
{
    public class ASFolderCommandRequest : ASCommandRequest
    {
        private string syncKey = "0";

        #region Property Accessors

        public string SyncKey
        {
            get
            {
                return syncKey;
            }
            set
            {
                syncKey = value;
            }
        }

        #endregion

        public ASFolderCommandRequest()
        {
            Command = "FolderSync";
        }

        // This function generates an ASFolderSyncResponse from an
        // HTTP response.
        protected override ASCommandResponse WrapHttpResponse(HttpWebResponse httpResp)
        {
            return new ASFolderCommandResponse(httpResp);
        }

        // This function generates the XML request body
        // for the FolderSync request.
        protected override void GenerateXMLPayload()
        {
            // If WBXML was explicitly set, use that
            if (WbxmlBytes != null)
                return;

            // Otherwise, use the properties to build the XML and then WBXML encode it
            XmlDocument folderSyncXML = new XmlDocument();

            XmlDeclaration xmlDeclaration = folderSyncXML.CreateXmlDeclaration("1.0", "utf-8", null);
            folderSyncXML.InsertBefore(xmlDeclaration, null);

            XmlNode folderSyncNode = folderSyncXML.CreateElement(Xmlns.folderHierarchyXmlns, "FolderSync", Namespaces.folderHierarchyNamespace);
            folderSyncNode.Prefix = Xmlns.folderHierarchyXmlns;
            folderSyncXML.AppendChild(folderSyncNode);

            if (syncKey == "")
                syncKey = "0";

            XmlNode syncKeyNode = folderSyncXML.CreateElement(Xmlns.folderHierarchyXmlns, "SyncKey", Namespaces.folderHierarchyNamespace);
            syncKeyNode.Prefix = Xmlns.folderHierarchyXmlns;
            syncKeyNode.InnerText = syncKey;
            folderSyncNode.AppendChild(syncKeyNode);

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlw = new XmlTextWriter(sw);
            xmlw.Formatting = Formatting.Indented;
            folderSyncXML.WriteTo(xmlw);
            xmlw.Flush();

            XmlString = sw.ToString();
        }
    }
}
