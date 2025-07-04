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
    public class ResolucionReclamoPostgreSQLRepository : IResolucionReclamoPostgreSQLRepository
    {
        private readonly SubastaDbContext _dbContext;

        public ResolucionReclamoPostgreSQLRepository(SubastaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> RegistrarResolucionReclamo(ResolucionReclamo resolucionReclamo)
        {

            var resolucionReclamoDB = resolucionReclamo.ToPostgres();
            await _dbContext.ResolucionReclamo.AddAsync(resolucionReclamoDB);
            await _dbContext.SaveChangesAsync();
            return resolucionReclamoDB.Id;
        }
    }
}
