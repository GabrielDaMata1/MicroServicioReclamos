using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Exceptions;
using Domain.Entities;
using Domain.Factory;
using Domain.Interfaces;
using Infrastructure.Mappers;
using Infrastructure.Models.MongoDB;
using MongoDB.Driver;

namespace Infrastructure.Repositories.MongoDB
{
    /// <summary>
    /// Clase repository que implementa las operaciones que se pueden realizar sobre la resolución de reclamos almacenados en MongoDB.
    /// </summary>
    public class ResolucionReclamoMongoRepository : IResolucionReclamoMongoRepository
    {
        /// <summary>
        /// Atributo que corresponde a la colección de resolución reclamos en la base de datos en MongoDB.
        /// </summary>
        private readonly IMongoCollection<ResolucionReclamoMongo> _resolucionReclamoCollection;

        public ResolucionReclamoMongoRepository(IMongoClient mongoClient, ISubastaService subastaService)
        {
            var database = mongoClient.GetDatabase("MicroservicioReclamos");
            _resolucionReclamoCollection = database.GetCollection<ResolucionReclamoMongo>("ResolucionReclamo");
        }
        /// <summary>
        /// Metodo que se encarga de registrar la resolucion de un reclamo en la base de datos en MongoDB.
        /// </summary>
        /// <param name="resolucionReclamo">Parametro que de tipo ResolucionReclamo que contiene el detalle  del objeto.</param>
        /// <returns>Retorna un estado HTTP si la operación fue exitosa</returns>
        public async Task<HttpStatusCode> RegistrarResolucionReclamoMongo(ResolucionReclamo resolucionReclamo)
        {
            try
            {
                await _resolucionReclamoCollection.InsertOneAsync(resolucionReclamo.ToMongo());
                return HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                throw new MongoRepositoryException($"Error al intentar registrar la resolucion del reclamo en MongoDB: {ex.Message}", ex);
            }
        }
        /// <summary>
        /// Metodo que se encarga de consultar la resolucion de un reclamo en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idReclamo">Parametro que contiene el id del reclamo a consultar.</param>
        /// <returns>Retorna un objeto ResolucionReclamo con su detalle si la operación fue exitosa</returns>
        public async Task<ResolucionReclamo> ConsultarResolucionReclamoMongo(Guid idReclamo)
        {
            var resolucionReclamoMongo = await _resolucionReclamoCollection.Find(r => r.IdReclamo == idReclamo).FirstOrDefaultAsync();
            if (resolucionReclamoMongo == null)
                return null;

            var resolucionReclamo = ResolucionReclamoFactory.CrearResolucionReclamoConId(resolucionReclamoMongo.Id, resolucionReclamoMongo.IdReclamo, 
                resolucionReclamoMongo.Descripcion, resolucionReclamoMongo.CreatedAt);
            return resolucionReclamo;
        }
    }
}