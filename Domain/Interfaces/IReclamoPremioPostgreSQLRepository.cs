using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    /// <summary>
    /// Clase interface que define las operaciones que se pueden realizar sobre los reclamos de premios almacenados en MongoDB.
    /// </summary>
    public interface IReclamoPremioPostgreSQLRepository
    {
        /// <summary>
        /// Metodo que se encarga de registrar un reclamo de un premio en la base de datos en PostgreSQL.
        /// </summary>
        /// <param name="reclamoPremio">Parametro que de tipo ReclamoPremio que contiene el detalle  del objeto.</param>
        /// <returns>Retorna el GUID del objeto si la operación fue exitosa</returns>
        Task<Guid> RegistrarReclamoPremioAsync(ReclamoPremio reclamoPremio);

    }
}
