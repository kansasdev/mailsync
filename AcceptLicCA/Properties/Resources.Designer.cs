﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AcceptLicCA.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("AcceptLicCA.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cancel.
        /// </summary>
        internal static string btnCancelRes {
            get {
                return ResourceManager.GetString("btnCancelRes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Install CA.
        /// </summary>
        internal static string btnInstallRes {
            get {
                return ResourceManager.GetString("btnInstallRes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Accepted License and CA.
        /// </summary>
        internal static string FormTitleRes {
            get {
                return ResourceManager.GetString("FormTitleRes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ERROR.
        /// </summary>
        internal static string GenericErrorRes {
            get {
                return ResourceManager.GetString("GenericErrorRes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Mail Sync is Outlook Add-in application which updates itselt using ClickOnce mechanism. Unfortunately I don&apos;t have certificate signed by one of few Certification Authorities which is required by ClickOnce mechanism. If you want to use my software, receive updates it is required you have to trust my public self-signed certificate and install it in Trusted Root Authority in your current user settings..
        /// </summary>
        internal static string InfoRes {
            get {
                return ResourceManager.GetString("InfoRes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] kansas {
            get {
                object obj = ResourceManager.GetObject("kansas", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CA certificate not added.
        /// </summary>
        internal static string NoCAAdded {
            get {
                return ResourceManager.GetString("NoCAAdded", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No CA cert installed.
        /// </summary>
        internal static string NoCAInstalledRes {
            get {
                return ResourceManager.GetString("NoCAInstalledRes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Proper CA cert already installed.
        /// </summary>
        internal static string ProperCAInstalledRes {
            get {
                return ResourceManager.GetString("ProperCAInstalledRes", resourceCulture);
            }
        }
    }
}