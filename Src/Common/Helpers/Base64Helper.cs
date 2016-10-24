using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers
{
    /// <summary>
    /// An static class to facilitate encoding/decoding to/from Base64.
    /// </summary>
    public static partial class Base64Helper
    {
        /// <summary>
        /// Returns a Base64 encoded string from <paramref name="toEncode"/> using default text encoding (<see cref="Encoding.Default"/>).
        /// </summary>
        /// <param name="toEncode">A string for encoding.</param>
        /// <returns>A base64 encoded string.</returns>
        public static string EncodeToBase64(string toEncode)
        {
            return EncodeToBase64(toEncode, Encoding.Default);
        }

        /// <summary>
        /// Returns a Base64 encoded string from <paramref name="toEncode"/> using <paramref name="encoding"/> text encoding.
        /// </summary>
        /// <param name="toEncode">A string for encoding.</param>
        /// <param name="encoding">A <see cref="System.Text.Encoding"/>class to do the encoding.</param>
        /// <returns>A base64 encoded string.</returns>
        public static string EncodeToBase64(string toEncode, Encoding encoding)
        {
            byte[] toEncodeAsBytes = encoding.GetBytes(toEncode);
            string returnValue = Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }

        /// <summary>
        /// Returns a Base64 encoded string of <paramref name="username"/> and <paramref name="password"/> using default text encoding (<see cref="Encoding.Default"/>).
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <remarks>Return value is usually used for Authorization field of HTTP request header.</remarks>
        /// <returns>A base64 encoded string.</returns>
        public static string EncodeToBase64(string username, string password)
        {
            return EncodeToBase64(username, password, Encoding.Default);
        }

        /// <summary>
        /// Returns a Base64 encoded string of <paramref name="username"/> and <paramref name="password"/> using <paramref name="encoding"/> text encoding.
        /// Return value is usually used for Authorization field of HTTP request header.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="encoding">A <see cref="System.Text.Encoding"/>class to do the encoding.</param>
        /// <returns>A base64 encoded string.</returns>
        public static string EncodeToBase64(string username, string password, Encoding encoding)
        {
            string toEncode = $"{username}:{password}";
            return EncodeToBase64(toEncode, encoding);
        }

        /// <summary>
        /// Returns a dencoded string from <paramref name="encodedData"/> using default text encoding (<see cref="Encoding.Default"/>).
        /// </summary>
        /// <param name="encodedData">A base 64 encoded string.</param>
        /// <returns>A string which is decoded from <paramref name="encodedData"/>.</returns>
        public static string DecodeFromBase64(string encodedData)
        {
            return DecodeFromBase64(encodedData, Encoding.Default);
        }

        /// <summary>
        /// Returns a dencoded string from <paramref name="encodedData"/> using <paramref name="encoding"/> text encoding.
        /// </summary>
        /// <param name="encodedData">A base 64 encoded string.</param>
        /// <param name="encoding">A <see cref="System.Text.Encoding"/>class to do the encoding.</param>
        /// <returns>A string which is decoded from <paramref name="encodedData"/>.</returns>
        public static string DecodeFromBase64(string encodedData, Encoding encoding)
        {
            byte[] encodedDataAsBytes = Convert.FromBase64String(encodedData);
            string returnValue = encoding.GetString(encodedDataAsBytes);
            return returnValue;
        }

        /// <summary>
        /// Extracts username and password from a base64 encoded string using default text encoding (<see cref="Encoding.Default"/>).
        /// </summary>
        /// <param name="encodedData">A base 64 encoded string.</param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <remarks>
        /// If HttpRequestHeaders.Authorization.Parameter is passed as <paramref name="encodedData"/>, decoded username and password would be sent out in <paramref name="username"/> and <paramref name="password"/> respectively.
        /// </remarks>        
        public static void ExtractUsernamePasswordFromBase64(string encodedData, out string username, out string password)
        {
            ExtractUsernamePasswordFromBase64(encodedData, Encoding.Default, out username, out password);
        }

        /// <summary>
        /// Extracts username and password from a base64 encoded string using <paramref name="encoding"/> text encoding.
        /// </summary>
        /// <param name="encodedData">A base 64 encoded string.</param>
        /// <param name="encoding">A <see cref="System.Text.Encoding"/>class to do the encoding.</param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <remarks>
        /// If HttpRequestHeaders.Authorization.Parameter is passed as <paramref name="encodedData"/>, decoded username and password would be sent out in <paramref name="username"/> and <paramref name="password"/> respectively.
        /// </remarks>        
        public static void ExtractUsernamePasswordFromBase64(string encodedData, Encoding encoding, out string username, out string password)
        {
            string[] slices = new string[2];

            try
            {
                string decodeFromBase64 = DecodeFromBase64(encodedData, encoding);
                slices = decodeFromBase64.Split(new char[] { ':' });
            }
            catch (Exception exception)
            {
                throw new Exception("Parameter encodedData cannot be decoded.", exception);
            }
            finally
            {
                if (slices.Count() != 2)
                {
                    throw new Exception("Decoded string from encodedData doesn't have {username:password} scheama.");
                }
            }

            username = slices[0];
            password = slices[1];
        }

        ///<summary>
        /// Base 64 Encoding with URL and Filename Safe Alphabet using UTF-8 character set.
        ///</summary>
        ///<param name="encBuff">The origianl array of byte.</param>
        ///<returns>The Base64 encoded string</returns>
        public static string EncodeToBase64UrlSafe(byte[] encBuff)
        {
            //byte[] encbuff = Encoding.UTF8.GetBytes(str);
            return System.Web.HttpServerUtility.UrlTokenEncode(encBuff);
        }

        ///<summary>
        /// Decode Base64 encoded string with URL and Filename Safe Alphabet using UTF-8.
        ///</summary>
        ///<param name="str">Base64 code</param>
        ///<returns>The decoded string.</returns>
        public static byte[] DecodeFromBase64UrlSafe(string str)
        {
            //byte[] decbuff =
            return System.Web.HttpServerUtility.UrlTokenDecode(str);
            //return Encoding.UTF8.GetString(decbuff);
        }
    }
}
