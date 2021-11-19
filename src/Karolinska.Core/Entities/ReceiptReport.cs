using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karolinska.Core.Entities
{
    public class ReceiptReport
    {
        public Guid Id { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public DateTime ExpectedDeliveryDate { get; set; }

        public int NumberOfVials { get; set; }

        public string GLNReceiver { get; set; }

        public Guid SupplierId { get; set; }

        public Guid HealthcareProviderId { get; set; }

        public DateTime InsertDate { get; set; }
    }
}
