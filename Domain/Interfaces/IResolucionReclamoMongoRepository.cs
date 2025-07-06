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
    /// Clase interface que define las operaciones que se pueden realizar sobre las resoluciones de reclamos almacenados en MongoDB.
    /// </summary>
    public interface IResolucionReclamoMongoRepository
    {
        /// <summary>
        /// Metodo que se encarga de registrar la resolucion de un reclamo en la base de datos en MongoDB.
        /// </summary>
        /// <param name="resolucionReclamo">Parametro que de tipo ResolucionReclamo que contiene el detalle  del objeto.</param>
        /// <returns>Retorna un estado HTTP si la operación fue exitosa</returns>
        Task<HttpStatusCode> RegistrarResolucionReclamoMongo(ResolucionReclamo resolucionReclamo);
        /// <summary>
        /// Metodo que se encarga de consultar la resolucion de un reclamo en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idReclamo">Parametro que contiene el id del reclamo a consultar.</param>
        /// <returns>Retorna un objeto ResolucionReclamo con su detalle si la operación fue exitosa</returns>
        Task<ResolucionReclamo> ConsultarResolucionReclamoMongo(Guid idReclamo);
    }
}
