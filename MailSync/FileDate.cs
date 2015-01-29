using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace MailSync
{
    public class FileDateMI : IComparable<FileDateMI>
    {
        public Outlook.MailItem MI { get; set; }
        public long Tick { get; set; }
        public string Subject { get; set; }
        public string FileName { get; set; }

        public bool IsClosed { get; set; }

        public DateTime ReceivedTime { get; set; }

        public int CompareTo(FileDateMI other)
        {
            if (other.Tick > this.Tick)
                return -1;
            else if (other.Tick == this.Tick)
                return 0;
            else
                return 1;

        }
    }
}
