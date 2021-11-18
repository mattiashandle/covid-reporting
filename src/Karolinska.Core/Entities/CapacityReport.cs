using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karolinska.Core.Entities
{
    public class CapacityReport
    {
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public int NumberOfVials { get; set; }

        public Guid SupplierId { get; set; }

        public Guid HealthcareProviderId { get; set; }
    }
}
