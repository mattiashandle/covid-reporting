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
    public class CreateCapacityReportCommand
    {
        public Guid HealthcareProviderId { get; set; }

        public DateTime Date { get; set; }

        public int NumberOfDoses { get; set; }
    }

    public class CreateCapacityReportCommandHandler : ICommandHandler<CreateCapacityReportCommand, CapacityReportDto?>
    {
        private readonly KarolinskaContext _context;
        private readonly IMapper _mapper;

        public CreateCapacityReportCommandHandler(KarolinskaContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<CapacityReportDto?> HandleAsync(CreateCapacityReportCommand command)
        {
            var healthcareProvider = await _context.HealthcareProviders.AnyAsync(e => e.Id.Equals(command.HealthcareProviderId));

            if (!healthcareProvider)
            {
                return null;
            }

            var capacityReport = new CapacityReport
            {
                Id = Guid.NewGuid(),
                Date = command.Date,
                HealthcareProviderId = command.HealthcareProviderId,
                InsertDate = DateTime.UtcNow,
                NumberOfDoses = command.NumberOfDoses
            };

            await _context.CapacityReports.AddAsync(capacityReport);

            await _context.SaveChangesAsync();

            return _mapper.Map<CapacityReportDto>(capacityReport);
        }
    }
}
