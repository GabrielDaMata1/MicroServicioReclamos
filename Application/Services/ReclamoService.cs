using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Exceptions;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class ReclamoService : IReclamoService
    {

        private readonly IReclamoMongoRepository _reclamoMongoRepository;
        private readonly IReclamoPostgreSQLRepository _reclamoPostgreSQLRepository;


        public ReclamoService(IReclamoMongoRepository reclamoMongoRepository, IReclamoPostgreSQLRepository reclamoPostgreSQLRepository)

        {
            _reclamoMongoRepository = reclamoMongoRepository;
            _reclamoPostgreSQLRepository = reclamoPostgreSQLRepository;

        }
        public async Task<HttpStatusCode> RegistrarReclamoMongoAsync(Reclamo reclamo)
        {
            try
            {
                var resul = await _reclamoMongoRepository.RegistrarReclamoMongo(reclamo);
                return resul;
            }
            catch (System.Exception ex)
            {
                throw new MongoRepositoryException($"Error al intentar registrar el reclamo en MongoDB {ex.Message}", ex);
            }
        }

        public async Task<Guid> RegistrarReclamoPostgreSQLAsync(Reclamo reclamo)
        {
            try
            {
                var resul = await _reclamoPostgreSQLRepository.RegistrarReclamoAsync(reclamo);
                return resul;
            }
            catch (System.Exception ex)
            {
                throw new PostgresRepositoryException($"Error al intentar registrar el reclamo en PostgreSQL {ex.Message}", ex);
            }
        }
    }
}
