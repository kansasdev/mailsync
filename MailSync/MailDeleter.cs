using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace MailSync
{
    public class MailDeleter
    {
        private string path;
        private ResourceManager _rm;

        public event Action<string> TotalNumberOfFilesEvent;
        public event Action<string> ConvertedFilesNumberEvent;
        public event Action<string> NewFilesNumberEvent;

        public MailDeleter(string pathAfter,ResourceManager rm)
        {
            path = pathAfter;
            _rm = rm;
        }

        public bool CleanFolderMapiMimeFiles(List<FileDateMI> lst)
        {
                foreach (FileDateMI fdm in lst)
                {
                    if (!fdm.IsClosed)
                    {
                        fdm.MI.Close(Microsoft.Office.Interop.Outlook.OlInspectorClose.olDiscard);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(fdm.MI);
                        fdm.IsClosed = true;
                       
                    }
                }
                lst.Clear();
                return CleanFolderMapiMimeFiles();
        }

        public bool CleanFolderMapiMimeFiles()
        {
           if(!string.IsNullOrEmpty(path)&&Directory.Exists(path))
           {
               string[] files = Directory.GetFiles(path, "*.*");
               OnTotalNumberOfFilesEvent(files.Count()+ " " + _rm.GetString("strFilesToDeleteRes"));
               OnNewFilesNumberEvent("");
               int number = 0;
               foreach(string f in files)
               {
                   
                   number++;
                   File.Delete(f);
                   OnConvertedFilesNumberEvent(_rm.GetString("strDeletingFileRes")+ " " + number.ToString());
               }
               return true;
           }
           else
           {
               return false;
           }
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
