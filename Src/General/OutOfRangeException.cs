using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace General
{
   /// <summary>
    /// The exception that is thrown when the value of an argument is outside the allowable range of values.
    /// This is an extension of <see cref="ArgumentOutOfRangeException"/>.
    /// </summary>
    public class OutOfRangeException : ArgumentOutOfRangeException, IException
    {
        /// <summary>
        /// Gets valid values for the parameters which this <see cref="OutOfRangeException"/> has been raised for.
        /// </summary>
        public string ValidValues { get; protected set; }

        /// <summary>
        /// Initializes a new instance of <see cref="OutOfRangeException"/>.
        /// </summary>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="actualValue">The value of the argument that causes this exception.</param>
        /// <param name="validValues">The valid values for <paramref name="paramName"/>.</param>
        /// <param name="message">The message that describes the error.</param>
        public OutOfRangeException(string paramName, Object actualValue, string validValues, string message)
            : base(paramName, actualValue, message)
        {
            this.ValidValues = validValues;
        }
    }
}
