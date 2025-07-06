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
    /// Clase interface que define las operaciones que se pueden realizar sobre los reclamos almacenados en PostgreSQL.
    /// </summary>
    public interface IReclamoPostgreSQLRepository
    {

        /// <summary>
        /// Metodo que se encarga de registrar un reclamo en la base de datos en PostgreSQL.
        /// </summary>
        /// <param name="reclamo">Parametro que de tipo Reclamo que contiene el detalle  del objeto.</param>
        /// <returns>Retorna el GUID del objeto si la operación fue exitosa</returns>
        Task<Guid> RegistrarReclamoAsync(Reclamo reclamo);
        /// <summary>
        /// Metodo que se encarga de actualizar el estado de un reclamo en la base de datos en PostgreSQL.
        /// </summary>
        /// <param name="idReclamo">Parametro que contiene el id del reclamo a modificar.</param>
        /// <param name="nuevoEstado">Parametro que contiene el valor del nuevo estado del reclamo.</param>
        /// <returns>Retorna un estado HTTP si la operación fue exitosa</returns>
        Task<HttpStatusCode> ActualizarEstadoReclamo(Guid idReclamo, string nuevoEstado);

    }
}
