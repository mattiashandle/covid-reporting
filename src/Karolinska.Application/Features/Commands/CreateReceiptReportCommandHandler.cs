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
    public class CreateReceiptReportCommand
    {
        public DateTime? DeliveryDate { get; set; }

        public DateTime ExpectedDeliveryDate { get; set; }

        public int NumberOfVials { get; set; }

        public string GLNReceiver { get; set; }

        public Guid SupplierId { get; set; }

        public Guid HealthcareProviderId { get; set; }
    }

    public class CreateReceiptReportCommandHandler : ICommandHandler<CreateReceiptReportCommand, ReceiptReportDto?>
    {
        private readonly KarolinskaContext _context;
        private readonly IMapper _mapper;

        public CreateReceiptReportCommandHandler(KarolinskaContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ReceiptReportDto?> HandleAsync(CreateReceiptReportCommand command)
        {
            var healthcareProvider = await _context.HealthcareProviders.AnyAsync(e => e.Id.Equals(command.HealthcareProviderId));

            if (!healthcareProvider)
            {
                return null;
            }

            var receiptReport = new ReceiptReport
            {
                Id = Guid.NewGuid(),
                HealthcareProviderId = command.HealthcareProviderId,
                InsertDate = DateTime.UtcNow,
                DeliveryDate = command.DeliveryDate,
                ExpectedDeliveryDate = command.ExpectedDeliveryDate,
                GLNReceiver = command.GLNReceiver,
                NumberOfVials = command.NumberOfVials,
                SupplierId = command.SupplierId
            };

            await _context.ReceiptReports.AddAsync(receiptReport);

            await _context.SaveChangesAsync();

            return _mapper.Map<ReceiptReportDto>(receiptReport);
        }
    }
}
