﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
                        
            List<string> lstForAdd = new List<string>(lstMAPI);
            foreach(Outlook.MailItem mi in choosenFolder.Items)
            {
                const string internetMessageIdWTag = "http://schemas.microsoft.com/mapi/proptag/0x1035001F";
                string idMess = mi.PropertyAccessor.GetProperty(internetMessageIdWTag);

                string result = lstMAPI.Find(q => q.EndsWith(idMess.Replace(":", "") + ".msg"));
                if(!string.IsNullOrEmpty(result))
                {
                    lstForAdd.Remove(result);
                    duplicateNumber++;
                    OnTotalNumberOfFilesEvent(string.Format("{0} {1}", duplicateNumber, _rm.GetString("strDuplicatesInsideDirectoryRes")));
                }
                Marshal.ReleaseComObject(mi);
            }

            foreach(string path in lstForAdd)
            {
                Outlook.MailItem mi = null;

                mi = (Outlook.MailItem)app.Session.OpenSharedItem(path);
                mi.Move(choosenFolder);
                addedNumber++;
                OnNewFilesNumberEvent(string.Format("{0} {1}", addedNumber.ToString(), _rm.GetString("strNewEmailsInsideOutlookDirRes")));
                OnConvertedFilesNumberEvent(string.Format("{0} {1}", howManyFilesInsideDir + addedNumber, _rm.GetString("strEmailsInsideChoosenOutlookDirRes")));
                Marshal.ReleaseComObject(mi);
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
