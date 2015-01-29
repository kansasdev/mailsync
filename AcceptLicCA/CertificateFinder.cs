using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AcceptLicCA
{
    public class CertificateFinder
    {
        public bool FindProperCA(string serial,ref string error)
        {
            error = string.Empty;
            try
            {
                var rgx = new Regex("[^a-fA-F0-9]");
                string serial_regex = rgx.Replace(serial, string.Empty).ToUpper();

                X509Store store = new X509Store(StoreName.Root,StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection collection = store.Certificates.Find(X509FindType.FindBySerialNumber, serial_regex, false);
                if (collection.Count >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                error = "Error: " + ex.Message;
                return false;
            }

        }

        public bool AddCACert(X509Certificate2 cert,ref string error)
        {
            try
            {
                X509Store store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadWrite);
                store.Add(cert);

                store.Close();
                return true;
            }
            catch (Exception ex)
            {
                error = "Error: " + ex.Message;
                return false;                 
            }
        }
    }
}
