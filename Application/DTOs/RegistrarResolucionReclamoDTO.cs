using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    /// <summary>
    /// Clase DTO que se encarga de encapsular la información necesaria para registrar la resolución de un reclamo de una subastas.
    /// </summary>
    public class RegistrarResolucionReclamoDTO
    {
        /// <summary>
        /// Atributo que corresponde a la descripción de la resolución del reclamo.
        /// </summary>
        public string resolucion { get; set; }

        /// <summary>
        /// Atributo que corresponde al ID del reclamo a solucionar.
        /// </summary>
        public Guid idReclamo { get; set; }
    }
}
