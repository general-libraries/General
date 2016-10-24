using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using HashidsNet;

namespace Common.Helpers
{
    public abstract class HashHelper
    {
        private const int MIN_INT_HASH_LEN = 8;
        public static readonly string DefaultSalt = Manager.Instance.GlobalSetting.GeneralSalt;

        public static object Decode(string value, Type type, string salt = null)
        {
            if (value == null) { return type.IsValueType ? Activator.CreateInstance(type) : null; }

            if (type == typeof(long))
            {
                return DecodeLong(value, salt);
            }
            if (type == typeof(string))
            {
                return DecodeString(value, salt);
            }
            if (type == typeof(int))
            {
                return DecodeInt(value, salt);
            }
            if (type == typeof(long[]))
            {
                return DecodeLongs(value, salt);
            }
            if (type == typeof(int[]))
            {
                return DecodeInts(value, salt);
            }

            throw new ArgumentException($"Decode to type '{type}' is not supported.", nameof(value));
        }

        public static T Decode<T>(string value, string salt = null)
            where T : IConvertible
        {
            return (T)Decode(value, typeof(T), salt);
        }

        public static int DecodeInt(string value, string salt = null)
        {
            int[] decoded = DecodeInts(value, salt);
            if (decoded == null || decoded.Length == 0) { return 0; }
            return decoded[0];
        }

        public static int[] DecodeInts(string value, string salt = null)
        {
            if (string.IsNullOrWhiteSpace(value)) { return new int[0]; }

            int[] decoded;

            try
            {
                var hashids = GenerateHashids(salt);
                decoded = hashids.Decode(value);
            }
            catch (Exception)
            {
                //TODO: log
                decoded = new int[0];
            }

            return decoded;
        }

        public static long DecodeLong(string value, string salt = null)
        {
            long[] decoded = DecodeLongs(value, salt);
            if (decoded == null || decoded.Length == 0) { return 0; }
            return decoded[0];
        }

        public static long[] DecodeLongs(string value, string salt = null)
        {
            if (string.IsNullOrWhiteSpace(value)) { return new long[0]; }

            long[] decoded;

            try
            {
                var hashids = GenerateHashids(salt);
                decoded = hashids.DecodeLong(value);
            }
            catch (Exception)
            {
                //TODO: log
                decoded = new long[0];
            }

            return decoded;
        }

        public static string DecodeString(string value, string salt = null)
        {
            if (string.IsNullOrEmpty(value)) { return string.Empty; }

            string decoded = string.Empty;

            try
            {
                var hashids = GenerateHashids(salt);
                var sequence = hashids.Decode(value);

                if (sequence != null && sequence.Length > 0)
                {
                    var strBuilder = new StringBuilder(sequence.Length, sequence.Length);
                    foreach (var i in sequence) { strBuilder.Append(Convert.ToChar(i)); }
                    decoded = strBuilder.ToString();
                }
            }
            catch
            {
                // TODO: log exception
            }

            return decoded;
        }

        public static string Encode(object value, string salt = null)
        {
            if (value == null) { return null; }

            if (value is long)
            {
                return EncodeLong((long)value);
            }
            if (value is string)
            {
                return EncodeString((string)value);
            }
            if (value is int)
            {
                return EncodeInt((int)value);
            }
            if (value is long[])
            {
                return EncodeLong((long[])value);
            }
            if (value is int[])
            {
                return EncodeInt((int[])value);
            }

            throw new ArgumentException($"type '{value.GetType()}' is not supported.", nameof(value));
        }

        public static string EncodeInt(int value, string salt = null)
        {
            return EncodeInt(new int[1] { value });
        }

        public static string EncodeInt(int[] values, string salt = null)
        {
            string hash;

            try
            {
                var hashids = GenerateHashids(salt);
                hash = hashids.Encode(values);
            }
            catch (Exception)
            {
                //TODO: log
                hash = string.Empty;
            }

            return hash;
        }

        public static string EncodeLong(long value, string salt = null)
        {
            return EncodeLong(new long[1] { value });
        }

        public static string EncodeLong(long[] values, string salt = null)
        {
            string hash;

            try
            {
                var hashids = GenerateHashids(salt);
                hash = hashids.EncodeLong(values);
            }
            catch (Exception)
            {
                //TODO: log
                hash = string.Empty;
            }

            return hash;
        }

        public static string EncodeString(string value, string salt = null)
        {
            if (string.IsNullOrEmpty(value)) { return string.Empty; }

            string encoded = string.Empty;

            try
            {
                var sequence = value.ToCharArray().Select(Convert.ToInt32).ToArray();

                if (sequence.Length > 0)
                {
                    var hashids = GenerateHashids(salt);
                    encoded = hashids.Encode(sequence);
                }
            }
            catch
            {
                // TODO:  log the error
            }

            return encoded;
        }

        public static string GetHash(string input)
        {
            HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();

            byte[] byteValue = Encoding.UTF8.GetBytes(input);

            byte[] byteHash = hashAlgorithm.ComputeHash(byteValue);

            return Convert.ToBase64String(byteHash);
        }

        private static Hashids GenerateHashids(string salt, int minHashLen = MIN_INT_HASH_LEN)
        {
            if (string.IsNullOrWhiteSpace(salt))
            {
                salt = DefaultSalt;
            }

            if (string.IsNullOrWhiteSpace(salt))
            {
                salt = DateTime.MinValue.ToLongDateString();
            }

            return new Hashids(salt, minHashLen);
        }
    }
}
