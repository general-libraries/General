using General.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace General.Cryptography
{
    /// <summary>
    /// Decryption Exception.
    /// </summary>
    public class DecryptionException : Exception, IException
    {
        /// <summary>
        /// Initializes a new instance of <see cref="DecryptionException"/>.
        /// </summary>
        /// <param name="cipherBytes"></param>
        /// <param name="vector"></param>
        /// <param name="innerException"></param>
        public DecryptionException(byte[] cipherBytes, byte[] vector, Exception innerException = null)
            : base(BuildErrorMessage(cipherBytes, vector), innerException)
        { }

        /// <summary>
        /// Builds the error message.
        /// </summary>
        /// <param name="cipherBytes"></param>
        /// <param name="vector"></param>
        /// <returns>Error message</returns>
        private static string BuildErrorMessage(byte[] cipherBytes, byte[] vector)
        {
            return
                $"Error in decryption. \ncipherText = {StringHelper.ByteArrayToString(cipherBytes)} \nvector = {StringHelper.ByteArrayToString(vector)}";
        }
    }
}
