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
    public class UpdateOrderReportCommand
    {
        public UpdateOrderReportCommand(Guid orderReportId, JsonPatchDocument<OrderReportDto> orderReport)
        {
            OrderReportId = orderReportId;
            OrderReport = orderReport;
        }

        public Guid OrderReportId { get; }

        public JsonPatchDocument<OrderReportDto> OrderReport { get; }
    }

    public class UpdateOrderReportCommandHandler : ICommandHandler<UpdateOrderReportCommand, OrderReportDto>
    {
        private readonly KarolinskaContext _context;
        private readonly IMapper _mapper;

        public UpdateOrderReportCommandHandler(KarolinskaContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<OrderReportDto?> HandleAsync(UpdateOrderReportCommand command)
        {
            var orderReport = await _context.OrderReports.FirstOrDefaultAsync(e => e.Id.Equals(command.OrderReportId));

            if (orderReport == null)
            {
                return null;
            }

            var orderReportDto = _mapper.Map<OrderReportDto>(orderReport);

            command.OrderReport.ApplyTo(orderReportDto);

            _mapper.Map(orderReportDto, orderReport);

            await _context.SaveChangesAsync();

            return _mapper.Map<OrderReportDto>(orderReport);
        }
    }
}
