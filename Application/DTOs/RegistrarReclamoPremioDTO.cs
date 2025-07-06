using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    /// <summary>
    /// Clase DTO que se encarga de encapsular la información necesaria para registrar un reclamo de premio de una subasta.
    /// </summary>
    public class RegistrarReclamoPremioDTO
    {
        /// <summary>
        /// Atributo que corresponde al ID de la subasta que se le hace el reclamo del premio.
        /// </summary>
        public Guid idSubasta { get; set; }
        /// <summary>
        /// Atributo que corresponde al correo del usuario que realiza el reclamo del premio.
        /// </summary>
        public string correo { get; set; }
        /// <summary>
        /// Atributo que corresponde a la dirección de envio del reclamo de premio.
        /// </summary>
        public string direccionEnvio { get; set; }
        /// <summary>
        /// Atributo que corresponde al metodo de envio del reclamo de premio.
        /// </summary>
        public string metodoEntrega { get; set; }
    }
}
