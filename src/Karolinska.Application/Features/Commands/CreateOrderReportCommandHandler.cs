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
    public class CreateOrderReportCommand
    {
        public DateTime? OrderDate { get; set; }

        public DateTime? RequestedDeliveryDate { get; set; }

        public int Quantity { get; set; }

        public string GLNReceiver { get; set; }

        public Guid HealthcareProviderId { get; set; }
    }

    public class CreateOrderReportCommandHandler : ICommandHandler<CreateOrderReportCommand, OrderReportDto?>
    {
        private readonly KarolinskaContext _context;
        private readonly IMapper _mapper;

        public CreateOrderReportCommandHandler(KarolinskaContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<OrderReportDto?> HandleAsync(CreateOrderReportCommand command)
        {
            var providerExists = _context.HealthcareProviders.Any(e => e.Id.Equals(command.HealthcareProviderId));

            if (!providerExists)
            {
                return null;
            }

            var orderReport = new OrderReport
            {
                GLNReceiver = command.GLNReceiver,
                HealthcareProviderId = command.HealthcareProviderId,
                Id = Guid.NewGuid(),
                OrderDate = command.OrderDate,
                Quantity = command.Quantity,
                RequestedDeliveryDate = command.RequestedDeliveryDate,
                InsertDate = DateTime.UtcNow
            };

            await _context.OrderReports.AddAsync(orderReport);

            await _context.SaveChangesAsync();

            return _mapper.Map<OrderReportDto>(orderReport);
        }
    }
}
