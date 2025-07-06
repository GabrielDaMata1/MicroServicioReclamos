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
    /// Clase repository que implementa las operaciones que se pueden realizar sobre los reclamos de premios almacenados en MongoDB.
    /// </summary>
    public class ReclamoPremioMongoRepository : IReclamoPremioMongoRepository
    {
        /// <summary>
        /// Atributo que corresponde a la colección de reclamos de premios en la base de datos en MongoDB.
        /// </summary>
        private readonly IMongoCollection<ReclamoPremioMongo> _reclamoPremioCollection;

        public ReclamoPremioMongoRepository(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("MicroservicioReclamos");
            _reclamoPremioCollection = database.GetCollection<ReclamoPremioMongo>("ReclamosPremios");
        }
        /// <summary>
        /// Metodo que se encarga de registrar un reclamo de un premio en la base de datos en MongoDB.
        /// </summary>
        /// <param name="reclamoPremio">Parametro que de tipo ReclamoPremio que contiene el detalle  del objeto.</param>
        /// <returns>Retorna un estado HTTP si la operación fue exitosa</returns>
        public async Task<HttpStatusCode> RegistrarReclamoPremioAsync(ReclamoPremio reclamoPremio)
        {
            try
            {
                await _reclamoPremioCollection.InsertOneAsync(reclamoPremio.ToMongo());
                return HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                throw new MongoRepositoryException($"Error al intentar registrar el reclamo del premio en MongoDB: {ex.Message}", ex);
            }
        }
        /// <summary>
        /// Metodo que se encarga de consultar los reclamos de premios de un usuario en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idUsuario">Parametro que contiene el id del usuario que se consultan sus reclamos  de premios.</param>
        /// <returns>Retorna una lista de objetos ReclamoPremio con su detalle si la operación fue exitosa</returns>

        public async Task<List<ReclamoPremio>> ConsultarReclamosPremiosUsuarioMongo(Guid idUsuario)
        {
            var reclamosPremioMongo = await _reclamoPremioCollection.Find(r => r.IdUsuario == idUsuario).ToListAsync();
            Console.WriteLine(reclamosPremioMongo.ElementAt(0).IdUsuario);
            var reclamos = reclamosPremioMongo.Select(s => ReclamoPremioFactory.CrearReclamoPremioConId(s.Id, s.IdUsuario, s.IdSubasta,
                s.DireccionEnvio, s.MetodoEntrega, s.FechaReclamo)).ToList();


            return reclamos;
        }
        /// <summary>
        /// Metodo que se encarga de consultar un reclamo de un premio en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idReclamoPremio">Parametro que contiene el id del reclamo del premio a consultar.</param>
        /// <returns>Retorna un objeto ReclamoPremio con su detalle si la operación fue exitosa</returns>
        public async Task<ReclamoPremio> ConsultarReclamoPremioMongo(Guid idReclamoPremio)
        {
            var reclamosPremioMongo = await _reclamoPremioCollection.Find(r => r.Id == idReclamoPremio).FirstOrDefaultAsync();
            var reclamos = ReclamoPremioFactory.CrearReclamoPremioConId(reclamosPremioMongo.Id, reclamosPremioMongo.IdUsuario, reclamosPremioMongo.IdSubasta,
                reclamosPremioMongo.DireccionEnvio, reclamosPremioMongo.MetodoEntrega, reclamosPremioMongo.FechaReclamo);

            return reclamos;
        }
    }
}
