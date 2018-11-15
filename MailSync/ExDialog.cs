using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Resources;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MailSync
{
    public class ExDialog
    {
        // Create credentials for the user
        WebCredentials cred;
        
        string mail;
        string server;
        string username;
        public string mailDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Mailbox";

        private ExchangeService service;

        private ResourceManager _rm;

        public event Action<string> TotalNumberOfFilesEvent;
        public event Action<string> ConvertedFilesNumberEvent;
        public event Action<string> NewFilesNumberEvent;



        public ExDialog(ResourceManager rm, string user, string pass, string serv, string email, string dID, string dType)
        {
            //user = user.Replace("\\", "");
            cred = new WebCredentials(user, pass);
            mail = email;
            server = serv;
            username = user;
            _rm = rm;
            
        }

        public bool Initialize(ref string kom)
        {
            try
            {
                OnNewFilesNumberEvent("");
                OnTotalNumberOfFilesEvent("");
                OnConvertedFilesNumberEvent("");
                if (!Directory.Exists(mailDir))
                {
                    Directory.CreateDirectory(mailDir);
                }

                service = new ExchangeService(ExchangeVersion.Exchange2013_SP1);
                service.Credentials = cred;
                service.TraceEnabled = true;
                service.TraceFlags = TraceFlags.All;
                service.Url = new Uri(server);
                service.AutodiscoverUrl(mail);

                return true;
            }
            catch (Exception ex)
            {
                kom = _rm.GetString("lblTotalGenericErrorRes") + ex.Message;
                return false;
            }
        }

       

        public bool SetConversation(ref string kom)
        {
            try
            {
                string folderToSync = string.Empty;
                string days = string.Empty;
                FolderView view = new FolderView(100);
                view.PropertySet = new PropertySet(BasePropertySet.IdOnly);
                view.PropertySet.Add(FolderSchema.DisplayName);
                view.Traversal = FolderTraversal.Deep;
                if (service != null)
                {
                    List<string> lstDirsToChoose = new List<string>();
                    List<string> lstDirsToDisplay = new List<string>();
                    //find specific folder
                    
                        lstDirsToChoose.Add(WellKnownFolderName.Inbox.ToString());
                    lstDirsToChoose.Add(WellKnownFolderName.SentItems.ToString());
                    lstDirsToDisplay.Add(WellKnownFolderName.Inbox.ToString());
                    lstDirsToDisplay.Add(WellKnownFolderName.SentItems.ToString());
                    
                    FolderDecision fd = new FolderDecision(lstDirsToDisplay, true);
                    fd.SelectedFolder = lstDirsToChoose.FirstOrDefault();
                    fd.ShowDialog();

                    if (!Directory.Exists(mailDir + "\\" + fd.SelectedFolder.Replace(" ", "")))
                    {
                        Directory.CreateDirectory(mailDir + "\\" + fd.SelectedFolder.Replace(" ", ""));

                    }
                    mailDir = mailDir + "\\" + fd.SelectedFolder.Replace(" ", "");
                    folderToSync = lstDirsToChoose.Where(q => q == fd.SelectedFolder).FirstOrDefault();
                    days = fd.SelectedTime;

                    SearchFilter sfs = new SearchFilter.IsGreaterThan(ItemSchema.DateTimeReceived, DateTime.Now.AddDays(-int.Parse(days)));

                    int offset = 0;
                    int pageSize = 50;
                    bool more = true;
                    ItemView viewItem = new ItemView(pageSize, offset, OffsetBasePoint.Beginning);

                    FindItemsResults<Item> findResults;
                    List<EmailMessage> emails = new List<EmailMessage>();

                    while (more)
                    {
                        WellKnownFolderName folderName = WellKnownFolderName.Inbox;
                        if(folderToSync == WellKnownFolderName.Inbox.ToString())
                        {
                            folderName = WellKnownFolderName.Inbox;
                        }
                        if(folderToSync == WellKnownFolderName.SentItems.ToString())
                        {
                            folderName = WellKnownFolderName.SentItems;
                        }

                        findResults = service.FindItems(folderName, sfs, viewItem);
                        foreach (var item in findResults.Items)
                        {
                            emails.Add((EmailMessage)item);
                        }

                        more = findResults.MoreAvailable;
                        if (more)
                        {
                            viewItem.Offset += pageSize;
                        }
                    }
                    PropertySet properties = (BasePropertySet.FirstClassProperties); //A PropertySet with the explicit properties you want goes here
                    service.LoadPropertiesForItems(emails, properties);
                    int index = 1;
                    foreach (EmailMessage em in emails)
                    {
                        em.Load(new PropertySet(ItemSchema.MimeContent));
                        MimeContent mc = em.MimeContent;
                        string nazwa = BitConverter.ToString(MD5.Create().ComputeHash(ASCIIEncoding.ASCII.GetBytes(em.Id.UniqueId)));
                        if (!File.Exists(mailDir+"\\"+nazwa+".eml"))
                        {
                            File.WriteAllBytes(mailDir + "\\" + nazwa + ".eml", em.MimeContent.Content);
                            OnNewFilesNumberEvent(string.Format("{0} {1}", index, _rm.GetString("strNewEmailsInsideOutlookDirRes")));
                            index++;
                        }
                    }
                }
                               
                return true;
            }
            catch (Exception ex)
            {
                kom = _rm.GetString("lblTotalGenericErrorRes") + " " + ex.Message;
                return false;
            }

        }
        /*
        private void WriteEmailsToDirectory(List<string> lstEmailsSid, string folderToSync, uint policyKey)
        {
            ASCommandResponse commandResponse = null;
            int fileNo = 1;
            foreach (string sid in lstEmailsSid)
            {
                string nazwaPlikuZKatalogiem = mailDir + "\\" + sid.Replace(":", "");
                if (!File.Exists(nazwaPlikuZKatalogiem + ".eml"))
                {

                    string fetchXml = SetFetchDataObjectAsXml(folderToSync, sid, true);
                    StringBuilder xmlBuilder = new StringBuilder();
                    xmlBuilder.Append(fetchXml);

                    ASCommandRequest commandRequest = CreateCommandRequest("ItemOperations", cred, devID, devType, protVer, server, username, xmlBuilder.ToString(), policyKey);

                    // Send the request
                    commandResponse = commandRequest.GetResponse();

                    EAS.generated.ItemOperationsResponseNamespace.ItemOperations emailFetched = GetFetchedMail(commandResponse.XmlString);
                    if (emailFetched != null && emailFetched.Response != null && emailFetched.Response.Fetch != null && emailFetched.Response.Fetch.Count() > 0)
                    {
                        //WYCHWYC BODY
                        EAS.generated.ItemOperationsResponseNamespace.Body b = null;
                        List<EAS.generated.ItemOperationsResponseNamespace.ItemsChoiceType3> lst = emailFetched.Response.Fetch[0].Properties.ItemsElementName.ToList();
                        int index = lst.FindIndex(q => q == EAS.generated.ItemOperationsResponseNamespace.ItemsChoiceType3.Body);
                        if (index > 0)
                        {
                            if (emailFetched.Response.Fetch[0].Properties.ItemsElementName[index] == EAS.generated.ItemOperationsResponseNamespace.ItemsChoiceType3.Body)
                            {
                                b = (EAS.generated.ItemOperationsResponseNamespace.Body)emailFetched.Response.Fetch[0].Properties.Items[index];

                            }
                        }

                        if (b != null)
                        {
                            string s = b.Data;

                            MemoryStream ms = new MemoryStream();
                            StreamWriter sw = new StreamWriter(ms);
                            sw.Write(s);
                            sw.Flush();
                            ms.Seek(0, SeekOrigin.Begin);

                            MimeKit.MimeMessage mm = MimeKit.MimeMessage.Load(ms);
                            sw.Close();
                            ms.Close();


                            if (!string.IsNullOrEmpty(mm.HtmlBody))
                            {
                                string body = mm.HtmlBody;
                                string sContent = BitConverter.ToString(Encoding.UTF8.GetBytes(body));


                                int idx = 0;
                                do
                                {
                                    idx = sContent.IndexOf("C3", idx);
                                    if (idx != -1)
                                    {
                                        string podciag = sContent.Substring(idx, sContent.Length - idx);

                                        if (podciag.StartsWith("C3-83") || podciag.StartsWith("C3-84") || podciag.StartsWith("C3-85") || podciag.StartsWith("C3-82")
                                            || podciag.StartsWith("C3-A2")) //WYCINA CUDZYSŁÓW i apostrof w pewnych dziwnych przypadkach..
                                        {
                                            if (podciag.Length > 10)
                                            {
                                                //string subPodciag = podciag.Substring(9, 2);
                                                //if ( subPodciag != "AD" || subPodciag != "AA" || subPodciag != "AC" || subPodciag != "AE" || subPodciag != "AF"
                                                //|| subPodciag != "A1" || subPodciag != "A2" || subPodciag != "A0" || subPodciag != "A3" || subPodciag != "A4" || subPodciag != "A5" || subPodciag != "A6" || subPodciag != "A7" || subPodciag != "A8" || subPodciag != "A9"
                                                //|| subPodciag != "B0" || subPodciag != "B1" || subPodciag != "B2" || subPodciag != "B3" || subPodciag != "B4" || subPodciag != "B5" || subPodciag != "B6" || subPodciag != "B7" || subPodciag != "B8" || subPodciag != "B9" || subPodciag != "B0"
                                                //|| subPodciag != "BA" || subPodciag != "BB" || subPodciag != "BC" || subPodciag != "BD" || subPodciag != "BE" || subPodciag != "BF")
                                                {
                                                    sContent = sContent.Remove(idx + 1, 3);
                                                    sContent = sContent.Remove(idx + 3, 3);
                                                }
                                            }
                                        }
                                        idx++;
                                    }
                                } while (idx != -1);


                                string[] sArr = sContent.Split('-');

                                byte[] bNew = new byte[sArr.Length];
                                for (int i = 0; i < sArr.Length; i++)
                                {
                                    byte byt = Convert.ToByte(sContent.Substring(3 * i, 2), 16);

                                    bNew[i] = byt;

                                }

                                string enc = Encoding.UTF8.GetString(bNew);

                                s = s.Replace(body, enc);

                                //recreate object
                                ms = new MemoryStream();
                                sw = new StreamWriter(ms);
                                sw.Write(s);
                                sw.Flush();
                                ms.Seek(0, SeekOrigin.Begin);
                                mm = MimeKit.MimeMessage.Load(ms);
                                sw.Close();
                                ms.Close();
                            }

                            mm.WriteTo(nazwaPlikuZKatalogiem + ".eml");
                            OnNewFilesNumberEvent(string.Format("{0} {1}", fileNo, _rm.GetString("strEmailsDownloaded")));
                            fileNo++;
                        }

                    }
                    else
                    {
                        MessageBox.Show(_rm.GetString("errSerialization"));
                    }
                }

            }
        }

        private ASCommandRequest CreateCommandRequest(string commandName, NetworkCredential cred, string devID, string devType,
            string protVer, string server, string username, string wbXmlPayload, uint policyKey = 0)
        {
            ASCommandRequest commandRequest = new ASCommandRequest();
            commandRequest.Command = commandName;
            commandRequest.Credentials = cred;
            commandRequest.DeviceID = devID;
            commandRequest.DeviceType = devType;
            commandRequest.ProtocolVersion = protVer;
            commandRequest.Server = server;
            commandRequest.UseEncodedRequestLine = true;
            commandRequest.User = username;
            commandRequest.UseSSL = true;
            commandRequest.PolicyKey = policyKey;
            //XmlDocument doc = new XmlDocument();
            //doc.LoadXml(wbXmlPayload);
            //doc.PreserveWhitespace = false;
            commandRequest.XmlString = wbXmlPayload;

            return commandRequest;

        }

        private string SetProvisionObjectAsXml()
        {
            EAS.generated.ProvisionRequestNamespace.Provision prov = new EAS.generated.ProvisionRequestNamespace.Provision();
            prov.DeviceInformation = new EAS.generated.ProvisionRequestNamespace.DeviceInformation();
            prov.DeviceInformation.Set = new EAS.generated.ProvisionRequestNamespace.DeviceInformationSet();
            prov.DeviceInformation.Set.Model = Environment.MachineName;
            prov.DeviceInformation.Set.OS = Environment.OSVersion.Platform.ToString();
            prov.DeviceInformation.Set.FriendlyName = "MailSync";
            prov.DeviceInformation.Set.UserAgent = "MailApp";

            prov.Policies = new EAS.generated.ProvisionRequestNamespace.ProvisionPolicies();
            prov.Policies.Policy = new EAS.generated.ProvisionRequestNamespace.ProvisionPoliciesPolicy();
            prov.Policies.Policy.PolicyType = "MS-EAS-Provisioning-WBXML";

            XmlSerializer ser = new XmlSerializer(prov.GetType());

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("settings", "Settings");

            MemoryStream ms = new MemoryStream();
            ser.Serialize(ms, prov, ns);
            ms.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(ms);
            string s = sr.ReadToEnd();

            return s;
        }

        private string SetSettingsObjectAsXml()
        {
            EAS.generated.ProvisionRequestNamespace.Settings set = new EAS.generated.ProvisionRequestNamespace.Settings();
            set.DeviceInformation = new EAS.generated.ProvisionRequestNamespace.DeviceInformation();
            set.DeviceInformation.Set = new EAS.generated.ProvisionRequestNamespace.DeviceInformationSet();
            set.DeviceInformation.Set.Model = "masz";//Environment.MachineName;
            set.DeviceInformation.Set.IMEI = "12123434";
            set.DeviceInformation.Set.OS = Environment.OSVersion.Platform.ToString();
            set.DeviceInformation.Set.FriendlyName = "MailSync";
            set.DeviceInformation.Set.UserAgent = "MailApp";

            set.UserInformation = new EAS.generated.ProvisionRequestNamespace.SettingsUserInformation();
            set.UserInformation.Item = "";

            XmlSerializer ser = new XmlSerializer(set.GetType());

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("settings", "Settings");

            MemoryStream ms = new MemoryStream();
            ser.Serialize(ms, set, ns);
            ms.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(ms);
            string s = sr.ReadToEnd();

            return s;
        }

        private string SetFolderSyncObjectAsXml(string syncKey)
        {
            EAS.generated.FolderReqestNamespace.FolderSync fs = new EAS.generated.FolderReqestNamespace.FolderSync();

            fs.SyncKey = syncKey;

            XmlSerializer ser = new XmlSerializer(typeof(EAS.generated.FolderReqestNamespace.FolderSync));
            MemoryStream ms = new MemoryStream();
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("folderhierarchy", "FolderHierarchy");

            ser.Serialize(ms, fs, ns);
            ms.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(ms);
            string s = sr.ReadToEnd();

            return s;
        }

        private string SetSyncObjectAsXml(string syncKey, string folderToSync, string syncFromDays)
        {
            string daysOption = "5";
            switch (syncFromDays)
            {
                case "1":
                    daysOption = "1";
                    break;
                case "3":
                    daysOption = "2";
                    break;
                case "7":
                    daysOption = "3";
                    break;
                case "14":
                    daysOption = "4";
                    break;
                case "30":
                    daysOption = "5";
                    break;
                case "90":
                    daysOption = "6";
                    break;
                case "180":
                    daysOption = "7";
                    break;
                case "ALL":
                    daysOption = "0";
                    break;

                default:
                    daysOption = "5";
                    break;
            }

            //EAS.generated
            EAS.generated.SyncRequestNamespace.Sync sync = new EAS.generated.SyncRequestNamespace.Sync();

            EAS.generated.SyncRequestNamespace.SyncCollection sa = new EAS.generated.SyncRequestNamespace.SyncCollection();
            sa.CollectionId = folderToSync;
            sa.SyncKey = syncKey;
            sa.WindowSize = "25";
            if (syncKey != "0")
            {

                EAS.generated.SyncRequestNamespace.Options opt = new EAS.generated.SyncRequestNamespace.Options();

                opt.ItemsElementName = new EAS.generated.SyncRequestNamespace.ItemsChoiceType1[] { EAS.generated.SyncRequestNamespace.ItemsChoiceType1.BodyPreference };

                //EAS.generated.SyncRequestNamespace.BodyPreference bp1 = new EAS.generated.SyncRequestNamespace.BodyPreference();
                //bp1.Type = 1;
                //bp1.Preview = 128;

                opt.ItemsElementName = new EAS.generated.SyncRequestNamespace.ItemsChoiceType1[] { EAS.generated.SyncRequestNamespace.ItemsChoiceType1.FilterType };

                // 0 wszystko, 1 1 dzien, 2 3 dni, 3 7 dni,4 14, 5 1 miesiac, 6 3 mies, 7 6 mies
                //opt.Items = new object[] { "5" };

                sa.Options = new EAS.generated.SyncRequestNamespace.Options[] { opt };
            }

            sync.Collections = new EAS.generated.SyncRequestNamespace.SyncCollection[] { sa };

            XmlSerializer ser = new XmlSerializer(typeof(EAS.generated.SyncRequestNamespace.Sync));
            //WYWAL KLASĘ SUPPORTED
            //WYSZUKAJ "AllDayEvent", typeof i zamien byte na string

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("airsync", "AirSync");
            if (syncKey != "0")
            {
                ns.Add("airsyncbase", "AirSyncBase");
            }

            MemoryStream ms = new MemoryStream();
            //ser.Serialize(ms, prov, ns);
            ser.Serialize(ms, sync, ns);
            ms.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(ms);
            string s = sr.ReadToEnd();
            //error with serialization options
            //if (syncKey != "0")
            {
                s = s.Replace("<airsync:Options />", "<airsync:Options><airsync:FilterType>" + daysOption + "</airsync:FilterType></airsync:Options>");
            }
            return s;
        }

        private string SetProvisionObjectACKAsXml(string policyKey, bool wipeRequested)
        {
            EAS.generated.ProvisionRequestNamespace.Provision prov = new EAS.generated.ProvisionRequestNamespace.Provision();
            if (!wipeRequested)
            {
                prov.Policies = new EAS.generated.ProvisionRequestNamespace.ProvisionPolicies();
                prov.Policies.Policy = new EAS.generated.ProvisionRequestNamespace.ProvisionPoliciesPolicy();
                prov.Policies.Policy.PolicyType = "MS-EAS-Provisioning-WBXML";

                prov.Policies.Policy.Status = "1";
                prov.Policies.Policy.PolicyKey = policyKey;
            }
            else
            {
                prov.RemoteWipe = new EAS.generated.ProvisionRequestNamespace.ProvisionRemoteWipe();
                prov.RemoteWipe.Status = "1";
            }

            XmlSerializer ser = new XmlSerializer(prov.GetType());

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("settings", "Settings");

            MemoryStream ms = new MemoryStream();
            ser.Serialize(ms, prov, ns);
            ms.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(ms);
            string s = sr.ReadToEnd();

            return s;
        }
        private EAS.generated.ProvisionResponseNamespace.Provision GetProvisionObjectFromXML(string x)
        {
            MemoryStream streamOut = new MemoryStream();
            StreamWriter writer = new StreamWriter(streamOut);
            writer.Write(x);
            writer.Flush();
            streamOut.Position = 0;

            XmlSerializer deser = new XmlSerializer(typeof(EAS.generated.ProvisionResponseNamespace.Provision));
            EAS.generated.ProvisionResponseNamespace.Provision provResponse = (EAS.generated.ProvisionResponseNamespace.Provision)deser.Deserialize(streamOut);

            return provResponse;
        }

        private EAS.generated.FolderResponseNamespace.FolderSync GetFolderSyncObjectFromXML(string x)
        {
            MemoryStream streamOut = new MemoryStream();
            StreamWriter writer = new StreamWriter(streamOut);
            writer.Write(x);
            writer.Flush();
            streamOut.Position = 0;

            XmlSerializer deser = new XmlSerializer(typeof(EAS.generated.FolderResponseNamespace.FolderSync));
            EAS.generated.FolderResponseNamespace.FolderSync fsResponse = (EAS.generated.FolderResponseNamespace.FolderSync)deser.Deserialize(streamOut);

            return fsResponse;
        }

        private EAS.generated.SyncResponseNamespace.Sync GetSyncObjectFromXML(string x)
        {

            try
            {
                XmlReaderSettings xmlReaderSettings = new XmlReaderSettings() { CheckCharacters = false };
                XmlReader xmlReader = XmlTextReader.Create(new StringReader(x), xmlReaderSettings);

                XmlSerializer deser = new XmlSerializer(typeof(EAS.generated.SyncResponseNamespace.Sync));
                EAS.generated.SyncResponseNamespace.Sync sync = (EAS.generated.SyncResponseNamespace.Sync)deser.Deserialize(xmlReader);
                //SyncCollectionsCollectionResponsesFetchApplicationData 
                //[System.Xml.Serialization.XmlElementAttribute("Attachments", typeof(Attachments[]), Namespace = "AirSyncBase")] //ZMIANA Z Attachments na Attachments[]
                return sync;
            }
            catch (Exception ex)
            {
                List<string> lstError = new List<string>();
                lstError.Add(ex.Message);
                lstError.Add(x);
                File.WriteAllLines(mailDir + "\\" + DateTime.Now.Ticks + ".txt", lstError);
                return null;
            }
        }

        private string SetFetchDataObjectAsXml(string collectionid, string serverid, bool useMIME)
        {
            EAS.generated.ItemOperationsRequestNamespace.ItemOperations io = new EAS.generated.ItemOperationsRequestNamespace.ItemOperations();

            EAS.generated.ItemOperationsRequestNamespace.ItemOperationsFetch iof = new EAS.generated.ItemOperationsRequestNamespace.ItemOperationsFetch();
            iof.CollectionId = collectionid;
            iof.ServerId = serverid;
            iof.Store = "Mailbox";

            iof.Options = new EAS.generated.ItemOperationsRequestNamespace.ItemOperationsFetchOptions();

            iof.Options.ItemsElementName = new EAS.generated.ItemOperationsRequestNamespace.ItemsChoiceType6[] { EAS.generated.ItemOperationsRequestNamespace.ItemsChoiceType6.BodyPreference };

            EAS.generated.ItemOperationsRequestNamespace.BodyPreference bp = new EAS.generated.ItemOperationsRequestNamespace.BodyPreference();

            bp.Type = 4;
            bp.AllOrNoneSpecified = true;
            bp.AllOrNone = true;
            bp.TruncationSizeSpecified = true;
            bp.TruncationSize = 1000000;
            bp.PreviewSpecified = true;
            bp.Preview = 0;

            iof.Options.Items = new object[] { bp };

            io.Items = new object[] { iof };

            XmlSerializer ser = new XmlSerializer(io.GetType());


            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("airsyncbase", "AirSyncBase");
            ns.Add("airsync", "AirSync");

            MemoryStream ms = new MemoryStream();
            ser.Serialize(ms, io, ns);
            ms.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(ms);
            string s = sr.ReadToEnd();

            if (useMIME)
            {
                //problem with serialization..
                s = s.Replace("<Options>", "<Options><airsync:MIMESupport>1</airsync:MIMESupport>");
            }

            return s;
        }

        private EAS.generated.ItemOperationsResponseNamespace.ItemOperations GetFetchedMail(string x)
        {
            try
            {
                XmlReaderSettings xmlReaderSettings = new XmlReaderSettings() { CheckCharacters = false };
                XmlReader xmlReader = XmlTextReader.Create(new StringReader(x), xmlReaderSettings);

                XmlSerializer deser = new XmlSerializer(typeof(EAS.generated.ItemOperationsResponseNamespace.ItemOperations));
                EAS.generated.ItemOperationsResponseNamespace.ItemOperations fetch = (EAS.generated.ItemOperationsResponseNamespace.ItemOperations)deser.Deserialize(xmlReader);

                //1
                //SyncCollectionsCollectionResponsesFetchApplicationData 
                //[System.Xml.Serialization.XmlElementAttribute("Attachments", typeof(Attachments[]), Namespace = "AirSyncBase")] //ZMIANA Z Attachments na Attachments[]
                //2
                /// <uwagi/>
                //[System.Xml.Serialization.XmlElementAttribute("Categories", typeof(Categories))] //zamiana ze string[] na object[]
                //public object[] Categories
                //{
                //get
                //{
                //  return this.categoriesField;
                //}
                //set
                //{
                //  this.categoriesField = value;
                //}
                //3
                //Zakomentowano wszystkie wystapienia Attendess i AttendeesAttendess

                return fetch;
            }
            catch (Exception ex)
            {
                List<string> lstError = new List<string>();
                lstError.Add(ex.Message);
                lstError.Add(x);
                File.WriteAllLines(mailDir + "\\" + DateTime.Now.Ticks + ".txt", lstError);
                return null;
            }

        }*/


        private void OnTotalNumberOfFilesEvent(string s)
        {
            if (TotalNumberOfFilesEvent != null)
            {
                TotalNumberOfFilesEvent(s);
            }
        }

        private void OnConvertedFilesNumberEvent(string s)
        {
            if (ConvertedFilesNumberEvent != null)
            {
                ConvertedFilesNumberEvent(s);
            }
        }

        private void OnNewFilesNumberEvent(string s)
        {
            if (NewFilesNumberEvent != null)
            {
                NewFilesNumberEvent(s);
            }
        }
    }
}
