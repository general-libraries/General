using System;
using System.Collections.Generic;

namespace General
{
    public interface ICommonServiceProvider : IServiceProvider
    {

        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of service object to get.</typeparam>
        /// <returns>A service object of <typeparamref name="T"/>.</returns>
        T GetService<T>();

        /// <summary>
        /// Gets the list of service objects of the specified type. 
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <returns>A list of service object of type <paramref name="serviceType"/>.</returns>
        IEnumerable<object> GetServices(Type serviceType);

        /// <summary>
        /// Gets the list of service objects of the specified type. 
        /// </summary>
        /// <typeparam name="T">The type of service object to get.</typeparam>
        /// <returns>A list of service objects of <typeparamref name="T"/>.</returns>
        IEnumerable<T> GetServices<T>();
    }
}
