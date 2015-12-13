﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace EAS.Protocol
{
    public struct CommandParameter
    {
        public string Parameter;
        public string Value;
    }

    // Base class for ActiveSync command requests
    public class ASCommandRequest
    {
        private NetworkCredential credential = null;
        private string server = null;
        private bool useSSL = true;
        private byte[] wbxmlBytes = null;
        private string xmlString = null;
        private string protocolVersion = null;
        private string requestLine = null;
        private bool useEncodedRequestLine = true;
        private string command = null;
        private string user = null;
        private string deviceID = null;
        private string deviceType = null;
        private UInt32 policyKey = 0;
        private CommandParameter[] parameters = null;

        #region Property Accessors
        public NetworkCredential Credentials
        {
            get
            {
                return credential;
            }
            set
            {
                credential = value;
            }
        }

        public string Server
        {
            get
            {
                return server;
            }
            set
            {
                server = value;
            }
        }

        public bool UseSSL
        {
            get
            {
                return useSSL;
            }
            set
            {
                useSSL = value;
            }
        }

        public byte[] WbxmlBytes
        {
            get
            {
                return wbxmlBytes;
            }
            set
            {
                wbxmlBytes = value;
                // Loading WBXML bytes causes immediate decoding
                xmlString = DecodeWBXML(wbxmlBytes);
            }
        }

        public string XmlString
        {
            get
            {
                return xmlString;
            }
            set
            {
                xmlString = value;
                // Loading XML causes immediate encoding
                wbxmlBytes = EncodeXMLString(xmlString);
            }
        }

        public string ProtocolVersion
        {
            get
            {
                return protocolVersion;
            }
            set
            {
                protocolVersion = value;
            }
        }

        public string RequestLine
        {
            get
            {
                // Generate on demand
                BuildRequestLine();
                return requestLine;
            }
            set
            {
                requestLine = value;
            }
        }

        public bool UseEncodedRequestLine
        {
            get
            {
                return useEncodedRequestLine;
            }
            set
            {
                useEncodedRequestLine = value;
            }
        }

        public string Command
        {
            get
            {
                return command;
            }
            set
            {
                command = value;
            }
        }

        public string User
        {
            get
            {
                return user;
            }
            set
            {
                user = value;
            }
        }

        public string DeviceID
        {
            get
            {
                return deviceID;
            }
            set
            {
                deviceID = value;
            }
        }

        public string DeviceType
        {
            get
            {
                return deviceType;
            }
            set
            {
                deviceType = value;
            }
        }

        public UInt32 PolicyKey
        {
            get
            {
                return policyKey;
            }
            set
            {
                policyKey = value;
            }
        }

        public CommandParameter[] CommandParameters
        {
            get
            {
                return parameters;
            }
            set
            {
                parameters = value;
            }
        }
        #endregion

        public ASCommandResponse GetResponse()
        {
            GenerateXMLPayload();

            if (Credentials == null || Server == null || ProtocolVersion == null || WbxmlBytes == null)
                throw new InvalidDataException("ASCommandRequest not initialized.");

            string uriString = string.Format("{0}//{1}/Microsoft-Server-ActiveSync?{2}",
                useSSL ? "https:" : "http:", server, RequestLine);
            Uri serverUri = new Uri(uriString);
            CredentialCache creds = new CredentialCache();
            // Using Basic authentication
            creds.Add(serverUri, "Basic", credential);

            HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(uriString);
            httpReq.Credentials = creds;
            httpReq.Method = "POST";
            httpReq.ContentType = "application/vnd.ms-sync.wbxml";

            if (!UseEncodedRequestLine)
            {
                httpReq.Headers.Add("MS-ASProtocolVersion", ProtocolVersion);
                httpReq.Headers.Add("X-MS-PolicyKey", PolicyKey.ToString());
            }

            try
            {
                Stream requestStream = httpReq.GetRequestStream();
                requestStream.Write(WbxmlBytes, 0, WbxmlBytes.Length);
                requestStream.Close();

                HttpWebResponse httpResp = (HttpWebResponse)httpReq.GetResponse();

                ASCommandResponse response = WrapHttpResponse(httpResp);

                httpResp.Close();

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected virtual ASCommandResponse WrapHttpResponse(HttpWebResponse httpResp)
        {
            return new ASCommandResponse(httpResp);
        }

        protected virtual void BuildRequestLine()
        {
            if (Command == null || User == null || DeviceID == null || DeviceType == null)
                throw new InvalidDataException("ASCommandRequest not initialized.");

            if (UseEncodedRequestLine == true)
            {
                EncodedRequest encRequest = new EncodedRequest();

                encRequest.ProtocolVersion = Convert.ToByte(Convert.ToSingle(ProtocolVersion) * 10);
                //encRequest.ProtocolVersion = 0x14;//Convert.ToByte(14);
                encRequest.SetCommandCode(Command);
                encRequest.SetLocale("en-us");
                encRequest.DeviceId = DeviceID;
                encRequest.DeviceType = DeviceType;
                encRequest.PolicyKey = PolicyKey;

                encRequest.AddCommandParameter("User", user);

                if (CommandParameters != null)
                {
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        encRequest.AddCommandParameter(CommandParameters[i].Parameter, CommandParameters[i].Value);
                    }
                }

                RequestLine = encRequest.GetBase64EncodedString();
            }
            else
            {
                RequestLine = string.Format("Cmd={0}&User={1}&DeviceId={2}&DeviceType={3}",
                Command, User, DeviceID, DeviceType);

                if (CommandParameters != null)
                {
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        RequestLine = string.Format("{0}&{1}={2}", RequestLine,
                            CommandParameters[i].Parameter, CommandParameters[i].Value);
                    }
                }
            }
        }

        protected virtual void GenerateXMLPayload()
        {
            // For the base class, this is a no-op.
            // Classes that extend this class to implement
            // commands override this function to generate
            // the XML payload based on the command's request schema.
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

        private byte[] EncodeXMLString(string stringXML)
        {
            try
            {
                ASWBXML encoder = new ASWBXML();
                encoder.LoadXml(stringXML);
                return encoder.GetBytes();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

