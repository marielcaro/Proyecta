using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using ProyetaV1.Entities;

namespace ProyetaV1.Context
{
    public class ProyectaContext : DbContext
    {
        public ProyectaContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)

        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; } = null!;

        public DbSet<Perfil> Perfiles { get; set; } = null!;

        public DbSet<Disciplina> Disciplinas { get; set; } = null!;

        public DbSet<AreaDeInteres> AreaDeIntereses { get; set; } = null!;

    }
}
