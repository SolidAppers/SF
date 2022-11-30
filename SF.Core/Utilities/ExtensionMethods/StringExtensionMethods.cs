using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SF.Core.Utilities.ExtensionMethods
{
    public static class StringExtensionMethods
    {
        /// <summary>
        /// new line \n karakterini <br/> tagına çevirir
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Nl2Br(this string str)
        {
            return str.Replace("\n", "<br/>");
        }


        public static string ToTitleCase(this string str)
        {
            TextInfo textInfo = new CultureInfo("tr-TR", false).TextInfo;
            return textInfo.ToTitleCase(str);
        }


        public static bool IsValidEmail(this string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// öçşğüıİÖÇŞĞÜ karaklerleri içerip içermediğini sorgular
        /// </summary>
        /// <param name="sStr"></param>
        /// <returns></returns>
        public static bool TurkceKarakterVarMi(this string sStr)
        {
            const string sTurkceKarakterler = "öçşğüıİÖÇŞĞÜ";
            for (var i = 0; i < sTurkceKarakterler.Length; i++)
            {
                if (sStr.Contains(sTurkceKarakterler[i].ToString()))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// öçşğüı harflerini ocsgui harflerine dönüşür, trim eder
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string TurkceKarakterleriKaldir(this string str)
        {
            str = str.Trim().Replace('ö', 'o').Replace('ü', 'u').Replace('ğ', 'g').Replace('ş', 's').Replace('ı', 'i').Replace('ç', 'c');
            str = str.Replace('Ö', 'O').Replace('Ü', 'U').Replace('Ğ', 'G').Replace('Ş', 'S').Replace('İ', 'I').Replace('Ç', 'C');

            return str;
        }


        public static string RemoveNonAlphaNumeric(this string str)
        {
            char[] arr = str.ToCharArray();
            arr = Array.FindAll(arr, (c => (char.IsLetterOrDigit(c))));
            return new string(arr);
        }



        public static string Mask(this string str, string maskChar = "***")
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            return $"{str.First()}{maskChar}{str.Last()}";
        }


        /// <summary>
        /// MD5 Hash e çevirir
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToMd5Hash(this string input)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.ASCII.GetBytes(input);
                return Convert.ToHexString(md5.ComputeHash(inputBytes));
         
            }
        }

 

        public static string Base64Encode(this string plainText)
        {

            return System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(plainText));
        }
        public static string Base64Decode(this string encoded)
        {

            return System.Text.ASCIIEncoding.ASCII.GetString(System.Convert.FromBase64String(encoded));


        }

        public static string Encrypt(this string message, string privateKey = "")
        {
            var passPhrase = ConfigurationManager.AppSettings["PassPhrase"];
            if (!string.IsNullOrEmpty(privateKey))
            {
                passPhrase = privateKey;
            }
            else
            {
                if (string.IsNullOrEmpty(passPhrase))
                {
                    passPhrase = DateTime.Now.ToShortDateString();
                }
            }

            byte[] iv = new byte[16];
            byte[] array;
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(passPhrase);
                aes.IV = iv;
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(message);
                        }
                        array = memoryStream.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(array).Replace('+', '-').Replace('/', '_').Replace('=', ',');
        }
        public static string Decrypt(this string message, string privateKey = "")
        {
            message = message.Replace('-', '+').Replace('_', '/').Replace(',', '=');
            var passPhrase = ConfigurationManager.AppSettings["PassPhrase"];
            if (!string.IsNullOrEmpty(privateKey))
            {
                passPhrase = privateKey;
            }
            else
            {
                if (string.IsNullOrEmpty(passPhrase))
                {
                    passPhrase = DateTime.Now.ToShortDateString();
                }
            }
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(message);
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(passPhrase);//I have already defined "Key" in the above EncryptString function
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

    }
}
