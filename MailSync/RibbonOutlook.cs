using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using System.Windows.Forms;
using Outlook = Microsoft.Office.Interop.Outlook;
using Microsoft.Exchange.WebServices.Data;
using System.IO;
using System.ComponentModel;
using System.Resources;
using System.Threading;
using System.Reflection;

namespace MailSync
{
    public partial class RibbonOutlook
    {
        public Outlook.Application App { get; set; }
        private string pathAfter = string.Empty;
        private string pathWithAttachmentsAfter = string.Empty;
        private List<string> lstOutlookDirs;
        private string targetDirOutlook;
        private MrMapiConverter mapiMimeClass;
        private MailAdder ma;

        private void RibbonOutlook_Load(object sender, RibbonUIEventArgs e)
        {
            string wersja = Globals.ThisAddIn.Application.Version;
            btnImport.Enabled = false;
            
        }


        #region Mail Importing
        private void btnImport_Click(object sender, RibbonControlEventArgs e)
        {
                        
            if(mapiMimeClass!=null&&mapiMimeClass.LstMapi!=null)
            {
                if(mapiMimeClass.LstMapi.Count>=500)
                {
                    
                }
            }

            lstOutlookDirs = new List<string>();
            EnumerateFoldersInDefaultStore();
            if(lstOutlookDirs.Count>=1)
            {
                FolderDecision fd = new FolderDecision(lstOutlookDirs);
                DialogResult dr = fd.ShowDialog();
                if(!string.IsNullOrEmpty(fd.SelectedFolder))
                {
                    targetDirOutlook = fd.SelectedFolder;
                    if(mapiMimeClass!=null && mapiMimeClass.LstMapi!=null && mapiMimeClass.LstMapi.Count>=1)
                    {
                        btnImport.Enabled = false;
                        btnDirectory.Enabled = false;
                        btnHelp.Enabled = false;
                        btnClean.Enabled = false;

                        BackgroundWorker bwAdd = new BackgroundWorker();
                        bwAdd.DoWork+=bwAdd_DoWork;
                        bwAdd.RunWorkerCompleted+=bwAdd_RunWorkerCompleted;
                        bwAdd.RunWorkerAsync();
                       
                    }
                    else
                    {
                        lblTotal.Label = rm.GetString("lblTotalNoFilesInsideDirRes");
                    }
                }
            }
        }

        void bwAdd_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnDirectory.Enabled = true;
            btnImport.Enabled = true;
            btnHelp.Enabled = true;
            btnClean.Enabled = true;
            if(e.Error!=null)
            {
                lblTotal.Label = rm.GetString("lblTotalGenericErrorRes") + e.Error.Message;
            }
            else
            {

            }
        }

        void bwAdd_DoWork(object sender, DoWorkEventArgs e)
        {
            //null check earlier
            if (ma == null)
            {
                ma = new MailAdder(mapiMimeClass.LstMapi, targetDirOutlook, this.App, pathWithAttachmentsAfter,rm);
                ma.TotalNumberOfFilesEvent += ma_TotalNumberOfFilesEvent;
                ma.NewFilesNumberEvent += ma_NumberOfNewFilesEvent;
                ma.ConvertedFilesNumberEvent += ma_ConvertedNumberEvent;
                ma.AddMailsToFolder();
            }
            else
            {
                ma.AddMailsToFolder();
            }
        }

        void ma_ConvertedNumberEvent(string obj)
        {
            mapiMime_ConvertedFilesNumberEvent(obj);
        }

        void ma_NumberOfNewFilesEvent(string obj)
        {
            mapiMime_NewFilesNumberEvent(obj);
        }

        void ma_TotalNumberOfFilesEvent(string obj)
        {
            mapiMime_TotalNumberOfFilesEventEvent(obj);
        }
        #endregion
        
        #region Directory Operations
        private void btnDirectory_Click(object sender, RibbonControlEventArgs e)
                {
                    bool result = GetMailFolder();
                    if (result)
                    {
                        //set path to folder with attachments
                        pathWithAttachmentsAfter = pathAfter.Replace("\\Indexed", "");
                        pathWithAttachmentsAfter = pathWithAttachmentsAfter.Replace("Mail\\15", "Att");

                        BackgroundWorker bw = new BackgroundWorker();
                        bw.DoWork += bw_DoWork;
                        bw.RunWorkerCompleted += bw_RunWorkerCompleted;
                        btnImport.Enabled = false;
                        btnDirectory.Enabled = false;
                        btnHelp.Enabled = false;
                        btnClean.Enabled = false;
                

                        bw.RunWorkerAsync();
                    }
            
            
                }

                void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
                {
                    btnImport.Enabled = true;
                    btnDirectory.Enabled = true;
                    btnHelp.Enabled = true;
                    btnClean.Enabled = true;
                    if(e.Error==null&&(bool)e.Result==true)
                    {
                
                    }
                    else
                    {
                        if(e.Error!=null)
                        {
                            MessageBox.Show(rm.GetString("GenericErrorRes"), e.Error.Message);
                        }
                
                    }
                }

                void bw_DoWork(object sender, DoWorkEventArgs e)
                {
                    mapiMimeClass = new MrMapiConverter(pathAfter,App,rm);
                    mapiMimeClass.NewFilesNumberEvent += mapiMime_NewFilesNumberEvent;
                    mapiMimeClass.ConvertedFilesNumberEvent += mapiMime_ConvertedFilesNumberEvent;
                    mapiMimeClass.TotalNumberOfFilesEvent += mapiMime_TotalNumberOfFilesEventEvent;
                    e.Result = mapiMimeClass.CheckDirectoryAndConvert();
                }

                void mapiMime_TotalNumberOfFilesEventEvent(string obj)
                {
                    lblTotal.Label = obj;
                }

