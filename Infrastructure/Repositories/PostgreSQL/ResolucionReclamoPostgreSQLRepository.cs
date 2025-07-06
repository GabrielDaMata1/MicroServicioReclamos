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
    /// Clase repository que implementa las operaciones que se pueden realizar sobre la resolución de reclamos almacenados en PostreSQL.
    /// </summary>
    public class ResolucionReclamoPostgreSQLRepository : IResolucionReclamoPostgreSQLRepository
    {
        /// <summary>
        /// Atributo que corresponde al contexto de la base de datos del Microservicio Reclamos en PostgreSQL.
        /// </summary>
        private readonly SubastaDbContext _dbContext;

        public ResolucionReclamoPostgreSQLRepository(SubastaDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// Metodo que se encarga de registrar la resolucion de un reclamo en la base de datos en PostgreSQL.
        /// </summary>
        /// <param name="resolucionReclamo">Parametro que de tipo ResolucionReclamo que contiene el detalle  del objeto.</param>
        /// <returns>Retorna el GUID del objeto si la operación fue exitosa</returns>
        public async Task<Guid> RegistrarResolucionReclamo(ResolucionReclamo resolucionReclamo)
        {

            var resolucionReclamoDB = resolucionReclamo.ToPostgres();
            await _dbContext.ResolucionReclamo.AddAsync(resolucionReclamoDB);
            await _dbContext.SaveChangesAsync();
            return resolucionReclamoDB.Id;
        }
    }
}
