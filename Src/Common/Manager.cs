using System;
using System.Data;
using System.Reflection;
using Common.Configuration;

namespace Common
{
    /// <summary>
    /// A singleton implementation to encapsulate necessary elements for the Common Library. 
    /// </summary>
    public sealed class Manager
    {
        #region Singleton Implementation

        private static readonly Lazy<Manager> LazyMe = new Lazy<Manager>(() => new Manager());

        /// <summary>
        /// Gets the single and the only instance of the <see cref="Manager"/>.
        /// </summary>
        public static Manager Instance { get { return LazyMe.Value; } }

        private Manager()
        {
            _serviceProvider = new CommonServiceProvider();
        }

        #endregion

        private ICommonServiceProvider _serviceProvider;

        /// <summary>
        /// Gets or sets a Service Provider for the Common library.
        /// </summary>
        /// <value>
        /// A Service Provider for the Common library.
        /// </value>
        /// <remarks>
        /// Becareful if you want to set or make changes to this property. 
        /// Many other classes and functions in the Common Library use this property to get the Service Provider.
        /// </remarks>
        public ICommonServiceProvider ServiceProvider
        {
            get { return _serviceProvider; }
            set
            {
                if (value == null) throw new NoNullAllowedException("ServiceProvider does not accept null value.");
                _serviceProvider = value;
            }
        }

        /// <summary>
        /// Gets a container of global settings by calling <see cref="Manager"/>.Instance.ServiceProveder.Get(). 
        /// </summary>
        /// <value>
        /// A container of global settings.
        /// </value>
        /// <remarks>
        /// Becareful if you want to set or make changes to this property. 
        /// Many other classes and functions in the Common Library use this property to get an initialized instance of <see cref="GlobalSetting"/>.
        /// </remarks>
        public GlobalSetting GlobalSetting
        {
            get { return ServiceProvider.GetService<IGlobalSettingProvider>().Get(); }
        }

        public Assembly ApplicationAssembly { get; set; }
    }
}
