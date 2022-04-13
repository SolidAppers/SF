using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SF.Core.Entities.Concrete
{
    [Table("AuditLogDetail")]
    public class AuditLogDetail
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }


        public long AuditLogId { get; set; }

        [ForeignKey("AuditLogId")]
        public virtual AuditLog AuditLog { get; set; }


        [DisplayName("Kolon Adı"), StringLength(50)]
        public string KolonAdi { get; set; }

        [DisplayName("Eski Veri")]
        public string EskiVeri { get; set; }

        [DisplayName("Yeni Veri")]
        public string YeniVeri { get; set; }



    }
}
