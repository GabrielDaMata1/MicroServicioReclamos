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
    public class ReclamoPremioMongoRepository : IReclamoPremioMongoRepository
    {
        private readonly IMongoCollection<ReclamoPremioMongo> _reclamoPremioCollection;

        public ReclamoPremioMongoRepository(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("MicroservicioReclamos");
            _reclamoPremioCollection = database.GetCollection<ReclamoPremioMongo>("ReclamosPremios");
        }

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

        public async Task<List<ReclamoPremio>> ConsultarReclamosPremiosUsuarioMongo(Guid idUsuario)
        {
            var reclamosPremioMongo = await _reclamoPremioCollection.Find(r => r.IdUsuario == idUsuario).ToListAsync();
            Console.WriteLine(reclamosPremioMongo.ElementAt(0).IdUsuario);
            var reclamos = reclamosPremioMongo.Select(s => ReclamoPremioFactory.CrearReclamoPremioConId(s.Id, s.IdUsuario, s.IdSubasta,
                s.DireccionEnvio, s.MetodoEntrega, s.FechaReclamo)).ToList();


            return reclamos;
        }

        public async Task<ReclamoPremio> ConsultarReclamoPremioMongo(Guid idReclamoPremio)
        {
            var reclamosPremioMongo = await _reclamoPremioCollection.Find(r => r.Id == idReclamoPremio).FirstOrDefaultAsync();
            var reclamos = ReclamoPremioFactory.CrearReclamoPremioConId(reclamosPremioMongo.Id, reclamosPremioMongo.IdUsuario, reclamosPremioMongo.IdSubasta,
                reclamosPremioMongo.DireccionEnvio, reclamosPremioMongo.MetodoEntrega, reclamosPremioMongo.FechaReclamo);

            return reclamos;
        }
    }
}
