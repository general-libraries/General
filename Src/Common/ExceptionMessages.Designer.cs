﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Common {
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
    internal class ExceptionMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ExceptionMessages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Common.ExceptionMessages", typeof(ExceptionMessages).Assembly);
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
        ///   Looks up a localized string similar to The value of appSetting &apos;{0}&apos; is &apos;{1}&apos; which cannot be converted to the type &apos;{2}&apos;..
        /// </summary>
        internal static string BadAppSettingException {
            get {
                return ResourceManager.GetString("BadAppSettingException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Company Name cannot be found. Either add the company name to the [assembly: AssemblyCompany(&quot;&quot;)] attribute of AssemblyInfo.cs or add a new item to the &lt;appSettings&gt; node of Web.config (or App.config) with this key: &quot;{0}&quot;..
        /// </summary>
        internal static string CompanyNameMissingException {
            get {
                return ResourceManager.GetString("CompanyNameMissingException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to EmptyCollectionException.
        /// </summary>
        internal static string EmptyCollectionException {
            get {
                return ResourceManager.GetString("EmptyCollectionException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to GlobalSetting cannot be re-initialized. Call &lt;see cref=&quot;GlobalSetting&quot;/&gt;.Initialize() method only one time at the starting point of the application (E.g. Application_Start() method inside Global.asax.cs file)..
        /// </summary>
        internal static string GlobalSettingReinitializeException {
            get {
                return ResourceManager.GetString("GlobalSettingReinitializeException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;{0}&quot; is not defined in the Application Setting collection. 
        ///Add &quot;{1}&quot; to &lt;appSettings&gt; node of the application Web.config or App.config..
        /// </summary>
        internal static string MissingAppSettingException {
            get {
                return ResourceManager.GetString("MissingAppSettingException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MoreThanOneObjectFoundException.
        /// </summary>
        internal static string MoreThanOneObjectFoundException {
            get {
                return ResourceManager.GetString("MoreThanOneObjectFoundException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not a right type..
        /// </summary>
        internal static string NotRightTypeValidationError {
            get {
                return ResourceManager.GetString("NotRightTypeValidationError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ObjectNotFoundException.
        /// </summary>
        internal static string ObjectNotFoundException {
            get {
                return ResourceManager.GetString("ObjectNotFoundException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to OutOfRangeException.
        /// </summary>
        internal static string OutOfRangeException {
            get {
                return ResourceManager.GetString("OutOfRangeException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to XmlTransformationException.
        /// </summary>
        internal static string XmlTransformationException {
            get {
                return ResourceManager.GetString("XmlTransformationException", resourceCulture);
            }
        }
    }
}