using System;

namespace SF.Core.Extensions
{
    public class NotificationException : Exception
    {
        public NotificationException(string msg, int code = 0)
            : base(msg)
        {
        
            base.Source = code.ToString();
        }

        public NotificationException(string msg, Exception innerException, int code = 0)
            : base(msg, innerException)
        {
            base.Source = code.ToString();
        }
    }
}
