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
    public class ReclamoPremioPostgreSQLRepository : IReclamoPremioPostgreSQLRepository
    {
        private readonly SubastaDbContext _dbContext;

        public ReclamoPremioPostgreSQLRepository(SubastaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> RegistrarReclamoPremioAsync(ReclamoPremio reclamoPremio)
        {
            var reclamoPremioBD = reclamoPremio.ToPostgres();
            await _dbContext.ReclamoPremio.AddAsync(reclamoPremioBD);
            await _dbContext.SaveChangesAsync();
            return reclamoPremioBD.Id;
        }
    }
}
