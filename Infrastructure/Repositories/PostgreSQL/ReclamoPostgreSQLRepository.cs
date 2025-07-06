using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Mappers;
using Infrastructure.Persistance;

namespace Infrastructure.Repositories.PostgreSQL
{
    /// <summary>
    /// Clase repository que implementa las operaciones que se pueden realizar sobre los reclamos almacenados en PostreSQL.
    /// </summary>
    public class ReclamoPostgreSQLRepository : IReclamoPostgreSQLRepository
    {
        /// <summary>
        /// Atributo que corresponde al contexto de la base de datos del Microservicio Reclamos en PostgreSQL.
        /// </summary>
        private readonly SubastaDbContext _dbContext;

        public ReclamoPostgreSQLRepository(SubastaDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// Metodo que se encarga de registrar un reclamo en la base de datos en PostgreSQL.
        /// </summary>
        /// <param name="reclamo">Parametro que de tipo Reclamo que contiene el detalle  del objeto.</param>
        /// <returns>Retorna el GUID del objeto si la operación fue exitosa</returns>
        public async Task<Guid> RegistrarReclamoAsync(Reclamo reclamo)
        {
            var reclamoBD = reclamo.ToPostgres();
            await _dbContext.Reclamo.AddAsync(reclamoBD);
            await _dbContext.SaveChangesAsync();
            return reclamoBD.Id;
        }
        /// <summary>
        /// Metodo que se encarga de actualizar el estado de un reclamo en la base de datos en PostgreSQL.
        /// </summary>
        /// <param name="idReclamo">Parametro que contiene el id del reclamo a modificar.</param>
        /// <param name="nuevoEstado">Parametro que contiene el valor del nuevo estado del reclamo.</param>
        /// <returns>Retorna un estado HTTP si la operación fue exitosa</returns>
        public async Task<HttpStatusCode> ActualizarEstadoReclamo(Guid idReclamo, string nuevoEstado)
        {
            var subasta = await _dbContext.Reclamo.FindAsync(idReclamo);
            subasta.Estado = nuevoEstado;
            await _dbContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }
    }
}
