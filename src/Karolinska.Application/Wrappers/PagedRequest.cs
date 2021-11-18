using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karolinska.Application.Wrappers
{
    public abstract class PagedRequest
    {
        //[Range(1, int.MaxValue / 1000)]
        public abstract int PageNumber { get; set; }

        //[Range(1, 1000)]
        public abstract int PageSize { get; set; }
    }
}
