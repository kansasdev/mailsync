using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace EAS.Protocol
{
    public class ASCommandResponse
    {
        private byte[] wbxmlBytes = null;
        private string xmlString = null;
        private HttpStatusCode httpStatus = HttpStatusCode.OK;

        public byte[] WbxmlBytes
        {
            get
            {
                return wbxmlBytes;
            }
        }

        public string XmlString
        {
            get
            {
                return xmlString;
            }
        }

        public HttpStatusCode HttpStatus
        {
            get
            {
                return httpStatus;
            }
        }

        public ASCommandResponse(HttpWebResponse httpResponse)
        {
            httpStatus = httpResponse.StatusCode;

            Stream responseStream = httpResponse.GetResponseStream();
            List<byte> bytes = new List<byte>();
            byte[] byteBuffer = new byte[256];
            int count = 0;

            count = responseStream.Read(byteBuffer, 0, 256);
            while (count > 0)
            {
                bytes.AddRange(byteBuffer);

                if (count < 256)
                {
                    int excess = 256 - count;
                    bytes.RemoveRange(bytes.Count - excess, excess);
                }

                count = responseStream.Read(byteBuffer, 0, 256);
            }

            wbxmlBytes = bytes.ToArray();

            xmlString = DecodeWBXML(wbxmlBytes);
        }

        private string DecodeWBXML(byte[] wbxml)
        {
            try
            {
                ASWBXML decoder = new ASWBXML();
                decoder.LoadBytes(wbxml);
                return decoder.GetXml();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

