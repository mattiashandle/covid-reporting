using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karolinska.Core.Entities
{
    public class OrderReport
    {
        public OrderReport()
        {

        }

        public Guid Id { get; set; }

        public DateTime? OrderDate { get; set; }

        public DateTime? RequestedDeliveryDate { get; set; }

        public int Quantity { get; set; }

        public string GLNReceiver { get; set; }

        public Guid HealthcareProviderId { get; set; }
    }
}
