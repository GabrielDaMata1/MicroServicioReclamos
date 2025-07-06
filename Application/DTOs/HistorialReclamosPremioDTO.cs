using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.DTOs
{
    /// <summary>
    /// Clase DTO que se encarga de encapsular la información necesaria para mostrar el historial de reclamos de premios.
    /// </summary>
    public class HistorialReclamosPremioDTO
    {
        /// <summary>
        /// Atributo que corresponde al ID de la subasta que se le hace el reclamo del premio.
        /// </summary>
        public Guid id { get; set; }
        /// <summary>
        /// Atributo que corresponde al nombre de la subasta que se le hace el reclamo del premio.
        /// </summary>
        public string nombreSubasta { get; set; }
        /// <summary>
        /// Atributo que corresponde a la descripción de la subasta que se le hace el reclamo del premio.
        /// </summary>
        public string descripcionSubasta { get; set; }
        /// <summary>
        /// Atributo que corresponde a la fecha de inicio de la subasta que se le hace el reclamo del premio.
        /// </summary>
        public DateTime fechaInicioSubasta { get; set; }
        /// <summary>
        /// Atributo que corresponde a la fecha fin de la subasta que se le hace el reclamo del premio.
        /// </summary>
        public DateTime fechaFinSubasta { get; set; }
        /// <summary>
        /// Atributo que corresponde al incremento minimo de la subasta que se le hace el reclamo del premio.
        /// </summary>
        public decimal incrementoMinimoSubasta { get; set; }
        /// <summary>
        /// Atributo que corresponde al precio de reserva de la subasta que se le hace el reclamo del premio.
        /// </summary>
        public decimal precioReservaSubasta { get; set; }
        /// <summary>
        /// Atributo que corresponde al estado de la subasta que se le hace el reclamo del premio.
        /// </summary>
        public string estadoSubasta { get; set; }
        /// <summary>
        /// Atributo que corresponde al ID del reclamo de premio.
        /// </summary>
        public Guid idReclamoPremio { get; set; }
        /// <summary>
        /// Atributo que corresponde a la dirección de envio del reclamo de premio.
        /// </summary>
        public string direccionEnvio { get; set; }
        /// <summary>
        /// Atributo que corresponde al metodo de envio del reclamo de premio.
        /// </summary>
        public string metodoEnvio { get; set; }
        /// <summary>
        /// Atributo que corresponde a la fecha de reclamo de premio.
        /// </summary>
        public DateTime fechaReclamo { get; set; }
    }
}
