using System;
using System.Diagnostics;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestEAS
{
    [TestClass]
    public class ExchangeTest
    {
        [TestMethod]
        public void GetEmails()
        {
            try
            {
                string pass = string.Empty;
                string user = string.Empty;

                MailSync.Credentials cred = new MailSync.Credentials();
                cred.GetCredentials("Input authorization data", "username user@domain.com", ref user, ref pass);
                                                
                string url = Interaction.InputBox("Address with https schema", "Exchange address", "https://");

                
                string mail = Interaction.InputBox("Input mailbox (user@domain.com)", "Enter mailbox", user);

                ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2013_SP1);
                service.Credentials = new WebCredentials(user,pass);
                service.TraceEnabled = true;
                service.TraceFlags = TraceFlags.All;
                service.Url = new Uri(url);
                service.AutodiscoverUrl(mail, RedirectionUrlValidationCallback);

                FindItemsResults<Item> findResults = service.FindItems(
                    WellKnownFolderName.Inbox,
                        new ItemView(10));

                foreach (Item item in findResults.Items)
                {
                    Trace.WriteLine(item.Subject);
                    
                    
                }



                Console.ReadLine();
            }
            catch(Exception ex)
            {
                Trace.WriteLine("BŁĄD: " + ex.Message);
            }
        }

        private static bool RedirectionUrlValidationCallback(string redirectionUrl)
        {
            // The default for the validation callback is to reject the URL.
            bool result = false;
            Uri redirectionUri = new Uri(redirectionUrl);
            // Validate the contents of the redirection URL. In this simple validation
            // callback, the redirection URL is considered valid if it is using HTTPS
            // to encrypt the authentication credentials. 
            if (redirectionUri.Scheme == "https")
            {
                result = true;
            }
            return result;
        }
    }
}
