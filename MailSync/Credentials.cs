using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MailSync
{
    public class Credentials
    {
        [DllImport("ole32.dll")]
        public static extern void CoTaskMemFree(IntPtr ptr);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct CREDUI_INFO
        {
            public int cbSize;
            public IntPtr hwndParent;
            public string pszMessageText;
            public string pszCaptionText;
            public IntPtr hbmBanner;
        }


        [DllImport("credui.dll", CharSet = CharSet.Auto)]
        private static extern bool CredUnPackAuthenticationBuffer(int dwFlags,
                                                                   IntPtr pAuthBuffer,
                                                                   uint cbAuthBuffer,
                                                                   StringBuilder pszUserName,
                                                                   ref int pcchMaxUserName,
                                                                   StringBuilder pszDomainName,
                                                                   ref int pcchMaxDomainame,
                                                                   StringBuilder pszPassword,
                                                                   ref int pcchMaxPassword);

        [DllImport("credui.dll", CharSet = CharSet.Auto)]
        private static extern int CredUIPromptForWindowsCredentials(ref CREDUI_INFO notUsedHere,
                                                                     int authError,
                                                                     ref uint authPackage,
                                                                     IntPtr InAuthBuffer,
                                                                     uint InAuthBufferSize,
                                                                     out IntPtr refOutAuthBuffer,
                                                                     out uint refOutAuthBufferSize,
                                                                     ref bool fSave,
                                                                     int flags);



        public void GetCredentials(string title,string mess, ref string user, ref string pass)
        {
            CREDUI_INFO credui = new CREDUI_INFO();
            credui.pszCaptionText = title;
            credui.pszMessageText = mess;
            credui.cbSize = Marshal.SizeOf(credui);
            uint authPackage = 0;
            IntPtr outCredBuffer = new IntPtr();
            uint outCredSize;
            bool save = false;
            int result = -1;
            while (true)
            {
                result = CredUIPromptForWindowsCredentials(ref credui,
                                                               0,
                                                               ref authPackage,
                                                               IntPtr.Zero,
                                                               0,
                                                               out outCredBuffer,
                                                               out outCredSize,
                                                               ref save,
                                                               1 /* Generic */);
                if(result!=1223)
                {
                    break;
                }
            }

            var usernameBuf = new StringBuilder(100);
            var passwordBuf = new StringBuilder(100);
            var domainBuf = new StringBuilder(100);

            int maxUserName = 100;
            int maxDomain = 100;
            int maxPassword = 100;
            if (result == 0)
            {
                if (CredUnPackAuthenticationBuffer(0, outCredBuffer, outCredSize, usernameBuf, ref maxUserName,
                                                   domainBuf, ref maxDomain, passwordBuf, ref maxPassword))
                {
                    //TODO: ms documentation says we should call this but i can't get it to work
                    //SecureZeroMem(outCredBuffer, outCredSize);

                    //clear the memory allocated by CredUIPromptForWindowsCredentials 
                    CoTaskMemFree(outCredBuffer);
                    user = usernameBuf.ToString();
                    pass = passwordBuf.ToString();
                    return;
                }
            }

           
        }
    }
}
