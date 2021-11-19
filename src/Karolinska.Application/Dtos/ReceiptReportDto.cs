using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karolinska.Application.Dtos
{
    public class ReceiptReportDto
    {
        public Guid Id { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public DateTime ExpectedDeliveryDate { get; set; }

        public int NumberOfVials { get; set; }

        public string GLNReceiver { get; set; }

        public Guid SupplierId { get; set; }

        public DateTime InsertDate { get; set; }
    }
}
