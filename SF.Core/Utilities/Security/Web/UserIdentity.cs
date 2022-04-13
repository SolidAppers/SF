using SF.Core.Entities.Concrete;
using System;
using System.Security.Principal;

namespace SF.Core.Utilities.Security.Web
{
    [Serializable]
    public class UserIdentity : IIdentity
    {
        public string Name { get; set; }

        /// <summary>
        /// aktif profil id
        /// </summary>
        public int? UserId { get; set; }
               

        /// <summary>
        /// i�lem yap�lan ip adresi
        /// </summary>
        public string UserIp { get; set; }

        /// <summary>
        /// i�lem yap�lan adres
        /// </summary>
        public string RequestUrl { get; set; }



        /// <summary>
        /// IIdentity özellikleri
        /// </summary>
   
        public string AuthenticationType { get; set; }
        public bool IsAuthenticated { get; set; }

  
        /// <summary>
        /// Ek ta��nacak bilgiler 
        /// [Serializeable] olmal�d�r
        /// </summary>
        public YetkiKullanici UserData { get; set; }
    }
}