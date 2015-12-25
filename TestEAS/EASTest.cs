using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Xml;
using System.Collections.Generic;
using System.Text;

namespace TestEAS
{
    [TestClass]
    public class EASTests       
    {
        [TestMethod]
        public void CheckSerialization()
        {
            try
            {
                 var files = Directory.EnumerateFiles(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)+"\\Mailbox" , "*.txt", SearchOption.TopDirectoryOnly);

                foreach (string f in files)
                {
                    string[] lines = File.ReadAllLines(f);
                    List<string> newLines = new List<string>(lines);
                    newLines.RemoveAt(0);

                    StringBuilder sb = new StringBuilder();
                    foreach(string s in newLines)
                    {
                        sb.AppendLine(s);
                    }
                                

                    XmlReaderSettings xmlReaderSettings = new XmlReaderSettings() { CheckCharacters = false };
                    XmlReader xmlReader = XmlTextReader.Create(new StringReader(sb.ToString()), xmlReaderSettings);

                    XmlSerializer deser = new XmlSerializer(typeof(EAS.generated.SyncResponseNamespace.Sync));
                    EAS.generated.SyncResponseNamespace.Sync sync = (EAS.generated.SyncResponseNamespace.Sync)deser.Deserialize(xmlReader);
                }

                //EAS.generated.SyncResponseNamespace.Sync sync = (EAS.generated.SyncResponseNamespace.Sync)deser.Deserialize(streamOut);
            }
            catch(Exception ex)
            {
                Trace.WriteLine("ERROR: " + ex.Message);
            }
        }
    }
}
