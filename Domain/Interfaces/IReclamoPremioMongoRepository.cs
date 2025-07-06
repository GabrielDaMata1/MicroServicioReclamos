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
    /// Clase interface que define las operaciones que se pueden realizar sobre los reclamos de premios almacenados en MongoDB.
    /// </summary>
    public interface IReclamoPremioMongoRepository
    {
        /// <summary>
        /// Metodo que se encarga de registrar un reclamo de un premio en la base de datos en MongoDB.
        /// </summary>
        /// <param name="reclamoPremio">Parametro que de tipo ReclamoPremio que contiene el detalle  del objeto.</param>
        /// <returns>Retorna un estado HTTP si la operación fue exitosa</returns>
        Task<HttpStatusCode> RegistrarReclamoPremioAsync(ReclamoPremio reclamoPremio);
        /// <summary>
        /// Metodo que se encarga de consultar los reclamos de premios de un usuario en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idUsuario">Parametro que contiene el id del usuario que se consultan sus reclamos  de premios.</param>
        /// <returns>Retorna una lista de objetos ReclamoPremio con su detalle si la operación fue exitosa</returns>
        Task<List<ReclamoPremio>> ConsultarReclamosPremiosUsuarioMongo(Guid idUsuario);
        /// <summary>
        /// Metodo que se encarga de consultar un reclamo de un premio en la base de datos en MongoDB.
        /// </summary>
        /// <param name="idReclamoPremio">Parametro que contiene el id del reclamo del premio a consultar.</param>
        /// <returns>Retorna un objeto ReclamoPremio con su detalle si la operación fue exitosa</returns>

        Task<ReclamoPremio> ConsultarReclamoPremioMongo(Guid idReclamoPremio);


    }
}
