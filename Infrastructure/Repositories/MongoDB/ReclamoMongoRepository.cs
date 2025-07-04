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
    public class ReclamoMongoRepository : IReclamoMongoRepository
    {
        private readonly IMongoCollection<ReclamoMongo> _reclamoCollection;
        private readonly ISubastaService _subastaService;

        public ReclamoMongoRepository(IMongoClient mongoClient, ISubastaService subastaService)
        {
            var database = mongoClient.GetDatabase("MicroservicioReclamos");
            _reclamoCollection = database.GetCollection<ReclamoMongo>("Reclamos");
            _subastaService = subastaService;
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

        public async Task<List<Reclamo>> ConsultarReclamosMongo()
        {
            var reclamosMongo = await _reclamoCollection.Find(_ => true).ToListAsync();

            var reclamos = reclamosMongo.Select(s => ReclamoFactory.CrearReclamoConId(s.Id, s.IdUsuario, s.IdSubasta,
                s.Descripcion, s.Motivo, s.UrlEvidencia, s.CreatedAt, s.Estado)).ToList();

            return reclamos;
        }

        public async Task<Reclamo> ConsultarReclamoMongo(Guid idReclamo)
        {
            var reclamosMongo = await _reclamoCollection.Find(r => r.Id == idReclamo).FirstOrDefaultAsync();

            var reclamo = ReclamoFactory.CrearReclamoConId(reclamosMongo.Id, reclamosMongo.IdUsuario, reclamosMongo.IdSubasta,
                reclamosMongo.Descripcion, reclamosMongo.Motivo, reclamosMongo.UrlEvidencia, reclamosMongo.CreatedAt, reclamosMongo.Estado);

            return reclamo;
        }

        public async Task<List<Reclamo>> ConsultarReclamosPorSubastadorMongo(Guid idSubastador)
        {
            var reclamosMongo = await _reclamoCollection.Find(_ => true).ToListAsync();

            if (!reclamosMongo.Any())
                return new List<Reclamo>();

            var reclamos = new List<Reclamo>();

            foreach (var reclamo in reclamosMongo)
            {
                Console.WriteLine(reclamo.IdSubasta);
                var subasta = await _subastaService.ObtenerSubastaPorGuid(reclamo.IdSubasta);
                Console.WriteLine(subasta.idUsuario);

                if (subasta == null || subasta.idUsuario != idSubastador)
                    continue;

                var reclamoSubastador = ReclamoFactory.CrearReclamoConId(reclamo.Id, reclamo.IdUsuario, reclamo.IdSubasta,
                reclamo.Descripcion, reclamo.Motivo, reclamo.UrlEvidencia, reclamo.CreatedAt, reclamo.Estado);

                reclamos.Add(reclamoSubastador);
            }

            return reclamos;
        }

    }
}
