using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ipsilon_1A.Models;

namespace Ipsilon_1A.Data
{
    public class Ipsilon_1AContext : DbContext
    {
        public Ipsilon_1AContext (DbContextOptions<Ipsilon_1AContext> options)
            : base(options)
        {
        }

        public DbSet<Ipsilon_1A.Models.Paquete> Paquete { get; set; } = default!;
    }
}
