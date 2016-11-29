using System;

namespace General
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MoreThanOneObjectFoundException<T> : Exception,IException
        where T : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public MoreThanOneObjectFoundException(string propertyName, object value)
            : base(string.Format(ExceptionMessages.MoreThanOneObjectFoundException, typeof(T).Name, propertyName, value))
        {
        }
    }
}
