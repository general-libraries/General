using Common.Configuration;
using Common.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Common.Cryptography
{
    /// <summary>
    /// Encapsulates members to faciliates AES encryption and decryption.
    /// </summary>
    public sealed class AES
    {
        /// <summary>
        /// Gets or sets a key for AES.
        /// </summary>
        /// <value>Key for AES.</value>
        public byte[] Key
        {
            get { return key; }
            set
            {
                ValidateKey(key);
                key = value;
            }
        }
        private byte[] key;     //Sample: { 123, 217, 19, 11, 24, 26, 85, 45, 114, 184, 27, 162, 37, 112, 222, 209, 241, 24, 175, 144, 173, 53, 196, 29, 24, 26, 17, 218, 131, 236, 53, 209 };

        /// <summary>
        /// Gets default AES vector splitter.
        /// </summary>
        /// <value>AES vector splitter.</value>
        public const char DEFAULT_IV_SPLITTER = ':';

        /// <summary>
        /// Creates a new instance of <see cref="AES"/>
        /// </summary>
        /// <param name="key">AES key</param>
        public AES(byte[] key = null)
        {
            if (key == null)
            {
                key = Manager.Instance.GlobalSetting.AesKey;
            }

            ValidateKey(key);

            this.key = key;
        }

        /// <summary>
        /// Validates AES key.
        /// </summary>
        /// <param name="key">AES key.</param>
        private void ValidateKey(byte[] key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (key.Length != 32)
            {
                throw new ArgumentException(
                    $"AES key lenght is invalid (it is {key.Length} but 32 bytes are expected).", nameof(key));
            }
        }

        /// <summary>
        /// Encrypts <paramref name="plainText"/> and returns a string suitable for passing in a URL.
        /// </summary>
        /// <param name="plainText">Text to be encrypted.</param>
        /// <param name="vector">Generated AES vector.</param>
        /// <returns>Encrypted text.</returns>
        public string EncryptToString(string plainText, out byte[] vector)
        {
            return Base64Helper.EncodeToBase64UrlSafe(Encrypt(plainText, out vector));
        }

        /// <summary>
        /// Encrypts <paramref name="plainText"/> and returns a string suitable for passing in a URL.
        /// </summary>
        /// <param name="plainText">Text to be encrypted.</param>
        /// <param name="vector">Generated AES vector.</param>
        /// <returns>Encrypted text.</returns>
        public string EncryptToString(string plainText, out string vector)
        {
            byte[] iV;
            string result = Base64Helper.EncodeToBase64UrlSafe(Encrypt(plainText, out iV));
            vector = Base64Helper.EncodeToBase64UrlSafe(iV);
            return result;
        }

        /// <summary>
        /// Encrypts <paramref name="plainText"/> and adds vector(IV) at the beginnig of result, using <paramref name="splitter"/> as the separator.
        /// </summary>
        /// <param name="plainText">Text to be encrypted.</param>
        /// <param name="splitter">The character that separate encrypted text and vector(IV).</param>
        /// <returns>Encrypted text.</returns>
        public string EncryptToString(string plainText, char splitter = DEFAULT_IV_SPLITTER)
        {
            string vectorString;
            string encryptedText = EncryptToString(plainText, out vectorString);
            return $"{vectorString}{splitter}{encryptedText}";
        }

        /// <summary>
        /// Encrypts <paramref name="plainText"/> and returns an encrypted byte array.
        /// </summary>
        /// <param name="plainText">Text to be encrypted.</param>
        /// <param name="vector">The generated IV which is used to encrypt <paramref name="plainText"/> and generate the result.</param>
        /// <returns>Encrypted byte array.</returns>
        public byte[] Encrypt(string plainText, out byte[] vector)
        {
            // Check arguments.
            if (string.IsNullOrWhiteSpace(plainText))
                throw new ArgumentNullException(nameof(plainText));

            byte[] result;

            // Create a new instance of the AesManaged
            // class.  This generates a new key and initialization 
            // vector (IV).
            using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
            {
                rijndaelManaged.GenerateIV();
                rijndaelManaged.Key = this.key;
                vector = rijndaelManaged.IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor(rijndaelManaged.Key, rijndaelManaged.IV);

                // Create the streams used for encryption.
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            //Write all data to the stream.
                            streamWriter.Write(plainText);
                        }
                        result = memoryStream.ToArray();
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Decrypts <paramref name="cipherText"/> to plain text.
        /// The method assumes that <paramref name="cipherText"/> is in this format: {vector string}{splitter char}{cipherText}
        /// Sample: 193017219101214006208145014084217162137012222209:949389489834798704520978127843987470834087943609430879843790843
        /// </summary>
        /// <param name="cipherText">Encrypted text.</param>
        /// <param name="splitter">Separator character.</param>
        /// <returns>Plain text.</returns>
        public string Decrypt(string cipherText, char splitter = DEFAULT_IV_SPLITTER)
        {
            int splitterIndex = cipherText.IndexOf(splitter);

            // Not accepted situations:
            //      Splitter not found:             splitterIndex == -1
            //      Splitter is at the beginning:   splitterIndex == 0
            //      Splitter is at the end:         splitterIndex == cipherText.Length
            if (splitterIndex < 1 || splitterIndex == cipherText.Length)
            {
                // Text Format must be: "{vector string}{splitter char}{cipherText}"
                throw new Exception("Text is not in expected format!");
            }

            string vector = cipherText.Substring(0, splitterIndex);
            string encryptedText = cipherText.Substring(splitterIndex + 1);

            return Decrypt(encryptedText, vector);
        }

        /// <summary>
        /// Decrypts <paramref name="cipherText"/>.
        /// </summary>
        /// <param name="cipherText">Encrypted text.</param>
        /// <param name="vector">AES vector(IV).</param>
        /// <returns>Plain text.</returns>
        private string Decrypt(string cipherText, string vector)
        {
            return Decrypt(Base64Helper.DecodeFromBase64UrlSafe(cipherText), Base64Helper.DecodeFromBase64UrlSafe(vector));
        }

        /// <summary>
        /// Decrypts <paramref name="cipherBytes"/> by using <see cref="Key"/> and <paramref name="vector"/>.
        /// </summary>
        /// <param name="cipherBytes">Encrypted data in byte array.</param>
        /// <param name="vector">AES vector(IV) in byte array.</param>
        /// <returns>Plain text.</returns>
        public string Decrypt(byte[] cipherBytes, byte[] vector)
        {
            // Check arguments.
            if (cipherBytes == null || cipherBytes.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (vector == null || vector.Length <= 0)
                throw new ArgumentNullException(nameof(vector));

            // Declare the string used to hold
            // the decrypted text.
            string plainText = string.Empty;

            try
            {
                // Create an AesManaged object
                // with the specified key and IV.
                using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
                {
                    rijndaelManaged.Key = this.key;
                    rijndaelManaged.IV = vector;

                    // Create a decrytor to perform the stream transform.
                    ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor(rijndaelManaged.Key, rijndaelManaged.IV);

                    // Create the streams used for decryption.
                    using (MemoryStream memoryStream = new MemoryStream(cipherBytes))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader(cryptoStream))
                            {
                                // Read the decrypted bytes from the decrypting stream
                                // and place them in a string.
                                plainText = streamReader.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                throw new DecryptionException(cipherBytes, vector, exception);
            }

            return plainText;
        }

        /// <summary>
        /// Tries to decrypt <paramref name="cipherText"/>.
        /// The method assumes that <paramref name="cipherText"/> is in this format: {vector string}{splitter char}{cipherText}
        /// Sample: 193017219101214006208145014084217162137012222209:949389489834798704520978127843987470834087943609430879843790843
        /// </summary>
        /// <param name="cipherText">Encrypted text.</param>
        /// <param name="plainText">Plain text result of decryption.</param>
        /// <param name="splitter">Separator character.</param>
        /// <returns>True if succeed; otherwise, false.</returns>
        public bool TryDecrypt(string cipherText, out string plainText, char splitter = DEFAULT_IV_SPLITTER)
        {
            bool result = false;
            plainText = null;

            try
            {
                plainText = Decrypt(cipherText, splitter);
                result = true;
            }
            catch (Exception exception)
            {
                //LogManager.Log(
                //    logLevel: LogLevel.Error,
                //    exception: exception,
                //    message: string.Format("An exception raised during decryption. \ncipherText = {0} \nsplitter = {1} \nAES Key = [Not logged! It should kept secret.]",
                //                            cipherText, splitter),
                //    methodName: System.Reflection.MethodBase.GetCurrentMethod().Name,
                //    className: this.GetType().Name
                //);
            }

            return result;
        }
    }
}
