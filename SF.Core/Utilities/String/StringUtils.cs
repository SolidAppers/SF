using System;
using System.Linq;

namespace SF.Core.Utilities.String
{
    public static class StringUtils
    {
        /// <summary>
        /// belirtilen uzunlukta random string üretir
        /// okunması zor ve karışabilecek karakterler içermez
        /// </summary>
        /// <param name="length"></param>
        /// <param name="isAphaNumeric">sayılar eklenecekmi</param>
        /// <returns></returns>
        public static string RandomString(int length, bool isAphaNumeric=false)
        {
            string set = "ABCDEFGHJKLMNPQRSTUXYZ";
            if (isAphaNumeric)
            {
                set += "0123456789";
            }

            var random = new Random();
            return new string(Enumerable.Repeat(set, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
