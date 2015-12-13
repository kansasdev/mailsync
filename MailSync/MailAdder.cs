using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace MailSync
{
    public class MailAdder
    {
        private List<string> lstMAPI;
        
        public List<FileDateMI> LstFDMi;
        
        private string folderOutlook;
        private Outlook.Application app;
        private int howManyFilesInsideDir;

        private string pathToDirWithAttachments;

        private Outlook.Folder choosenFolder;

        private ResourceManager _rm;

        public event Action<string> TotalNumberOfFilesEvent;
        public event Action<string> ConvertedFilesNumberEvent;
        public event Action<string> NewFilesNumberEvent;

        public MailAdder(List<string> lstPathMAPI, string DirectoryOutlook,Outlook.Application App,string pathToAttachments,ResourceManager rm)
        {
            lstMAPI = lstPathMAPI;

            _rm = rm;

            LstFDMi = new List<FileDateMI>();

            folderOutlook = DirectoryOutlook;
            app = App;
            pathToDirWithAttachments = pathToAttachments;
            
            OnNewFilesNumberEvent(string.Format("{0} {1}", lstPathMAPI.Count.ToString(),_rm.GetString("strMailsToAddRes")));
        }

        /// <summary>
        /// exception wrapper in backgroundworker
        /// </summary>
        public void AddMailsToFolder()
        {
            FindSelectedFolder((Outlook.Folder)app.Session.DefaultStore.GetRootFolder(), folderOutlook);
            OnTotalNumberOfFilesEvent("");
            OnNewFilesNumberEvent("");
            OnConvertedFilesNumberEvent("");
            LiczbaMailiWFolderze();
            


            AddMails();
        }

        private void LiczbaMailiWFolderze()
        {
            howManyFilesInsideDir = 0;
           

          if(choosenFolder!=null)
          {
              howManyFilesInsideDir =choosenFolder.Items.Count;
          }
          else
          {
              howManyFilesInsideDir = 0;
          }

          OnConvertedFilesNumberEvent(string.Format("{0} {1}", howManyFilesInsideDir, _rm.GetString("strEmailsInsideChoosenDirRes")));

        }

        private void AddMails()
        {
            
            int addedNumber=0;
            int duplicateNumber = 0;
                    

            int sortedNumber = 0;

            
            foreach (string path in lstMAPI)
            {
                sortedNumber++;
                string fName = path.Substring(path.LastIndexOf("\\")+1,(path.Length-(path.LastIndexOf("\\")+1)));
                FileDateMI fdm = LstFDMi.Where(q => q.FileName == fName).FirstOrDefault();
                if (fdm == null)
                {
                    Outlook.MailItem mi = (Outlook.MailItem)app.Session.OpenSharedItem(path);
                    FileInfo fi = new FileInfo(path);
                    LstFDMi.Add(new FileDateMI() { MI = mi, Tick = mi.ReceivedTime.Ticks,ReceivedTime = mi.ReceivedTime, FileName = fName });
                   
                    OnTotalNumberOfFilesEvent(_rm.GetString("strSortElementNumberRes")+" "+ sortedNumber.ToString());
                }
                
            }

            LstFDMi = LstFDMi.OrderByDescending(q => q.Tick).ToList();
            
            //find oldest newest
            FileDateMI FDMiMin = LstFDMi.LastOrDefault();
            FileDateMI FDMiMax = LstFDMi.FirstOrDefault();

            DateTime dtMin = FDMiMin.ReceivedTime;
            DateTime dtMax = FDMiMax.ReceivedTime;

            List<Outlook.MailItem> lstMiToSearch = SetSelection(dtMin, dtMax);

            foreach(FileDateMI fdmi in LstFDMi)
            {
                addedNumber++;

                Outlook.MailItem mi = fdmi.MI;

                if (!IsDuplicate(lstMiToSearch,mi))
                {
                    if(mi.Attachments!=null&&mi.Attachments.Count>0)
                    {
                        /* //NOT NEEDED
                        for (int i = 1; i<= mi.Attachments.Count; i++)
                        {
                           string file = FindProperAttachment(fdmi.FileName, mi.Attachments[i].FileName,pathToDirWithAttachments);
                            if(!string.IsNullOrEmpty(file))
                            {
                                mi.Attachments.Remove(i);
                                mi.Attachments.Add(file);
                            }

                        }*/
                    }
                    mi.Move(choosenFolder);
                    OnNewFilesNumberEvent(string.Format("{0} {1}", addedNumber.ToString(), _rm.GetString("strNewEmailsInsideOutlookDirRes")));
                    OnConvertedFilesNumberEvent(string.Format("{0} {1}", howManyFilesInsideDir + addedNumber,_rm.GetString("strEmailsInsideChoosenOutlookDirRes")));
                }
                else
                {
                    duplicateNumber++;
                    OnTotalNumberOfFilesEvent(string.Format("{0} {1}", duplicateNumber,_rm.GetString("strDuplicatesInsideDirectoryRes")));
                }
            }

           
        }

        private bool IsDuplicate(List<Outlook.MailItem> lstMi, Outlook.MailItem mi)
        {
            return lstMi.Any(q => q.Body == mi.Body);
        }

        private List<Outlook.MailItem> SetSelection(DateTime dtStart, DateTime dtKoniec)
        {
            List<Outlook.MailItem> lstMI = new List<Outlook.MailItem>();
                       
            foreach (Outlook.MailItem oItem in choosenFolder.Items)
            {
                
                if(oItem.ReceivedTime>=dtStart && oItem.ReceivedTime<=dtKoniec) 
                {
                    lstMI.Add(oItem);
                }
            }
            
            return lstMI;
        }

        private List<Outlook.MailItem> SetSelection()
        {
            List<Outlook.MailItem> lstMI = new List<Outlook.MailItem>();

            foreach (Outlook.MailItem oItem in choosenFolder.Items)
            {
                
                lstMI.Add(oItem);
                
            }

            return lstMI;
        }

        private void FindSelectedFolder(Outlook.Folder folder,string selectedFolderPath)
        {
            
            Outlook.Folders childFolders =
                folder.Folders;
            if (childFolders.Count > 0)
            {
                foreach (Outlook.Folder childFolder in childFolders)
                {
                    if (childFolder.FolderPath != selectedFolderPath)
                    {
                        FindSelectedFolder(childFolder, selectedFolderPath);
                    }
                    else
                    {
                        choosenFolder = childFolder;
                    }
                }
            }
        }
                
        private string FindProperAttachment(string mail,string attachmentName,string folder)
        {
            string file = string.Empty;
            
            if(Directory.Exists(folder))
            {
                string dirName = folder + "\\" + mail.Substring(0,mail.IndexOf("_"));
                if(Directory.Exists(dirName))
                {
                    string[] files = Directory.GetFiles(dirName);
                    foreach(string f in files)
                    {
                        if (f.Substring(f.LastIndexOf("\\") + 1, (f.Length - (f.LastIndexOf("\\") + 1))) == attachmentName)
                        {
                            file = f;
                            break;
                        }
                    }
                }
            }

            return file;
        }

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
