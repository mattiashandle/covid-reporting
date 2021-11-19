using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karolinska.Application.Dtos
{
    public class StockBalanceReportDto
    {
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public Guid SupplierId { get; set; }

        public int NumberOfVials { get; set; }

        public int NumberOfDosages { get; set; }

        public DateTime InsertDate { get; set; }
    }
}
