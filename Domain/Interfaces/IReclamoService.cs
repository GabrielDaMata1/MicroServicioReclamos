using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    /// <summary>
    /// Clase interface que define las operaciones que se pueden realizar sobre reclamos, resoluciones de reclamos, y reclamos de premios almacenados en ambas bases de datos (PostgreSQL, MongoDB).
    /// </summary>
    public interface IReclamoService
    {
        /// <summary>
        /// Metodo que se encarga de registrar un reclamo en la base de datos en MongoDB.
        /// </summary>
        /// <param name="reclamo">Parametro que de tipo Reclamo que contiene el detalle  del objeto.</param>
        /// <returns>Retorna un estado HTTP si la operación fue exitosa</returns>
        Task<HttpStatusCode> RegistrarReclamoMongoAsync(Reclamo reclamo);
        /// <summary>
        /// Metodo que se encarga de registrar un reclamo en la base de datos en PostgreSQL.
        /// </summary>
        /// <param name="reclamo">Parametro que de tipo Reclamo que contiene el detalle  del objeto.</param>
        /// <returns>Retorna el GUID del objeto si la operación fue exitosa</returns>
        Task<Guid> RegistrarReclamoPostgreSQLAsync(Reclamo reclamo);
        /// <summary>
        /// Metodo que se encarga de consultar los reclamos realizados por los usuarios en la base de datos en MongoDB.
        /// </summary>
        /// <returns>Retorna una lista de objetos Reclamo con su detalle si la operación fue exitosa</returns>
        Task<List<Reclamo>> ConsultarReclamosMongoAsync();
        /// <summary>
        /// Metodo que se encarga de consultar un reclamo en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idReclamo">Parametro que contiene el id del reclamo a consultar.</param>
        /// <returns>Retorna un objeto Reclamo con su detalle si la operación fue exitosa</returns>
        Task<Reclamo> ConsultarReclamoMongoAsync(Guid idReclamo);
        /// <summary>
        /// Metodo que se encarga de consultar los reclamos de un subastador en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idSubastador">Parametro que contiene el id del subastador que se consultan sus reclamos.</param>
        /// <returns>Retorna una lista de objetos Reclamo con su detalle si la operación fue exitosa</returns>
        Task<List<Reclamo>> ConsultarReclamosPorSubastadorMongoAsync(Guid idSubastador);
        /// <summary>
        /// Metodo que se encarga de registrar la resolucion de un reclamo en la base de datos en MongoDB.
        /// </summary>
        /// <param name="resolucionReclamo">Parametro que de tipo ResolucionReclamo que contiene el detalle  del objeto.</param>
        /// <returns>Retorna un estado HTTP si la operación fue exitosa</returns>
        Task<HttpStatusCode> RegistrarResolucionReclamoMongoAsync(ResolucionReclamo resolucionReclamo);

        /// <summary>
        /// Metodo que se encarga de registrar la resolucion de un reclamo en la base de datos en PostgreSQL.
        /// </summary>
        /// <param name="resolucionReclamo">Parametro que de tipo ResolucionReclamo que contiene el detalle  del objeto.</param>
        /// <returns>Retorna el GUID del objeto si la operación fue exitosa</returns>
        Task<Guid> RegistrarResolucionReclamoPostgreSQLAsync(ResolucionReclamo resolucionReclamo);
        /// <summary>
        /// Metodo que se encarga de actualizar el estado de un reclamo en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idReclamo">Parametro que contiene el id del reclamo a modificar.</param>
        /// <param name="nuevoEstado">Parametro que contiene el valor del nuevo estado del reclamo.</param>
        /// <returns>Retorna un estado HTTP si la operación fue exitosa</returns>
        Task<HttpStatusCode> ActualizarEstadoReclamoMongoAsync(Guid idReclamo, string nuevoEstado);
        /// <summary>
        /// Metodo que se encarga de actualizar el estado de un reclamo en la base de datos en PostgreSQL.
        /// </summary>
        /// <param name="idReclamo">Parametro que contiene el id del reclamo a modificar.</param>
        /// <param name="nuevoEstado">Parametro que contiene el valor del nuevo estado del reclamo.</param>
        /// <returns>Retorna un estado HTTP si la operación fue exitosa</returns>
        Task<HttpStatusCode> ActualizarEstadoReclamoPostgreSQLAsync(Guid idReclamo, string nuevoEstado);
        /// <summary>
        /// Metodo que se encarga de consultar la resolucion de un reclamo en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idReclamo">Parametro que contiene el id del reclamo a consultar.</param>
        /// <returns>Retorna un objeto ResolucionReclamo con su detalle si la operación fue exitosa</returns>
        Task<ResolucionReclamo> ConsultarResolucionReclamoMongoAsync(Guid idReclamo);
        /// <summary>
        /// Metodo que se encarga de registrar un reclamo de un premio en la base de datos en MongoDB.
        /// </summary>
        /// <param name="reclamoPremio">Parametro que de tipo ReclamoPremio que contiene el detalle  del objeto.</param>
        /// <returns>Retorna un estado HTTP si la operación fue exitosa</returns>
        Task<HttpStatusCode> RegistrarReclamoPremioMongoAsync(ReclamoPremio reclamoPremio);
        /// <summary>
        /// Metodo que se encarga de registrar un reclamo de un premio en la base de datos en PostgreSQL.
        /// </summary>
        /// <param name="reclamoPremio">Parametro que de tipo ReclamoPremio que contiene el detalle  del objeto.</param>
        /// <returns>Retorna el GUID del objeto si la operación fue exitosa</returns>
        Task<Guid> RegistrarReclamoPremioPostgreSQLAsync(ReclamoPremio reclamoPremio);
        /// <summary>
        /// Metodo que se encarga de consultar los reclamos de premios de un usuario en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idUsuario">Parametro que contiene el id del usuario que se consultan sus reclamos  de premios.</param>
        /// <returns>Retorna una lista de objetos ReclamoPremio con su detalle si la operación fue exitosa</returns>
        Task<List<ReclamoPremio>> ConsultarReclamosPremiosUsuarioMongoAsync(Guid idUsuario);
        /// <summary>
        /// Metodo que se encarga de consultar un reclamo de un premio en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idReclamoPremio">Parametro que contiene el id del reclamo del premio a consultar.</param>
        /// <returns>Retorna un objeto ReclamoPremio con su detalle si la operación fue exitosa</returns>
        Task<ReclamoPremio> ConsultarReclamoPremioMongoAsync(Guid idReclamoPremio);

        /// <summary>
        /// Metodo que se encarga de consultar los reclamos realizados por un usuario en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idUsuario">Parametro que contiene el id del usuario cuyos reclamos se van a consultar.</param>
        /// <returns>Retorna una lista de objetos Reclamo con su detalle si la operación fue exitosa</returns>
        Task<List<Reclamo>> ConsultarReclamosUsuarioMongoAsync(Guid idUsuario);

    }
}
