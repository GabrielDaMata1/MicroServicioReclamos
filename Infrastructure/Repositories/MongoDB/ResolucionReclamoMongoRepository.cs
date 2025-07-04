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
    public class ResolucionReclamoMongoRepository : IResolucionReclamoMongoRepository
    {
        private readonly IMongoCollection<ResolucionReclamoMongo> _resolucionReclamoCollection;

        public ResolucionReclamoMongoRepository(IMongoClient mongoClient, ISubastaService subastaService)
        {
            var database = mongoClient.GetDatabase("MicroservicioReclamos");
            _resolucionReclamoCollection = database.GetCollection<ResolucionReclamoMongo>("ResolucionReclamo");
        }

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