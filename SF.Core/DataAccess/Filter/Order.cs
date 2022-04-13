using SF.Core.Enums;

namespace SF.Core.DataAccess.Filter
{
    public class Order
    {
        public Order()
        {
            OrderBy = EnumOrderBy.Asc;
        }

        public string ColName { get; set; }
        public EnumOrderBy OrderBy { get; set; }
    }
}
