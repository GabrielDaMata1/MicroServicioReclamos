using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Domain.Value_Object;

namespace Application.DTOs
{
    /// <summary>
    /// Clase DTO que se encarga de encapsular la información par consultar una subasta en el Microservicio Subasta.
    /// </summary>
    public class SubastaDTO
    {
        /// <summary>
        /// Atributo que corresponde al ID de la subasta.
        /// </summary>
        [JsonPropertyName("idSubasta")]
        public Guid Id { get; set; }
        /// <summary>
        /// Atributo que corresponde al nombre de la subasta.
        /// </summary>
        public string nombreSubasta { get; set; }
        /// <summary>
        /// Atributo que corresponde a la descripcion de la subasta.
        /// </summary>
        public string descripcionSubasta { get; set; }
        /// <summary>
        /// Atributo que corresponde al ID del producto subastado en de la subasta.
        /// </summary>
        [JsonPropertyName("idProducto")]
        public Guid idProductoSubasta { get; set; }
        /// <summary>
        /// Atributo que corresponde a la fecha de inicio de la subasta.
        /// </summary>
        [JsonPropertyName("fechaInicio")]
        public DateTime fechaInicioSubasta { get; set; }
        /// <summary>
        /// Atributo que corresponde a la fecha fin de la subasta.
        /// </summary>
        [JsonPropertyName("fechaFin")]
        public DateTime fechaFinSubasta { get; set; }
        /// <summary>
        /// Atributo que corresponde al incremento minimo de la subasta.
        /// </summary>
        [JsonPropertyName("incrementoMinimo")] 
        public decimal incrementoMinimoSubasta { get; set; }
        /// <summary>
        /// Atributo que corresponde al precio de reserva de la subasta.
        /// </summary>
        [JsonPropertyName("precioReserva")]
        public decimal precioReservaSubasta { get; set; }
        /// <summary>
        /// Atributo que corresponde al estado de la subasta.
        /// </summary>
        [JsonPropertyName("estado")]
        public string estadoSubasta { get; set; }
        /// <summary>
        /// Atributo que corresponde al ID del subastador.
        /// </summary>
        public Guid idUsuario { get; set; }
    }
}
