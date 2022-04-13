using System;
using System.ComponentModel;

namespace SF.Core.Utilities.ExtensionMethods
{
    public static class EnumExtensions
    {

        /// <summary>
        /// Enum Description (DescriptionAttribute) özellğini okur
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDescription(this Enum value)
        {
            var da = (DescriptionAttribute[])(value.GetType().GetField(value.ToString())).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return da.Length > 0 ? da[0].Description : value.ToString();
        }
    }
}
