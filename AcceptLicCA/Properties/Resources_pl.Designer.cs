﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
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
    internal class Resources_pl {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources_pl() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("AcceptLicCA.Properties.Resources_pl", typeof(Resources_pl).Assembly);
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
        ///   Looks up a localized string similar to Anuluj.
        /// </summary>
        internal static string btnCancelRes {
            get {
                return ResourceManager.GetString("btnCancelRes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Instaluj CA.
        /// </summary>
        internal static string btnInstallRes {
            get {
                return ResourceManager.GetString("btnInstallRes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Akceptacja licencji i CA certyfikatu.
        /// </summary>
        internal static string FormTitleRes {
            get {
                return ResourceManager.GetString("FormTitleRes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to BŁĄD.
        /// </summary>
        internal static string GenericErrorRes {
            get {
                return ResourceManager.GetString("GenericErrorRes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Program Mail Sync jest wtyczką do Outlooka, która do aktualizacji używa mechanizmu ClickOnce. Niestety nie posiadam płatnego kwalifikowanego certyfikatu, podpisanego przez jedno z zaufanych centrów certyfikacji, co jest wymagane przez mechanizm ClickOnce.Jeżeli chcesz używać tego oprogramowania wymagane jest zaufanie mojemu publicznemu autocertyfikatowi i zainstalowanie go w Zaufanych Głównych Urzędach Certyfikacji i w ustawieniach bieżącego użytkownika.
        /// </summary>
        internal static string InfoRes {
            get {
                return ResourceManager.GetString("InfoRes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Certyfikat CA nie został dodany.
        /// </summary>
        internal static string NoCAAdded {
            get {
                return ResourceManager.GetString("NoCAAdded", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Brak odpowiedniego certyfikatu CA.
        /// </summary>
        internal static string NoCAInstalledRes {
            get {
                return ResourceManager.GetString("NoCAInstalledRes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Zainstalowany poprawny certyfikat CA.
        /// </summary>
        internal static string ProperCAInstalledRes {
            get {
                return ResourceManager.GetString("ProperCAInstalledRes", resourceCulture);
            }
        }
    }
}
