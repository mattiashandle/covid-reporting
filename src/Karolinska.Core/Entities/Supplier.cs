﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karolinska.Core.Entities
{
    public class Supplier
    {
        public Supplier(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        [Key]
        public Guid Id { get; private set; }

        public string Name { get; set; }
    }
}
