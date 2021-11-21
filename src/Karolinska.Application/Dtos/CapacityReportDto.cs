using System;

namespace Karolinska.Application.Dtos
{
    public class CapacityReportDto
    {
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public int NumberOfDoses { get; set; }

        public DateTime InsertDate { get; set; }
    }
}
