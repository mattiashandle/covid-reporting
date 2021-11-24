using AutoMapper;
using Karolinska.Application.Dtos;
using Karolinska.Core.Entities;
using Karolinska.Core.Interfaces;
using Karolinska.Infrastructure.Contexts;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karolinska.Application.Features.Commands
{
    public class UpdateCapacityReportCommand
    {
        public UpdateCapacityReportCommand(Guid capacityReportId, JsonPatchDocument<CapacityReportDto> capacityReport)
        {
            CapacityReportId = capacityReportId;
            CapacityReport = capacityReport;
        }

        public Guid CapacityReportId { get; }

        public JsonPatchDocument<CapacityReportDto> CapacityReport { get; }
    }

    public class UpdateCapacityReportCommandHandler : ICommandHandler<UpdateCapacityReportCommand, CapacityReportDto>
    {
        private readonly KarolinskaContext _context;
        private readonly IMapper _mapper;

        public UpdateCapacityReportCommandHandler(KarolinskaContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<CapacityReportDto?> HandleAsync(UpdateCapacityReportCommand command)
        {
            var capacityReport = await _context.CapacityReports.FirstOrDefaultAsync(e => e.Id.Equals(command.CapacityReportId));

            if (capacityReport == null)
            {
                return null;
            }

            var capacityReportDto = _mapper.Map<CapacityReportDto>(capacityReport);

            command.CapacityReport.ApplyTo(capacityReportDto);

            _mapper.Map(capacityReportDto, capacityReport);

            await _context.SaveChangesAsync();

            return _mapper.Map<CapacityReportDto>(capacityReport);
        }
    }
}
