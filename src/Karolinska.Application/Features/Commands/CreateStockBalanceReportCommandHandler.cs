using AutoMapper;
using Karolinska.Application.Dtos;
using Karolinska.Core.Entities;
using Karolinska.Core.Interfaces;
using Karolinska.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karolinska.Application.Features.Commands
{
    public class CreateStockBalanceReportCommand
    {
        public DateTime Date { get; set; }

        public Guid SupplierId { get; set; }

        public int NumberOfVials { get; set; }

        public int NumberOfDosages { get; set; }

        public Guid HealthcareProviderId { get; set; }
    }

    public class CreateStockBalanceReportCommandHandler : ICommandHandler<CreateStockBalanceReportCommand, StockBalanceReportDto?>
    {
        private readonly KarolinskaContext _context;
        private readonly IMapper _mapper;

        public CreateStockBalanceReportCommandHandler(KarolinskaContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<StockBalanceReportDto?> HandleAsync(CreateStockBalanceReportCommand command)
        {
            var healthcareProvider = await _context.HealthcareProviders.AnyAsync(e => e.Id.Equals(command.HealthcareProviderId));

            if (!healthcareProvider)
            {
                return null;
            }

            var stockBalanceReport = new StockBalanceReport
            {
                Id = Guid.NewGuid(),
                Date = command.Date,
                HealthcareProviderId = command.HealthcareProviderId,
                InsertDate = DateTime.UtcNow,
                NumberOfVials = command.NumberOfVials,
                SupplierId = command.SupplierId
            };

            await _context.StockBalanceReports.AddAsync(stockBalanceReport);

            await _context.SaveChangesAsync();

            return _mapper.Map<StockBalanceReportDto>(stockBalanceReport);
        }
    }
}
