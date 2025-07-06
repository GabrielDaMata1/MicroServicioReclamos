using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Models.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance
{
    /// <summary>
    /// Clase persistance que representa el contexto de base de datos en PostgreSQL del Microservicio Reclamos.
    /// </summary>
    public class SubastaDbContext : DbContext
    {
        public SubastaDbContext(DbContextOptions<SubastaDbContext> options) : base(options) { }

        /// <summary>
        /// Atributo que corresponde a la tabla de Reclamo en la base de datos PostgreSQL.
        /// </summary>
        public DbSet<ReclamoPostgreSQL> Reclamo { get; set; }
        /// <summary>
        /// Atributo que corresponde a la tabla de ResolucionReclamo en la base de datos PostgreSQL.
        /// </summary>
        public DbSet<ResolucionReclamoPostgreSQL> ResolucionReclamo { get; set; }
        /// <summary>
        /// Atributo que corresponde a la tabla de ReclamoPremio en la base de datos PostgreSQL.
        /// </summary>
        public DbSet<ReclamoPremioPostgreSQL> ReclamoPremio { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReclamoPostgreSQL>()
                .HasIndex(u => u.Id)
                .IsUnique();

            modelBuilder.Entity<ResolucionReclamoPostgreSQL>()
                .HasIndex(u => u.Id)
                .IsUnique();

            modelBuilder.Entity<ReclamoPremioPostgreSQL>()
                .HasIndex(u => u.Id)
                .IsUnique();

            base.OnModelCreating(modelBuilder);

        }
    }
}
