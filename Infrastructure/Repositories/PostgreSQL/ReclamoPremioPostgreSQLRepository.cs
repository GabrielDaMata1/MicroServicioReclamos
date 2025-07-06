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
    /// <summary>
    /// Clase repository que implementa las operaciones que se pueden realizar sobre los reclamos de premios almacenados en PostreSQL.
    /// </summary>
    public class ReclamoPremioPostgreSQLRepository : IReclamoPremioPostgreSQLRepository
    {
        /// <summary>
        /// Atributo que corresponde al contexto de la base de datos del Microservicio Reclamos en PostgreSQL.
        /// </summary>
        private readonly SubastaDbContext _dbContext;

        public ReclamoPremioPostgreSQLRepository(SubastaDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// Metodo que se encarga de registrar un reclamo de un premio en la base de datos en PostgreSQL.
        /// </summary>
        /// <param name="reclamoPremio">Parametro que de tipo ReclamoPremio que contiene el detalle  del objeto.</param>
        /// <returns>Retorna el GUID del objeto si la operación fue exitosa</returns>
        public async Task<Guid> RegistrarReclamoPremioAsync(ReclamoPremio reclamoPremio)
        {
            var reclamoPremioBD = reclamoPremio.ToPostgres();
            await _dbContext.ReclamoPremio.AddAsync(reclamoPremioBD);
            await _dbContext.SaveChangesAsync();
            return reclamoPremioBD.Id;
        }
    }
}
