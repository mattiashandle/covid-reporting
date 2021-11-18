﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karolinska.Core.Entities
{
    public class StockBalanceReport
    {
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public Guid SupplierId { get; set; }

        public int NumberOfVials { get; set; }

        public int NumberOfDosages { get; set; }

        public Guid HealthcareProviderId { get; set; }
    }
}