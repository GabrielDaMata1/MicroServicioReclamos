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
    /// Clase interface que define las operaciones que se pueden realizar sobre los reclamos almacenados en MongoDB.
    /// </summary>
    public interface IReclamoMongoRepository
    {
        /// <summary>
        /// Metodo que se encarga de registrar un reclamo en la base de datos en MongoDB.
        /// </summary>
        /// <param name="reclamo">Parametro que de tipo Reclamo que contiene el detalle  del objeto.</param>
        /// <returns>Retorna un estado HTTP si la operación fue exitosa</returns>
        Task<HttpStatusCode> RegistrarReclamoMongo(Reclamo reclamo);
        /// <summary>
        /// Metodo que se encarga de consultar los reclamos realizados por los usuarios en la base de datos en MongoDB.
        /// </summary>
        /// <returns>Retorna una lista de objetos Reclamo con su detalle si la operación fue exitosa</returns>
        Task<List<Reclamo>> ConsultarReclamosMongo();
        /// <summary>
        /// Metodo que se encarga de consultar un reclamo en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idReclamo">Parametro que contiene el id del reclamo a consultar.</param>
        /// <returns>Retorna un objeto Reclamo con su detalle si la operación fue exitosa</returns>
        Task<Reclamo> ConsultarReclamoMongo(Guid idReclamo);
        /// <summary>
        /// Metodo que se encarga de consultar los reclamos de un subastador en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idSubastador">Parametro que contiene el id del subastador que se consultan sus reclamos.</param>
        /// <returns>Retorna una lista de objetos Reclamo con su detalle si la operación fue exitosa</returns>
        Task<List<Reclamo>> ConsultarReclamosPorSubastadorMongo(Guid idSubastador);
        /// <summary>
        /// Metodo que se encarga de actualizar el estado de un reclamo en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idReclamo">Parametro que contiene el id del reclamo a modificar.</param>
        /// <param name="nuevoEstado">Parametro que contiene el valor del nuevo estado del reclamo.</param>
        /// <returns>Retorna un estado HTTP si la operación fue exitosa</returns>
        Task<HttpStatusCode> ActualizarEstadoReclamo(Guid idReclamo, string nuevoEstado);
    }
}
