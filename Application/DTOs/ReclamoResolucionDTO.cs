using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    /// <summary>
    /// Clase DTO que se encarga de encapsular la información necesaria para mostrar la resolución de los reclamos de las subastas.
    /// </summary>
    public class ReclamoResolucionDTO
    {
        /// <summary>
        /// Atributo que corresponde al ID del reclamo.
        /// </summary>
        public Guid idReclamo { get; set; }
        /// <summary>
        /// Atributo que corresponde al correo del usuario que realiza el reclamo.
        /// </summary>
        public string correo { get; set; }
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
        /// <summary>
        /// Atributo que corresponde al estado del reclamo (Pendiente, Resuelto).
        /// </summary>
        public string estado { get; set; }
        /// <summary>
        /// Atributo que corresponde a la fecha donde se realizó el reclamo.
        /// </summary>
        public DateTime fecha { get; set; }
        /// <summary>
        /// Atributo que corresponde al ID de la subasta que se le hace el reclamo.
        /// </summary>
        public Guid idSubasta { get; set; }
        /// <summary>
        /// Atributo que corresponde al nombre de la subasta que se le hace el reclamo.
        /// </summary>
        public string nombreSubasta { get; set; }
        /// <summary>
        /// Atributo que corresponde al ID de la resolución del reclamo.
        /// </summary>
        public Guid idResolucion { get; set; }
        /// <summary>
        /// Atributo que corresponde a la descripción de la resolución del reclamo.
        /// </summary>
        public string descripcionResolucion { get; set; }
        /// <summary>
        /// Atributo que corresponde a la fecha de la resolución del reclamo.
        /// </summary>
        public DateTime fechaResolucion { get; set; }
    }
}
