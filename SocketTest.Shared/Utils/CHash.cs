using System;
using System.Security.Cryptography;
using System.Text;

namespace SocketTest.Shared.Utils
{
    public static class CHash
    {
        public class HashSha256
        {
            private static string Salt = "l23sdhfbiu aswiruhwi4h39@#$%284h 234 u234289  fk sj skdfdhfsldhf";

            public static string Get(string str)
            {
                SHA256Managed sHA256Managed = new SHA256Managed();
                StringBuilder stringBuilder = new StringBuilder();
                str += Salt;
                byte[] array = sHA256Managed.ComputeHash(Encoding.UTF8.GetBytes(str), 0, Encoding.UTF8.GetByteCount(str));
                byte[] array2 = array;
                foreach (byte b in array2)
                {
                    stringBuilder.Append(b.ToString("x2"));
                }

                return stringBuilder.ToString();
            }
        }

        public static string EncryptMD5(string Text)
        {
            byte[] array = MD5.Create().ComputeHash(Encoding.Default.GetBytes(Text));
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
            {
                stringBuilder.Append(array[i].ToString("x2"));
            }

            return stringBuilder.ToString();
        }

        public static string ComputeStringToSha256Hash(string plainText)
        {
            using SHA256 sHA = SHA256.Create();
            byte[] array = sHA.ComputeHash(Encoding.UTF8.GetBytes(plainText));
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
            {
                stringBuilder.Append(array[i].ToString("x2"));
            }

            return stringBuilder.ToString();
        }

        public static string CalculateMD5(byte[] file)
        {
            using MD5 mD = MD5.Create();
            if (file != null)
            {
                byte[] array = mD.ComputeHash(file);
                return BitConverter.ToString(array).Replace("-", "").ToLowerInvariant();
            }

            return "";
        }
    }
}
