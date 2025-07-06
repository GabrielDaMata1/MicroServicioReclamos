using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Value_Object;

namespace Application.DTOs
{
    /// <summary>
    /// Clase DTO que se encarga de encapsular la información necesaria para registrar un reclamo de una subastas.
    /// </summary>
    public class RegistrarReclamoDTO
    {
        /// <summary>
        /// Atributo que corresponde al correo del usuario que realiza el reclamo.
        /// </summary>
        public string correo { get; set; }
        /// <summary>
        /// Atributo que corresponde al ID de la subasta que se le hace el reclamo.
        /// </summary>
        public Guid idSubasta { get; set; }
        /// <summary>
        /// Atributo que corresponde a la descripción del reclamo.
        /// </summary>
        public string descripcion { get; set; }
        /// <summary>
        /// Atributo que corresponde al motivo del reclamo.
        /// </summary>
        public string motivo { get; set; }
        /// <summary>
        /// Atributo que corresponde a la direccion URL de la evidencia del reclamo en Firebase.
        /// </summary>
        public string urlEvidencia { get; set; }

    }
}
