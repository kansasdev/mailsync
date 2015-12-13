using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace EAS.Protocol
{
    public class ASOptionsResponse
    {
        private string commands = null;
        private string versions = null;

        public ASOptionsResponse(HttpWebResponse httpResponse)
        {
            commands = httpResponse.GetResponseHeader("MS-ASProtocolCommands");
            versions = httpResponse.GetResponseHeader("MS-ASProtocolVersions");
        }

        public string SupportedCommands
        {
            get
            {
                return commands;
            }
        }

        public string SupportedVersions
        {
            get
            {
                return versions;
            }
        }

        public string HighestSupportedVersion
        {
            get
            {
                char[] chDelimiters = { ',' };
                string[] strVersions = SupportedVersions.Split(chDelimiters);

                string strHighestVersion = "0.0";
                /*
                foreach (string strVersion in strVersions)
                {
                    if (Convert.ToSingle(strVersion) > Convert.ToSingle(strHighestVersion))
                    {
                        strHighestVersion = strVersion;
                    }
                }*/

                //return strHighestVersion;
                return "14.0";
            }
        }
    }
}

