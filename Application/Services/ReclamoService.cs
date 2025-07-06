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
    /// <summary>
    /// Clase Service que se encarga de procesar todas las operaciones sobre un reclamo, reclamos de premios, y resolución de reclamos, incluyendo las operaciones con bases de datos (PostgreSQL, MongoDB).
    /// </summary>
    public class ReclamoService : IReclamoService
    {
        /// <summary>
        /// Atributo que corresponde al repositorio de reclamos en la base de datos en MongoDB.
        /// </summary>
        private readonly IReclamoMongoRepository _reclamoMongoRepository;
        /// <summary>
        /// Atributo que corresponde al repositorio de reclamos en la base de datos en PostgreSQL.
        /// </summary>
        private readonly IReclamoPostgreSQLRepository _reclamoPostgreSQLRepository;
        /// <summary>
        /// Atributo que corresponde al repositorio de resolución de reclamos en la base de datos en MongoDB.
        /// </summary>
        private readonly IResolucionReclamoMongoRepository _resolucionReclamoMongoRepository;
        /// <summary>
        /// Atributo que corresponde al repositorio de  resolución de reclamos en la base de datos en PostgreSQL.
        /// </summary>
        private readonly IResolucionReclamoPostgreSQLRepository _resolucionReclamoPostgreSQLRepository;
        /// <summary>
        /// Atributo que corresponde al repositorio de reclamos de premios en la base de datos en MongoDB.
        /// </summary>
        private readonly IReclamoPremioMongoRepository _reclamoPremioMongoRepository;
        /// <summary>
        /// Atributo que corresponde al repositorio de reclamos de premios en la base de datos en PostgreSQL.
        /// </summary>
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

        /// <summary>
        /// Metodo que se encarga de actualizar el estado de un reclamo en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idReclamo">Parametro que contiene el id del reclamo a modificar.</param>
        /// <param name="nuevoEstado">Parametro que contiene el valor del nuevo estado del reclamo.</param>
        /// <returns>Retorna un estado HTTP si la operación fue exitosa</returns>
        /// <exception cref="MongoRepositoryException">
        /// Esta excepcion ocurre si sucede un problema al actualizar el estado del reclamo en la base de datos.
        /// </exception>
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
        /// <summary>
        /// Metodo que se encarga de actualizar el estado de un reclamo en la base de datos en PostgreSQL.
        /// </summary>
        /// <param name="idReclamo">Parametro que contiene el id del reclamo a modificar.</param>
        /// <param name="nuevoEstado">Parametro que contiene el valor del nuevo estado del reclamo.</param>
        /// <returns>Retorna un estado HTTP si la operación fue exitosa</returns>
        /// <exception cref="PostgresRepositoryException">
        /// Esta excepcion ocurre si sucede un problema al actualizar el estado del reclamo en la base de datos.
        /// </exception>
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
        /// <summary>
        /// Metodo que se encarga de consultar un reclamo en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idReclamo">Parametro que contiene el id del reclamo a consultar.</param>
        /// <returns>Retorna un objeto Reclamo con su detalle si la operación fue exitosa</returns>
        /// <exception cref="MongoRepositoryException">
        /// Esta excepcion ocurre si sucede un problema al consultar el reclamo en la base de datos.
        /// </exception>
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
        /// <summary>
        /// Metodo que se encarga de consultar un reclamo de un premio en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idReclamoPremio">Parametro que contiene el id del reclamo del premio a consultar.</param>
        /// <returns>Retorna un objeto ReclamoPremio con su detalle si la operación fue exitosa</returns>
        /// <exception cref="MongoRepositoryException">
        /// Esta excepcion ocurre si sucede un problema al consultar el reclamo del premio en la base de datos.
        /// </exception>
        public async Task<ReclamoPremio> ConsultarReclamoPremioMongoAsync(Guid idReclamoPremio)
        {
            try
            {
                var resul = await _reclamoPremioMongoRepository.ConsultarReclamoPremioMongo(idReclamoPremio);
                return resul;
            }
            catch (System.Exception ex)
            {
                throw new MongoRepositoryException($"Error al intentar obtener el reclamo del premio en MongoDB {ex.Message}", ex);
            }

        }

        /// <summary>
        /// Metodo que se encarga de consultar los reclamos realizados por los usuarios en la base de datos en MongoDB.
        /// </summary>
        /// <returns>Retorna una lista de objetos Reclamo con su detalle si la operación fue exitosa</returns>
        /// <exception cref="MongoRepositoryException">
        /// Esta excepcion ocurre si sucede un problema al consultar los reclamos en la base de datos.
        /// </exception>
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

        /// <summary>
        /// Metodo que se encarga de consultar los reclamos de un subastador en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idSubastador">Parametro que contiene el id del subastador que se consultan sus reclamos.</param>
        /// <returns>Retorna una lista de objetos Reclamo con su detalle si la operación fue exitosa</returns>
        /// <exception cref="MongoRepositoryException">
        /// Esta excepcion ocurre si sucede un problema al consultar los reclamos en la base de datos.
        /// </exception>
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

        /// <summary>
        /// Metodo que se encarga de consultar los reclamos de premios de un usuario en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idUsuario">Parametro que contiene el id del usuario que se consultan sus reclamos  de premios.</param>
        /// <returns>Retorna una lista de objetos ReclamoPremio con su detalle si la operación fue exitosa</returns>
        /// <exception cref="MongoRepositoryException">
        /// Esta excepcion ocurre si sucede un problema al consultar los reclamos de premios en la base de datos.
        /// </exception>
        public async Task<List<ReclamoPremio>> ConsultarReclamosPremiosUsuarioMongoAsync(Guid idUsuario)
        {
            try
            {
                var resul = await _reclamoPremioMongoRepository.ConsultarReclamosPremiosUsuarioMongo(idUsuario);
                return resul;
            }
            catch (System.Exception ex)
            {
                throw new MongoRepositoryException($"Error al intentar obtener los reclamos de premios del usuario {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Metodo que se encarga de consultar la resolucion de un reclamo en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idReclamo">Parametro que contiene el id del reclamo a consultar.</param>
        /// <returns>Retorna un objeto ResolucionReclamo con su detalle si la operación fue exitosa</returns>
        /// <exception cref="MongoRepositoryException">
        /// Esta excepcion ocurre si sucede un problema al consultar la resolucion del reclamo en la base de datos.
        /// </exception>
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
        /// <summary>
        /// Metodo que se encarga de registrar un reclamo en la base de datos en MongoDB.
        /// </summary>
        /// <param name="reclamo">Parametro que de tipo Reclamo que contiene el detalle  del objeto.</param>
        /// <returns>Retorna un estado HTTP si la operación fue exitosa</returns>
        /// <exception cref="MongoRepositoryException">
        /// Esta excepcion ocurre si sucede un problema al registrar el reclamo en la base de datos.
        /// </exception>
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
        /// <summary>
        /// Metodo que se encarga de registrar un reclamo en la base de datos en PostgreSQL.
        /// </summary>
        /// <param name="reclamo">Parametro que de tipo Reclamo que contiene el detalle  del objeto.</param>
        /// <returns>Retorna el GUID del objeto si la operación fue exitosa</returns>
        /// <exception cref="PostgresRepositoryException">
        /// Esta excepcion ocurre si sucede un problema al registrar el reclamo en la base de datos.
        /// </exception>
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
        /// <summary>
        /// Metodo que se encarga de registrar un reclamo de un premio en la base de datos en MongoDB.
        /// </summary>
        /// <param name="reclamoPremio">Parametro que de tipo ReclamoPremio que contiene el detalle  del objeto.</param>
        /// <returns>Retorna un estado HTTP si la operación fue exitosa</returns>
        /// <exception cref="MongoRepositoryException">
        /// Esta excepcion ocurre si sucede un problema al registrar el reclamo del premio en la base de datos.
        /// </exception>
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
        /// <summary>
        /// Metodo que se encarga de registrar un reclamo de un premio en la base de datos en PostgreSQL.
        /// </summary>
        /// <param name="reclamoPremio">Parametro que de tipo ReclamoPremio que contiene el detalle  del objeto.</param>
        /// <returns>Retorna el GUID del objeto si la operación fue exitosa</returns>
        /// <exception cref="PostgresRepositoryException">
        /// Esta excepcion ocurre si sucede un problema al registrar el reclamo del premio en la base de datos.
        /// </exception>
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
        /// <summary>
        /// Metodo que se encarga de registrar la resolucion de un reclamo en la base de datos en MongoDB.
        /// </summary>
        /// <param name="resolucionReclamo">Parametro que de tipo ResolucionReclamo que contiene el detalle  del objeto.</param>
        /// <returns>Retorna un estado HTTP si la operación fue exitosa</returns>
        /// <exception cref="MongoRepositoryException">
        /// Esta excepcion ocurre si sucede un problema al registrar la resolucion de un reclamo en la base de datos.
        /// </exception>

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
        /// <summary>
        /// Metodo que se encarga de registrar la resolucion de un reclamo en la base de datos en PostgreSQL.
        /// </summary>
        /// <param name="resolucionReclamo">Parametro que de tipo ResolucionReclamo que contiene el detalle  del objeto.</param>
        /// <returns>Retorna el GUID del objeto si la operación fue exitosa</returns>
        /// <exception cref="PostgresRepositoryException">
        /// Esta excepcion ocurre si sucede un problema al registrar la resolucion de un reclamo en la base de datos.
        /// </exception>
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
