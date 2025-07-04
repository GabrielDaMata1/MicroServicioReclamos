using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Exceptions;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Mappers;
using Infrastructure.Models.MongoDB;
using MongoDB.Driver;

namespace Infrastructure.Repositories.MongoDB
{
    public class ReclamoMongoRepository : IReclamoMongoRepository
    {
        private readonly IMongoCollection<ReclamoMongo> _reclamoCollection;

        public ReclamoMongoRepository(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("MicroservicioReclamos");
            _reclamoCollection = database.GetCollection<ReclamoMongo>("Reclamos");
        }

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
    }
}
