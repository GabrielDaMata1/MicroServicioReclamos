using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    /// <summary>
    /// Clase interface que define las operaciones que se pueden realizar sobre subastas, en el Microservicio Subasta.
    /// </summary>
    public interface ISubastaService
    {
        /// <summary>
        /// Método que se encarga de obtener una subasta por su ID en el Microservicio Subasta.
        /// </summary>
        /// <param name="idSubasta">Parametro que corresponde al ID de la subasta a consultar</param>
        /// <returns>Retorna un objeto Subasta que contiene la informacion de la subasta dada.
        /// Si no lo consigue, retorna null</returns>
        Task<Subasta> ObtenerSubastaPorGuid(Guid idSubasta);
    }
}
