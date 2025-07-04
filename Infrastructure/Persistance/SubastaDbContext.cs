using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Models.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance
{
    public class SubastaDbContext : DbContext
    {
        public SubastaDbContext(DbContextOptions<SubastaDbContext> options) : base(options) { }


        public DbSet<ReclamoPostgreSQL> Reclamo { get; set; }
        public DbSet<ResolucionReclamoPostgreSQL> ResolucionReclamo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReclamoPostgreSQL>()
                .HasIndex(u => u.Id)
                .IsUnique();

            modelBuilder.Entity<ResolucionReclamoPostgreSQL>()
                .HasIndex(u => u.Id)
                .IsUnique();

            base.OnModelCreating(modelBuilder);

        }
    }
}
