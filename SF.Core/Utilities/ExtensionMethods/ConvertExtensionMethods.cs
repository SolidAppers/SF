using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using SF.Core.Utilities.ExtensionMethods;
using SF.Core.Utilities.Parsers;

namespace SF.Core.Utilities.ExtensionMethods
{
    public static class ConvertExtensionMethods
    {
        public static int ToInt32(this object obj)
        {
            return Convert.ToInt32(obj);
        }

        public static double ToDouble(this object obj)
        {
            return Convert.ToDouble(obj);
        }

        public static decimal ToDecimal(this object obj)
        {
            return Convert.ToDecimal(obj);
        }

        public static bool ToBoolean(this object obj)
        {
            return Convert.ToBoolean(obj);
        }

        public static long ToLong(this object obj)
        {
            return Convert.ToInt64(obj);
        }

        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static List<T> ToList<T>(this string value, char split = ',')
        {
            if (string.IsNullOrEmpty(value))
                return null;

            return (from veri in value.Split(split) where !string.IsNullOrEmpty(veri) select (T)Convert.ChangeType(veri, typeof(T))).ToList();
        }

        public static string ToString(this List<int> value, char split = ',')
        {
            var birlestirilenString = "";
            foreach (var deger in value)
            {
                birlestirilenString += deger + ",";
            }

            if (birlestirilenString.Length > 0)
                birlestirilenString = birlestirilenString.Remove(birlestirilenString.Length - 1, 1);

            return birlestirilenString;
        }


        /// <summary>
        /// newtonsoft ile objeyi json string yapar, selfloop kapalı, null ignorant
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {



            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                StringEscapeHandling = StringEscapeHandling.EscapeHtml
            };

            return JsonConvert.SerializeObject(obj, Formatting.None, jsonSerializerSettings);

        }





        public static object Clone(this object obj)
        {
            return ClassConverter<object, object>.Convert(obj);

        }

        public static T Clone<T>(this object obj)
        {
            return (T)ClassConverter<object, object>.Convert(obj);
        }

        /// <summary>
        /// objeyi key=val&amp;key1=val1... formatına çevirir
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToQueryString(this object obj)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());

            return string.Join("&", properties.ToArray());
        }



        ///// <summary>
        ///// upload edilen dosyayı byte array haline getirir
        ///// </summary>
        ///// <param name="httpPostedFileBase"></param>
        ///// <returns></returns>
        //public static byte[] ToByteArray(this HttpPostedFileBase httpPostedFileBase)
        //{
        //    var ms = new MemoryStream();
        //    httpPostedFileBase.InputStream.CopyTo(ms);

        //    return ms.ToArray();
        //}



        /// <summary>
        /// Para formatında gösterim
        /// </summary>
        /// <param name="para"></param>
        /// <param name="simge"></param>
        /// <returns></returns>
        public static string ToMoney(this decimal para, string simge = "")
        {
            return simge != "" ? $"{para:N2} {HttpUtility.HtmlDecode(simge)}" : para.ToString("N2");
        }

        /// <summary>
        /// Para formatında gösterim
        /// </summary>
        /// <param name="para"></param>
        /// <param name="simge"></param>
        /// <returns></returns>
        public static string ToMoney(this decimal? para, string simge = "")
        {

            if (para.HasValue)
            {
                return simge != "" ? $"{para.Value:N2} {HttpUtility.HtmlDecode(simge)}" : para.Value.ToString("N2");
            }

            return "";

        }

    }
}
