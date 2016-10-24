using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Common.Configuration;

namespace Common
{
    /// <summary>
    /// Default implementation of <see cref="ICommonServiceProvider"/> to contain and provide services for the Common Library.
    /// </summary>
    public class CommonServiceProvider : ICommonServiceProvider
    {
        private readonly ConcurrentDictionary<Type, Func<object>> _typeObjDic;
        private readonly ConcurrentDictionary<Type, Func<IEnumerable<object>>> _typeListDic;

        public CommonServiceProvider()
        {
            _typeObjDic = new ConcurrentDictionary<Type, Func<object>>();
            _typeListDic = new ConcurrentDictionary<Type, Func<IEnumerable<object>>>();

            Func<IAssemblyAsGlobalSettingRepository> aagsRepo = () => new AssemblyAsGlobalSettingRepository(Manager.Instance.ApplicationAssembly);
            Func<IConfigFileAsGlobalSettingRepository> cfagsRepo = () => new ConfigFileAsGlobalSettingRepository();

            _typeObjDic.TryAdd(typeof(IAssemblyAsGlobalSettingRepository), aagsRepo);
            _typeObjDic.TryAdd(typeof(IConfigFileAsGlobalSettingRepository), cfagsRepo);
            _typeObjDic.TryAdd(typeof(IGlobalSettingRepository), () => null);
            _typeObjDic.TryAdd(typeof(IGlobalSettingProvider), () => new GlobalSettingProvider(
                    GetService<IAssemblyAsGlobalSettingRepository>(),
                    GetService<IConfigFileAsGlobalSettingRepository>(),
                    GetService<IGlobalSettingRepository>()));
        }

        public virtual object GetService(Type serviceType)
        {
            if (serviceType == null)
                throw new ArgumentNullException("serviceType");

            Func<object> service;

            if (!_typeObjDic.TryGetValue(serviceType, out service))
                throw new ServiceNotFoundException(serviceType);

            return service.Invoke();
        }

        public virtual T GetService<T>()
        {
            var type = typeof(T);
            var result = GetService(type);
            return (T)result;
        }

        public virtual IEnumerable<object> GetServices(Type serviceType)
        {
            if (serviceType == null)
                throw new ArgumentNullException("serviceType");

            Func<IEnumerable<object>> services;

            if (!_typeListDic.TryGetValue(serviceType, out services))
                throw new ServiceNotFoundException(serviceType);

            return services.Invoke();
        }

        public virtual IEnumerable<T> GetServices<T>()
        {
            var type = typeof(T);
            var services = GetServices(type);
            return services.Cast<T>();
        }

        /// <summary>
        /// Adds or replaces a service for a type.
        /// </summary>
        /// <typeparam name="T">The type that would be asked to be resolved.</typeparam>
        /// <param name="service">The response object when code asks for resolving <typeparamref name="T"/>.</param>
        public virtual bool AddOrReplace<T>(object service)
        {
            if (service == null)
                throw new ArgumentNullException("service");

            if (!(service is T))
                throw new ArgumentException("service does not have the correct type.", "service");

            var serviceType = typeof(T);

            return _typeObjDic.TryAdd(serviceType, () => service)
                || _typeObjDic.TryUpdate(serviceType, () => service, _typeObjDic[serviceType]);
        }

        /// <summary>
        /// Adds or replaces a list of services for a type.
        /// </summary>
        /// <typeparam name="T">The type that would be asked to be resolved.</typeparam>
        /// <param name="services">>The response list of objects when code asks for resolving <typeparamref name="T"/>.</param>
        public virtual bool AddOrReplace<T>(IEnumerable<object> services)
        {
            if (services == null)
                throw new ArgumentNullException("services");

            var serviceList = services.ToList();

            if (serviceList.Any(svc => !(svc is T)))
                throw new ArgumentException("One or more items in 'services' do not have the correct type.", "services");

            var serviceType = typeof(T);

            return _typeListDic.TryAdd(serviceType, () => serviceList)
                || _typeListDic.TryUpdate(serviceType, () => serviceList, _typeListDic[serviceType]);
        }
    }

    /// <summary>
    /// Defines an exception thrown when there is no service for the requested type.
    /// </summary>
    public class ServiceNotFoundException : Exception, IException
    {
        /// <summary>
        /// The type that there is no service available for it.
        /// </summary>
        public Type ServiceType { get; private set; }

        /// <summary>
        /// Initializes a new <see cref="ServiceNotFoundException"/>.
        /// </summary>
        /// <param name="type"></param>
        public ServiceNotFoundException(Type type)
            : base(string.Format("Cannot provide a related type for '{0}'.", type == null ? "null" : type.FullName))
        {
            ServiceType = type;
        }
    }
}