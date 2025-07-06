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
    /// Clase repository que implementa las operaciones que se pueden realizar sobre los reclamos almacenados en MongoDB.
    /// </summary>
    public class ReclamoMongoRepository : IReclamoMongoRepository
    {
        /// <summary>
        /// Atributo que corresponde a la colección de reclamos en la base de datos en MongoDB.
        /// </summary>
        private readonly IMongoCollection<ReclamoMongo> _reclamoCollection;
        /// <summary>
        /// Atributo que corresponde a las operaciones posibles que se pueden realizar sobre una subasta en el Microservicio Subasta, el cual será inyectado por inversión de dependencias.
        /// </summary>
        private readonly ISubastaService _subastaService;

        public ReclamoMongoRepository(IMongoClient mongoClient, ISubastaService subastaService)
        {
            var database = mongoClient.GetDatabase("MicroservicioReclamos");
            _reclamoCollection = database.GetCollection<ReclamoMongo>("Reclamos");
            _subastaService = subastaService;
        }
        /// <summary>
        /// Metodo que se encarga de registrar un reclamo en la base de datos en MongoDB.
        /// </summary>
        /// <param name="reclamo">Parametro que de tipo Reclamo que contiene el detalle  del objeto.</param>
        /// <returns>Retorna un estado HTTP si la operación fue exitosa</returns>
        /// <exception cref="MongoRepositoryException">
        /// Esta excepcion ocurre si sucede un problema al registrar el reclamo en la base de datos.
        /// </exception>
        public async Task<HttpStatusCode> RegistrarReclamoMongo(Reclamo reclamo)
        {
            try
            {
                await _reclamoCollection.InsertOneAsync(reclamo.ToMongo());
                return HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                throw new MongoRepositoryException($"Error al intentar registrar el historial de pagos en MongoDB: {ex.Message}", ex);
            }
        }
        /// <summary>
        /// Metodo que se encarga de consultar los reclamos realizados por los usuarios en la base de datos en MongoDB.
        /// </summary>
        /// <returns>Retorna una lista de objetos Reclamo con su detalle si la operación fue exitosa</returns>
        public async Task<List<Reclamo>> ConsultarReclamosMongo()
        {
            var reclamosMongo = await _reclamoCollection.Find(_ => true).ToListAsync();

            var reclamos = reclamosMongo.Select(s => ReclamoFactory.CrearReclamoConId(s.Id, s.IdUsuario, s.IdSubasta,
                s.Descripcion, s.Motivo, s.UrlEvidencia, s.CreatedAt, s.Estado)).ToList();

            return reclamos;
        }
        /// <summary>
        /// Metodo que se encarga de consultar un reclamo en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idReclamo">Parametro que contiene el id del reclamo a consultar.</param>
        /// <returns>Retorna un objeto Reclamo con su detalle si la operación fue exitosa</returns>
        public async Task<Reclamo> ConsultarReclamoMongo(Guid idReclamo)
        {
            var reclamosMongo = await _reclamoCollection.Find(r => r.Id == idReclamo).FirstOrDefaultAsync();

            var reclamo = ReclamoFactory.CrearReclamoConId(reclamosMongo.Id, reclamosMongo.IdUsuario, reclamosMongo.IdSubasta,
                reclamosMongo.Descripcion, reclamosMongo.Motivo, reclamosMongo.UrlEvidencia, reclamosMongo.CreatedAt, reclamosMongo.Estado);

            return reclamo;
        }
        /// <summary>
        /// Metodo que se encarga de consultar los reclamos de un subastador en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idSubastador">Parametro que contiene el id del subastador que se consultan sus reclamos.</param>
        /// <returns>Retorna una lista de objetos Reclamo con su detalle si la operación fue exitosa</returns>
        public async Task<List<Reclamo>> ConsultarReclamosPorSubastadorMongo(Guid idSubastador)
        {
            var reclamosMongo = await _reclamoCollection.Find(_ => true).ToListAsync();

            if (!reclamosMongo.Any())
                return new List<Reclamo>();

            var reclamos = new List<Reclamo>();

            foreach (var reclamo in reclamosMongo)
            {
                var subasta = await _subastaService.ObtenerSubastaPorGuid(reclamo.IdSubasta);
                if (subasta == null || subasta.idUsuario != idSubastador)
                    continue;

                var reclamoSubastador = ReclamoFactory.CrearReclamoConId(reclamo.Id, reclamo.IdUsuario, reclamo.IdSubasta,
                reclamo.Descripcion, reclamo.Motivo, reclamo.UrlEvidencia, reclamo.CreatedAt, reclamo.Estado);

                reclamos.Add(reclamoSubastador);
            }

            return reclamos;
        }
        /// <summary>
        /// Metodo que se encarga de actualizar el estado de un reclamo en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idReclamo">Parametro que contiene el id del reclamo a modificar.</param>
        /// <param name="nuevoEstado">Parametro que contiene el valor del nuevo estado del reclamo.</param>
        /// <returns>Retorna un estado HTTP si la operación fue exitosa</returns>
        public async Task<HttpStatusCode> ActualizarEstadoReclamo(Guid idReclamo, string nuevoEstado)
        {
            var filtro = Builders<ReclamoMongo>.Filter.Eq(s => s.Id, idReclamo);
            var actualizacion = Builders<ReclamoMongo>.Update.Set(s => s.Estado, nuevoEstado);

            await _reclamoCollection.UpdateOneAsync(filtro, actualizacion);
            return HttpStatusCode.OK;
        }

    }
}
