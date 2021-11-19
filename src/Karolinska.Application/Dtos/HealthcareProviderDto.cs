using Karolinska.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karolinska.Application.Dtos
{
    public class HealthcareProviderDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        //public List<OrderReportDto> OrderReports { get; set; }

        //public List<CapacityReportDto> CapacityReports { get; set; }

        //public List<StockBalanceReportDto> StockBalanceReports { get; set; }

        //public List<ReceiptReportDto> ReceiptReports { get; set; }
    }
}
