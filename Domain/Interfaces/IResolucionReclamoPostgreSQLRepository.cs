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
    /// Clase interface que define las operaciones que se pueden realizar sobre las resoluciones de reclamos almacenados en PostgreSQL.
    /// </summary>
    public interface IResolucionReclamoPostgreSQLRepository
    {
        /// <summary>
        /// Metodo que se encarga de registrar la resolucion de un reclamo en la base de datos en PostgreSQL.
        /// </summary>
        /// <param name="resolucionReclamo">Parametro que de tipo ResolucionReclamo que contiene el detalle  del objeto.</param>
        /// <returns>Retorna el GUID del objeto si la operación fue exitosa</returns>
        Task<Guid> RegistrarResolucionReclamo(ResolucionReclamo resolucionReclamo);
    }
}
