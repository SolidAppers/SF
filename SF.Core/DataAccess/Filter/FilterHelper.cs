using System.Linq;
using SF.Core.Utilities.ExtensionMethods;

namespace SF.Core.DataAccess.Filter
{
    public class FilterHelper<T>
    {
        public int TotalRecords { get; set; }

        public IQueryable<T> PageBy(IQueryable<T> result, Pager filtre = null)
        {
            TotalRecords = result.Count();
            result = OrderBy(result, filtre);
            return result;
        }

        public int KayitSayisiGetir(IQueryable<T> result)
        {
            TotalRecords = result.Count();
            return TotalRecords;
        }


        public IQueryable<T> OrderBy(IQueryable<T> result, Pager filtre = null)
        {
            if (filtre != null)
            {
                if (filtre.OrderList != null)
                {
                    var i = 0;
                    foreach (var siralama in filtre.OrderList)
                    {
                        result = i == 0 ? result.OrderBy(siralama.ColName, siralama.OrderBy) : result.ThenBy(siralama.ColName, siralama.OrderBy);
                        i++;
                    }
                }
                if (filtre.Start != null)
                {
                    result = result.Skip(filtre.Start.Value);
                }

                if (filtre.Length > 0)
                {
                    result = result.Take(filtre.Length.Value);
                }
            }


            return result;
        }
    }
}
