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
        public HealthcareProviderDto(string name)
        {
            if(string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

            Name = name;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
