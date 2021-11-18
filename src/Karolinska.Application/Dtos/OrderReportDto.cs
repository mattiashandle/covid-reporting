using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karolinska.Application.Dtos
{
    public class OrderReportDto
    {
        public OrderReportDto()
        {

        }

        public Guid Id { get; set; }

        public DateTime? OrderDate { get; set; }

        public DateTime? RequestedDeliveryDate { get; set; }

        public int Quantity { get; set; }

        public string GLNReceiver { get; set; }
    }
}
