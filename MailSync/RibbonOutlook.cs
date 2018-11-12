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
using System.Threading.Tasks;
using System.Security;
using System.Runtime.InteropServices;
using MailSync.Properties;
using System.Security.Cryptography;
using System.Deployment.Application;

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
        private EASDialog eas;

        private string syncKey = string.Empty;
        private bool Error449 = false;
        private bool Error401 = false;

       
        string user = string.Empty;
        string Pass = null;
       

        

        private void RibbonOutlook_Load(object sender, RibbonUIEventArgs e)
        {
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                ApplicationDeployment applicationDeployment = ApplicationDeployment.CurrentDeployment;
                Version version = applicationDeployment.CurrentVersion;
                string wersja = String.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
                btnHelp.ScreenTip = wersja;
            }
           
            btnImport.Enabled = false;
          

        }


        #region Mail Importing
        private void btnImport_Click(object sender, RibbonControlEventArgs e)
        {

            lblConverted.Label = "";
            lblNew.Label = "";
            lblTotal.Label = "";        
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
                FolderDecision fd = new FolderDecision(lstOutlookDirs,false);
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
                        btnConfig.Enabled = false;

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
            btnConfig.Enabled = true;
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
           
           ma = new MailAdder(mapiMimeClass.LstMapi, targetDirOutlook, this.App, pathWithAttachmentsAfter,rm);
           ma.TotalNumberOfFilesEvent += ma_TotalNumberOfFilesEvent;
           ma.NewFilesNumberEvent += ma_NumberOfNewFilesEvent;
           ma.ConvertedFilesNumberEvent += ma_ConvertedNumberEvent;
           ma.AddMailsToFolder();
           
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
            lblConverted.Label = "";
            lblNew.Label = "";
            lblTotal.Label = "";
            bool result = GetMailFolder();
                    if (result)
                    {
                //set path to folder with attachments
                //pathWithAttachmentsAfter = pathAfter.Replace("\\Indexed", "");
                //pathWithAttachmentsAfter = pathWithAttachmentsAfter.Replace("Mail\\15", "Att");
                        pathWithAttachmentsAfter = string.Empty;
                                                
                        BackgroundWorker bw = new BackgroundWorker();
                        bw.DoWork += bw_DoWork;
                        bw.RunWorkerCompleted += bw_RunWorkerCompleted;
                        btnImport.Enabled = false;
                        btnDirectory.Enabled = false;
                        btnHelp.Enabled = false;
                        btnClean.Enabled = false;
                        btnConfig.Enabled = false;
                        btnSync.Enabled = false;

                        bw.RunWorkerAsync();
                    }
            
            
                }

                void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
                {
                    btnImport.Enabled = true;
                    btnDirectory.Enabled = true;
                    btnHelp.Enabled = true;
                    btnClean.Enabled = true;
                    btnConfig.Enabled = true;
                    btnSync.Enabled = true;
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
                    //string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Packages\\microsoft.windowscommunicationsapps_8wekyb3d8bbwe\\LocalState\\Indexed\\LiveComm";
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Mailbox";
                    FolderBrowserDialog fbd = new FolderBrowserDialog();
                    fbd.ShowNewFolderButton = false;
                    
                    if (Directory.Exists(path))
                    {
                        //string sciezkaInside = FindMimeMailsInside(path);
                        //fbd.SelectedPath = sciezkaInside;
                        if (eas != null && !string.IsNullOrEmpty(eas.mailDir))
                        {
                            fbd.SelectedPath = eas.mailDir;
                        }
                        else
                        {
                            fbd.SelectedPath = path;
                        }
                    }
                    else
                    {
                        Directory.CreateDirectory(path);
                        //string sciezkaInside = FindMimeMailsInside(path);
                        //fbd.SelectedPath = sciezkaInside;
                        fbd.SelectedPath = path;
                    }
                    
                    DialogResult dr = fbd.ShowDialog();
                    if (dr == DialogResult.OK)
                    {
                        pathAfter = fbd.SelectedPath;
                        if (path != pathAfter && Directory.EnumerateFiles(pathAfter,"*.eml").Count()>0)
                        {
                            return true;
                        }
                        else
                        {
                            lblTotal.Label = rm.GetString("strNoEmlInside");
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
            lblConverted.Label = "";
            lblNew.Label = "";
            lblTotal.Label = "";
            if (!string.IsNullOrEmpty(pathAfter))
            {
                BackgroundWorker bwKasowanie = new BackgroundWorker();
                bwKasowanie.DoWork += bwDelete_DoWork;
                bwKasowanie.RunWorkerCompleted += bwDelete_RunWorkerCompleted;
                btnClean.Enabled = false;
                btnImport.Enabled = false;
                btnDirectory.Enabled = false;
                btnHelp.Enabled = false;
                btnConfig.Enabled = false;
                btnSync.Enabled = false;
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
                    btnSync.Enabled = false;
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
            btnSync.Enabled = true;
            btnConfig.Enabled = true;
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
                e.Result = md.CleanFolderMapiMimeFiles(ma.LstFDMi);
            }
            else
            {
                e.Result = md.CleanFolderMapiMimeFiles();
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

        #region syncing
        private void btnSync_Click(object sender, RibbonControlEventArgs e)
        {
            lblConverted.Label = "";
            lblNew.Label = "";
            lblTotal.Label = "";
            if (Properties.Settings.Default.IsExchange)
            {

            }
            else
            {
                if (string.IsNullOrEmpty(Settings.Default.EASServer) || string.IsNullOrEmpty(Settings.Default.ProtocolVersion))
                {
                    SettingsForm sf = new SettingsForm();
                    sf.ShowDialog();
                }

                if (!string.IsNullOrEmpty(Settings.Default.EASServer) && !string.IsNullOrEmpty(Settings.Default.ProtocolVersion) && !string.IsNullOrEmpty(Settings.Default.DevID))
                {
                    btnImport.Enabled = false;
                    btnDirectory.Enabled = false;
                    btnHelp.Enabled = false;
                    btnClean.Enabled = false;
                    btnConfig.Enabled = false;
                    btnSync.Enabled = false;
                    DialogResult dr = DialogResult.OK;
                    if (string.IsNullOrEmpty(Settings.Default.Username) || string.IsNullOrEmpty(Settings.Default.Password) || Error401)
                    {
                        Credentials cred = new Credentials();
                        cred.GetCredentials(rm.GetString("credTitle"), rm.GetString("credMessage"), ref user, ref Pass);


                        if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(Pass))
                        {
                            dr = DialogResult.No;
                        }
                        else
                        {
                            Settings.Default.Username = user;
                            Settings.Default.Password = UTF8Encoding.Default.GetString(ProtectedData.Protect(UTF8Encoding.Default.GetBytes(Pass), null, DataProtectionScope.CurrentUser));
                        }

                    }

                    if (dr == DialogResult.OK)
                    {

                        eas = new EASDialog(rm, Settings.Default.Username, UTF8Encoding.Default.GetString(ProtectedData.Unprotect(UTF8Encoding.Default.GetBytes(Settings.Default.Password), null, DataProtectionScope.CurrentUser)), Settings.Default.EASServer, Settings.Default.ProtocolVersion, Settings.Default.DevID, Settings.Default.DevType);
                        eas.TotalNumberOfFilesEvent += md_TotalNumberOfFilesEvent;
                        eas.NewFilesNumberEvent += md_NewFilesNumberEvent;
                        eas.ConvertedFilesNumberEvent += md_ConvertedFilesNumberEvent;
                        string kom = string.Empty;

                        bool result = eas.Initialize(ref kom);
                        if (result)
                        {
                            BackgroundWorker bwOnline = new BackgroundWorker();

                            bwOnline.DoWork += BwOnline_DoWork;
                            bwOnline.RunWorkerCompleted += BwOnline_RunWorkerCompleted;
                            bwOnline.RunWorkerAsync();
                        }
                    }
                    else
                    {
                        btnImport.Enabled = true;
                        btnDirectory.Enabled = true;
                        btnHelp.Enabled = true;
                        btnClean.Enabled = true;
                        btnConfig.Enabled = true;
                        btnSync.Enabled = true;
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("settingsMess"));
                }
            }
        }

        private void BwOnline_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnClean.Enabled = true;
            btnImport.Enabled = false;
            btnDirectory.Enabled = true;
            btnHelp.Enabled = true;
            btnConfig.Enabled = true;
            btnSync.Enabled = true;
            if (e.Error != null)
            {
                if (e.Error.Message.Contains("449"))
                {
                    mapiMime_ConvertedFilesNumberEvent(rm.GetString("strSyncProvision"));
                    Error449 = true;
                }
                else if (e.Error.Message.Contains("401"))
                {
                    mapiMime_ConvertedFilesNumberEvent(rm.GetString("strSyncWrongUserPass"));
                    Error401 = true;
                }
                else
                {
                    mapiMime_ConvertedFilesNumberEvent(e.Error.Message);
                }
            }
            else
            {
                Error401 = false;
                Error449 = false;
                Settings.Default.SyncKey = syncKey;
                Settings.Default.Save();
            }
        }

        private void BwOnline_DoWork(object sender, DoWorkEventArgs e)
        {
            
            string kom = string.Empty;
            if (Error449||string.IsNullOrEmpty(syncKey))
            {
                //error viewing inside
                bool res = eas.SetConnection(ref syncKey, ref kom);
                if(res)
                {
                    Error449 = false;
                    e.Result = eas.SetConversation(syncKey, "0", ref kom);
                    
                }
                else
                {
                    throw new Exception(kom);
                }

            }
            else
            {
                e.Result = eas.SetConversation(syncKey,"0", ref kom);
            }
            if(!(bool)e.Result)
            {
                
                //jezeli provision wykonaj provisioning
                throw new Exception(kom);
            }
        }

        #endregion

        #region Config

        private void btnConfig_Click(object sender, RibbonControlEventArgs e)
        {
            SettingsForm sf = new SettingsForm();
            sf.ShowDialog();
        }

        #endregion

    }
}