                void mapiMime_ConvertedFilesNumberEvent(string obj)
                {
                    lblConverted.Label = obj;
                }

                void mapiMime_NewFilesNumberEvent(string obj)
                {
                    lblNew.Label = obj;
                }

       
                private string FindMimeMailsInside(string path)
                {
                    List<string> dirs = new DirectoryInfo(path).EnumerateDirectories().Where(d => d.EnumerateFiles("*.eml", SearchOption.AllDirectories).Any())
                                        .Select(d => d.Name).ToList();

                    if(dirs.Count>=1)
                    {
                        return FindMimeMailsInside(path + "\\" + dirs[0]);
                    }
                    else
                    {
                        return path;
                    }
            
                }

                private void EnumerateFoldersInDefaultStore()
                {
                    Outlook.Folder root = App.Session.DefaultStore.GetRootFolder() as Outlook.Folder;
                    EnumerateFolders(root);
                }

        
                private void EnumerateFolders(Outlook.Folder folder)
                {
                    Outlook.Folders childFolders =
                        folder.Folders;
                    if (childFolders.Count > 0)
                    {
                        foreach (Outlook.Folder childFolder in childFolders)
                        {
                    
                            lstOutlookDirs.Add(childFolder.FolderPath);
                            EnumerateFolders(childFolder);
                        }
                    }
                }

                private bool GetMailFolder()
                {
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Packages\\microsoft.windowscommunicationsapps_8wekyb3d8bbwe\\LocalState\\Indexed\\LiveComm";
                    FolderBrowserDialog fbd = new FolderBrowserDialog();
                    fbd.ShowNewFolderButton = false;
                    if (Directory.Exists(path))
                    {
                        string sciezkaInside = FindMimeMailsInside(path);
                        fbd.SelectedPath = sciezkaInside;
                    }

                    DialogResult dr = fbd.ShowDialog();
                    if (dr == DialogResult.OK)
                    {
                        pathAfter = fbd.SelectedPath;
                        if (path != pathAfter)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                                
                }
        #endregion

        #region Directory Cleaning
        private void btnClean_Click(object sender, RibbonControlEventArgs e)
        {
            if (!string.IsNullOrEmpty(pathAfter))
            {
                BackgroundWorker bwKasowanie = new BackgroundWorker();
                bwKasowanie.DoWork += bwDelete_DoWork;
                bwKasowanie.RunWorkerCompleted += bwDelete_RunWorkerCompleted;
                btnClean.Enabled = false;
                btnImport.Enabled = false;
                btnDirectory.Enabled = false;
                btnHelp.Enabled = false;
                bwKasowanie.RunWorkerAsync();
            }
            else
            {
                if(GetMailFolder())
                {
                    BackgroundWorker bwDelete = new BackgroundWorker();
                    bwDelete.DoWork += bwDelete_DoWork;
                    bwDelete.RunWorkerCompleted += bwDelete_RunWorkerCompleted;
                    btnClean.Enabled = false;
                    btnImport.Enabled = false;
                    btnDirectory.Enabled = false;
                    btnHelp.Enabled = false;
                    bwDelete.RunWorkerAsync();
                }
            }

        }

        void bwDelete_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnClean.Enabled = true;
            btnImport.Enabled = false;
            btnDirectory.Enabled = true;
            btnHelp.Enabled = true;
            if(e.Error!=null)
            {
                mapiMime_TotalNumberOfFilesEventEvent(rm.GetString("lblTotalErrorDeletingRes") +e.Error.Message);
            }
            else
            {
                if ((bool)e.Result)
                {
                    mapiMime_TotalNumberOfFilesEventEvent(rm.GetString("lblTotalDelSuccessRes"));
                }
                else
                {
                    mapiMime_TotalNumberOfFilesEventEvent(rm.GetString("lblTotalDelNotRes"));
                }


            }
            
        }

        void bwDelete_DoWork(object sender, DoWorkEventArgs e)
        {
            MailDeleter md = new MailDeleter(pathAfter,rm);
            md.TotalNumberOfFilesEvent += md_TotalNumberOfFilesEvent;
            md.NewFilesNumberEvent += md_NewFilesNumberEvent;
            md.ConvertedFilesNumberEvent += md_ConvertedFilesNumberEvent;
            if (ma != null && ma.LstFDMi != null && ma.LstFDMi.Count > 0)
            {
                e.Result = md.CleanFolderMapiFiles(ma.LstFDMi);
            }
            else
            {
                e.Result = md.CleanFolderMapiFiles();
            }
        }

        void md_ConvertedFilesNumberEvent(string obj)
        {
            mapiMime_ConvertedFilesNumberEvent(obj);
        }

        void md_NewFilesNumberEvent(string obj)
        {
            mapiMime_NewFilesNumberEvent(obj);
        }

        void md_TotalNumberOfFilesEvent(string obj)
        {
            mapiMime_TotalNumberOfFilesEventEvent(obj);
        }
        #endregion

        #region Program Helper
        private void btnHelp_Click(object sender, RibbonControlEventArgs e)
        {
            Assembly assemblyInfo = System.Reflection.Assembly.GetExecutingAssembly();
            Uri uriCodeBase = new Uri(assemblyInfo.CodeBase);
            string ClickOnceLocation = Path.GetDirectoryName(uriCodeBase.LocalPath.ToString());
            
            if (Thread.CurrentThread.CurrentCulture.Name == "pl-PL") 
            {
                
                Help.ShowHelp(new Form(), ClickOnceLocation+"\\help_mailsync_pl.chm");
            }
            else
            {
                Help.ShowHelp(new Form(), ClickOnceLocation+"\\help_mailsync.chm");
            }
        }
        #endregion

    }
}
