using System;

namespace General
{
    /// <summary>
    /// Exception when the app cannot find an specific object.
    /// </summary>
    /// <typeparam name="T">Type of an entity.</typeparam>
    public class ObjectNotFoundException<T> : Exception, IException
        where T : class
    {
        /// <summary>
        /// ID or name of the instance of <typeparamref name="T"/> that cannot be found.
        /// </summary>
        public object IdOrName { get; protected set; }

        /// <summary>
        /// Initialize an instance of <see cref="ObjectNotFoundException{T}"/>.
        /// </summary>
        /// <param name="idOrName">ID or name of the instance of <typeparamref name="T"/> that cannot be found.</param>
        public ObjectNotFoundException(object idOrName)
            : base(string.Format(ExceptionMessages.ObjectNotFoundException, typeof(T).Name, idOrName))
        {
            this.IdOrName = idOrName;
        }
    }
}
