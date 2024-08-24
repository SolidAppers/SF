using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SF.Core.Entities.Concrete
{
    [Table("AuditLog")]
    public class AuditLog
    {
        [Key]
        public long Id { get; set; }

        [Required, DisplayName("Kayıt Tipi"), StringLength(1, MinimumLength = 1)]
        public string Islem { get; set; }

        [Required, DisplayName("Log Zamanı")]
        public System.DateTime EklemeTarihi { get; set; }

        [Required, DisplayName("Tablo Adı"), StringLength(50)]
        public string TabloAdi { get; set; }

        public long KayitId { get; set; }


        [Required, DisplayName("Kullanıcı")]
        public int KullaniciId { get; set; }

        [DisplayName("IP Adres"), StringLength(40)]
        public string Ip { get; set; }


        [DisplayName("Vekil")]
        public int? VekilId { get; set; }


        public virtual ICollection<AuditLogDetail> AuditLogDetail { get; set; }
    }



}
