using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
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

                SearchFilter sfs = new SearchFilter.IsGreaterThan(ItemSchema.DateTimeReceived, DateTime.Now.AddDays(-3));

                int offset = 0;
                int pageSize = 50;
                bool more = true;
                ItemView view = new ItemView(pageSize, offset, OffsetBasePoint.Beginning);

                FindItemsResults<Item> findResults;
                List<EmailMessage> emails = new List<EmailMessage>();

                while (more)
                {
                    findResults = service.FindItems(WellKnownFolderName.Inbox,sfs, view);
                    foreach (var item in findResults.Items)
                    {
                        emails.Add((EmailMessage)item);
                    }
                    
                    more = findResults.MoreAvailable;
                    if (more)
                    {
                        view.Offset += pageSize;
                    }
                }
                PropertySet properties = (BasePropertySet.FirstClassProperties); //A PropertySet with the explicit properties you want goes here
                service.LoadPropertiesForItems(emails, properties);

                foreach (EmailMessage em in emails)
                {
                    
                    em.Load(new PropertySet(ItemSchema.MimeContent));
                    MimeContent mc = em.MimeContent;
                    string nazwa = Guid.NewGuid().ToString();                  

                    File.WriteAllBytes(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)+"\\Mailbox\\"+nazwa+".eml", mc.Content);
                }


                Trace.WriteLine("TEST has ended");
            }
            catch(Exception ex)
            {
                Trace.WriteLine("Error: " + ex.Message);
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
