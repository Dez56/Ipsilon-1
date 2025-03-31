using Ipsilon_1A.Models;
using Microsoft.EntityFrameworkCore;

namespace Ipsilon_1A.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Paquete> Paquetes { get; set; }
    }
}
