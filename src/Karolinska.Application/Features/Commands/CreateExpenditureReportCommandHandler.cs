using AutoMapper;
using Karolinska.Application.Dtos;
using Karolinska.Core.Entities;
using Karolinska.Core.Interfaces;
using Karolinska.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karolinska.Application.Features.Commands
{
    public class CreateExpenditureReportCommand
    {
        public DateTime Date { get; set; }

        public Guid SupplierId { get; set; }

        public int NumberOfVials { get; set; }

        public Guid HealthcareProviderId { get; set; }
    }

    public class CreateExpenditureReportCommandHandler : ICommandHandler<CreateExpenditureReportCommand, ExpenditureReportDto?>
    {
        private readonly KarolinskaContext _context;
        private readonly IMapper _mapper;

        public CreateExpenditureReportCommandHandler(KarolinskaContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ExpenditureReportDto?> HandleAsync(CreateExpenditureReportCommand command)
        {
            var providerExists = _context.HealthcareProviders.Any(e => e.Id.Equals(command.HealthcareProviderId));

            if (!providerExists)
            {
                return null;
            }

            var expenditureReport = new ExpenditureReport
            {
                HealthcareProviderId = command.HealthcareProviderId,
                Id = Guid.NewGuid(),
                Date = command.Date,
                InsertDate = DateTime.UtcNow,
                NumberOfVials = command.NumberOfVials,
                SupplierId = command.SupplierId
            };

            await _context.ExpenditureReports.AddAsync(expenditureReport);

            await _context.SaveChangesAsync();

            return _mapper.Map<ExpenditureReportDto>(expenditureReport);
        }
    }
}
