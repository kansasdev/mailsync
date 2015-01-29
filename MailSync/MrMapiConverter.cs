using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace MailSync
{
    public class MrMapiConverter
    {
        private string _pathWithMails;
        private List<string> _lstMime;
        public List<string> LstMapi;
        private Outlook.Application App;
        private ResourceManager _rm;

        public event Action<string> TotalNumberOfFilesEvent;
        public event Action<string> ConvertedFilesNumberEvent;
        public event Action<string> NewFilesNumberEvent;

        public MrMapiConverter(string pathWithMails,Microsoft.Office.Interop.Outlook.Application app,ResourceManager rm)
        {
            _pathWithMails = pathWithMails;
            App = app;
            _rm = rm;
        }

        public bool CheckDirectoryAndConvert()
        {
            try
            {
                LstMapi = new List<string>();
                _lstMime = new List<string>();

                string[] pathMime = Directory.GetFiles(_pathWithMails, "*.eml");

                int mimeNumber = 0;
                int mapiNumber = 0;

                OnTotalNumberOfFilesEvent(string.Format("{0} {1} ", pathMime.Length,_rm.GetString("strFilesInsideDirectoryRes")));

                foreach(string p in pathMime)
                {
                    if(p.EndsWith("eml"))
                    {
                        mimeNumber++;
                        OnNewFilesNumberEvent(string.Format("{0} {1}",mimeNumber,_rm.GetString("strMimeFilesInsideDirectoryRes")));
                        _lstMime.Add(p);
                    }
                    
                }

                string[] pathMapi = Directory.GetFiles(_pathWithMails, "*.msg");
                OnTotalNumberOfFilesEvent(string.Format("{0} {1}", pathMime.Length + pathMapi.Length, _rm.GetString("strFilesInsideDirectoryRes")));
               
                foreach (string p in pathMapi)
                {
                    if (p.EndsWith("msg"))
                    {
                        //check whether corresponding eml exists (windows mail deletes eml during synchro)
                        if (_lstMime.Where(q => q.StartsWith(p.Substring(0, p.LastIndexOf(".")))).FirstOrDefault() != null)
                        {
                            mapiNumber++;
                            OnConvertedFilesNumberEvent(string.Format("{0} {1}", mapiNumber, _rm.GetString("strMapiFilesInsideDirectoryRes")));
                            LstMapi.Add(p);
                        }
                    }

                }

                mapiNumber = 0;
                bool isX64 = Is64Bit(App);
                foreach(string mime in _lstMime)
                {
                    string substMime = mime.Substring(0, mime.LastIndexOf("."));
                    if(LstMapi.Where(q=>q.StartsWith(substMime)).FirstOrDefault()==null)
                    {
                        string wynik = InvokeMrMapi(mime, mime.Replace(".eml", ".msg"), isX64);
                        if(wynik.EndsWith("successfully.\r\n"))
                        {
                            mapiNumber++;
                            OnConvertedFilesNumberEvent(string.Format("{0} {1}", mapiNumber, _rm.GetString("strMapiFilesInsideDirectoryRes")));
                            OnTotalNumberOfFilesEvent(string.Format("{0} {1}", pathMime.Length + mapiNumber, _rm.GetString("strFilesInsideDirectoryRes")));
                            LstMapi.Add(substMime + ".msg");
                        }
                    }
                }

                return true;
            }
            catch(Exception ex)
            {
                OnTotalNumberOfFilesEvent(_rm.GetString("GenericErrorRes") + ex.Message);
                return false;
            }
        }
        
        private string InvokeMrMapi(string input,string output,bool isx64)
        {
            //Get the assembly informationSystem.Reflection.Assembly
            System.Reflection.Assembly assemblyInfo = System.Reflection.Assembly.GetExecutingAssembly();

            //Location is where the assembly is run from 
            string assemblyLocation = assemblyInfo.Location;

            //CodeBase is the location of the ClickOnce deployment files
            Uri uriCodeBase = new Uri(assemblyInfo.CodeBase);
            string lokalizacja = Path.GetDirectoryName(uriCodeBase.LocalPath.ToString());

            
            // Start the child process.
            Process p = new Process();
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardOutput = true;
            if(!isx64)
            {
                p.StartInfo.FileName = lokalizacja+"\\mrmapi.exe";
            }
            else
            {
                p.StartInfo.FileName = lokalizacja+"\\mrmapi_x64.exe";
            }
            p.StartInfo.Arguments = string.Format("-Ma -i {0} -o {1} -Cc CCSF_SMTP", input, output);
            p.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
            p.Start();
            // Do not wait for the child process to exit before
            // reading to the end of its redirected stream.
            // p.WaitForExit();
            // Read the output stream first and then wait.
            string outputConsole = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            return outputConsole;

        }

        private bool Is64Bit(Outlook.Application app)
        {
            if (app.Version.StartsWith("15") || app.Version.StartsWith("16") || app.Version.StartsWith("17"))
            {
                RegistryKey rk = Registry.LocalMachine;
                RegistryKey sk = rk.OpenSubKey("SOFTWARE\\Microsoft\\Office\\15.0\\Outlook");
                string architektura = (string)sk.GetValue("Bitness");
                sk.Close();
                rk.Close();
                
                if(architektura=="x64")
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

        private void OnTotalNumberOfFilesEvent(string s)
        {
            if(TotalNumberOfFilesEvent!=null)
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
            if(NewFilesNumberEvent!=null)
            {
                NewFilesNumberEvent(s);
            }
        }

    }
}
