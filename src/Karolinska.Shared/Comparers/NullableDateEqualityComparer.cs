using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karolinska.Shared.Comparers
{
    public class NullableDateEqualityComparer : IEqualityComparer<DateTime?>
    {
        private NullableDateEqualityComparer() { }

        public bool Equals(DateTime? x, DateTime? y)
        {
            var date1 = x.HasValue ? x.Value.ToUniversalTime() : DateTime.MinValue;
            var date2 = y.HasValue ? y.Value.ToUniversalTime() : DateTime.MinValue;

            return DateTime.Compare(date1.Date, date2.Date) == 0;
        }

        public int GetHashCode(DateTime? obj)
        {
            return obj.GetHashCode();
        }

        public static readonly IEqualityComparer<DateTime?> Default = new NullableDateEqualityComparer();
    }
}
