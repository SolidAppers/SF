using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SF.Core.Entities.Concrete
{
    public class AuditLogDetailMap : IEntityTypeConfiguration<AuditLogDetail>
    {


        public void Configure(EntityTypeBuilder<AuditLogDetail> builder)
        {
            builder.HasOne(t => t.AuditLog)
                .WithMany(t => t.AuditLogDetail)
                .HasForeignKey(t => t.AuditLogId).
                OnDelete(DeleteBehavior.Restrict);


            /* eski yöntem
             * 
             *    this.HasRequired(t => t.AuditLog)
               .WithMany(t => t.AuditLogDetail)
               .HasForeignKey(t => t.AuditLogId)
               .WillCascadeOnDelete(false);

             */

        }


    }
}
