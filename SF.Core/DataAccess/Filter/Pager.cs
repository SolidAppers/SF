using System.Collections.Generic;

namespace SF.Core.DataAccess.Filter
{
    public class Pager
    {
        public int? Start { get; set; }
        public int? Length { get; set; }

        public List<Order> OrderList { get; set; }

        //public Ara(int? basla, int? uzunluk)
        //{
        //    if (uzunluk.HasValue)
        //    {
        //        Uzunluk = uzunluk.Value > 0 ? uzunluk : null;
        //    }
        //}
    }
}
