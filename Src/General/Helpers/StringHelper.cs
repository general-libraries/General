using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace General.Helpers
{
    /// <summary>
    /// Encapsulates extention methods for <see cref="String"/> type.
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="splitter"></param>
        /// <returns></returns>
        public static byte[] StringToByteArray(string str, char splitter = Char.MinValue)
        {
            string[] stringArray;

            if (splitter == Char.MinValue)
            {
                stringArray = SplitInParts(str, 3).ToArray<string>();
            }
            else
            {
                stringArray = str.Split(splitter);
            }


            byte[] byteArray = new byte[stringArray.Length];

            for (int i = 0; i < stringArray.Length; i++)
            {
                byteArray[i] = Byte.Parse(stringArray[i]);
            }

            return byteArray;
        }

        /// <summary>
        /// Converts a byte array to a string.
        /// <seealso cref="StringToByteArray"/>
        /// Same comment as StrToByteArray(string).  Normally the conversion would use an ASCII encoding in the other direction:
        ///      System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
        ///      return enc.GetString(byteArr);
        /// </summary>
        /// <param name="byteArr">An array of bytes.</param>
        /// <returns>string</returns>
        public static string ByteArrayToString(byte[] byteArr)
        {
            byte val;
            string tempStr = "";
            for (int i = 0; i <= byteArr.GetUpperBound(0); i++)
            {
                val = byteArr[i];
                if (val < (byte)10)
                    tempStr += "00" + val.ToString();
                else if (val < (byte)100)
                    tempStr += "0" + val.ToString();
                else
                    tempStr += val.ToString();
            }
            return tempStr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="partLength"></param>
        /// <returns></returns>
        public static IEnumerable<string> SplitInParts(String str, int partLength)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (partLength <= 0)
            {
                throw new ArgumentException("Part length has to be positive.", nameof(partLength));
            }

            for (var i = 0; i < str.Length; i += partLength)
            {
                yield return str.Substring(i, Math.Min(partLength, str.Length - i));
            }
        }
    }
}
