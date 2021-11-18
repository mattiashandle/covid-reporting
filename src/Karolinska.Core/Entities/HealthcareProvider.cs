using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karolinska.Core.Entities
{
    public class HealthcareProvider
    {
        [Key]
        public Guid Id { get; set; }

        public ICollection<OrderReport> OrderReports { get; set; } = new HashSet<OrderReport>();

        public ICollection<CapacityReport> CapacityReports { get; set; } = new HashSet<CapacityReport>();

        public ICollection<StockBalanceReport> StockBalanceReports { get; set;} = new HashSet<StockBalanceReport>();

        public ICollection<ReceiptReport> ReceiptReports { get; set; } = new HashSet<ReceiptReport>();

        public ICollection<ExpenditureReport> ExpenditureReports { get; set; } = new HashSet<ExpenditureReport>();

        public string Name { get; set; }
    }
}
