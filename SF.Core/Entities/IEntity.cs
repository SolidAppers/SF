using System;

namespace SF.Core.Entities
{
    public interface IEntity
    {
        int? DegistirenId { get; set; }
        DateTime? DegistirmeTarihi { get; set; }
        int EkleyenId { get; set; }
        DateTime EklemeTarihi { get; set; }
        int RowVersion { get; set; }
    }
}
