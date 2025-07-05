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
        private readonly IResolucionReclamoMongoRepository _resolucionReclamoMongoRepository;
        private readonly IResolucionReclamoPostgreSQLRepository _resolucionReclamoPostgreSQLRepository;
        private readonly IReclamoPremioMongoRepository _reclamoPremioMongoRepository;
        private readonly IReclamoPremioPostgreSQLRepository _reclamoPremioPostgreSQLRepository;


        public ReclamoService(IReclamoMongoRepository reclamoMongoRepository, IReclamoPostgreSQLRepository reclamoPostgreSQLRepository, IResolucionReclamoPostgreSQLRepository resolucionReclamoPostgreSQLRepository,
            IResolucionReclamoMongoRepository resolucionReclamoMongoRepository, IReclamoPremioMongoRepository reclamoPremioMongoRepository, IReclamoPremioPostgreSQLRepository reclamoPremioPostgreSQLRepository)

        {
            _reclamoMongoRepository = reclamoMongoRepository;
            _reclamoPostgreSQLRepository = reclamoPostgreSQLRepository;
            _resolucionReclamoMongoRepository = resolucionReclamoMongoRepository;
            _resolucionReclamoPostgreSQLRepository = resolucionReclamoPostgreSQLRepository;
            _reclamoPremioMongoRepository = reclamoPremioMongoRepository;
            _reclamoPremioPostgreSQLRepository = reclamoPremioPostgreSQLRepository;
        }

        public async Task<HttpStatusCode> ActualizarEstadoReclamoMongoAsync(Guid idReclamo, string nuevoEstado)
        {
            try
            {
                var resul = await _reclamoMongoRepository.ActualizarEstadoReclamo(idReclamo, nuevoEstado);
                return resul;
            }
            catch (System.Exception ex)
            {
                throw new MongoRepositoryException($"Error al intentar actualizar el estado del reclamo en MongoDB {ex.Message}", ex);
            }
        }

        public async Task<HttpStatusCode> ActualizarEstadoReclamoPostgreSQLAsync(Guid idReclamo, string nuevoEstado)
        {
            try
            {
                var resul = await _reclamoPostgreSQLRepository.ActualizarEstadoReclamo(idReclamo, nuevoEstado);
                return resul;
            }
            catch (System.Exception ex)
            {
                throw new PostgresRepositoryException($"Error al intentar actualizar el estado del reclamo en PostgreSQL {ex.Message}", ex);
            }
        }

        public async Task<Reclamo> ConsultarReclamoMongoAsync(Guid idReclamo)
        {
            try
            {
                var resul = await _reclamoMongoRepository.ConsultarReclamoMongo(idReclamo);
                return resul;
            }
            catch (System.Exception ex)
            {
                throw new MongoRepositoryException($"Error al intentar obtener el reclamo en MongoDB {ex.Message}", ex);
            }
        }

        public async Task<List<Reclamo>> ConsultarReclamosMongoAsync()
        {
            try
            {
                var resul = await _reclamoMongoRepository.ConsultarReclamosMongo();
                return resul;
            }
            catch (System.Exception ex)
            {
                throw new MongoRepositoryException($"Error al intentar obtener los reclamos en MongoDB {ex.Message}", ex);
            }
        }

        public async Task<List<Reclamo>> ConsultarReclamosPorSubastadorMongoAsync(Guid idSubastador)
        {
            try
            {
                var resul = await _reclamoMongoRepository.ConsultarReclamosPorSubastadorMongo(idSubastador);
                return resul;
            }
            catch (System.Exception ex)
            {
                throw new MongoRepositoryException($"Error al intentar obtener los reclamos del subastador {ex.Message}", ex);
            }
        }

        public async Task<ResolucionReclamo> ConsultarResolucionReclamoMongoAsync(Guid idReclamo)
        {
            try
            {
                var resul = await _resolucionReclamoMongoRepository.ConsultarResolucionReclamoMongo(idReclamo);
                return resul;
            }
            catch (System.Exception ex)
            {
                throw new MongoRepositoryException($"Error al intentar obtener la resolución del reclamo del subastador {ex.Message}", ex);
            }
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

        public async Task<HttpStatusCode> RegistrarReclamoPremioMongoAsync(ReclamoPremio reclamoPremio)
        {
            try
            {
                var resul = await _reclamoPremioMongoRepository.RegistrarReclamoPremioAsync(reclamoPremio);
                return resul;
            }
            catch (System.Exception ex)
            {
                throw new MongoRepositoryException($"Error al intentar registrar el reclamo del premio en MongoDB {ex.Message}", ex);
            }
        }

        public async Task<Guid> RegistrarReclamoPremioPostgreSQLAsync(ReclamoPremio reclamoPremio)
        {
            try
            {
                var resul = await _reclamoPremioPostgreSQLRepository.RegistrarReclamoPremioAsync(reclamoPremio);
                return resul;
            }
            catch (System.Exception ex)
            {
                throw new PostgresRepositoryException($"Error al intentar registrar el reclamo del premio en PostgreSQL {ex.Message}", ex);
            }
        }

        public async Task<HttpStatusCode> RegistrarResolucionReclamoMongoAsync(ResolucionReclamo resolucionReclamo)
        {
            try
            {
                var resul = await _resolucionReclamoMongoRepository.RegistrarResolucionReclamoMongo(resolucionReclamo);
                return resul;
            }
            catch (System.Exception ex)
            {
                throw new MongoRepositoryException($"Error al intentar registrar la resolucion del reclamo en MongoDB {ex.Message}", ex);
            }
        }

        public async Task<Guid> RegistrarResolucionReclamoPostgreSQLAsync(ResolucionReclamo resolucionReclamo)
        {
            try
            {
                var resul = await _resolucionReclamoPostgreSQLRepository.RegistrarResolucionReclamo(resolucionReclamo);
                return resul;
            }
            catch (System.Exception ex)
            {
                throw new PostgresRepositoryException($"Error al intentar registrar el reclamo en PostgreSQL {ex.Message}", ex);
            }
        }
    }
}
