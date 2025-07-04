using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Mappers;
using Infrastructure.Persistance;

namespace Infrastructure.Repositories.PostgreSQL
{
    public class ReclamoPostgreSQLRepository : IReclamoPostgreSQLRepository
    {
        private readonly SubastaDbContext _dbContext;

        public ReclamoPostgreSQLRepository(SubastaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> RegistrarReclamoAsync(Reclamo reclamo)
        {
            var reclamoBD = reclamo.ToPostgres();
            await _dbContext.Reclamo.AddAsync(reclamoBD);
            await _dbContext.SaveChangesAsync();
            return reclamoBD.Id;
        }
    }
}
