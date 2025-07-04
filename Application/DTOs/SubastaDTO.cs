using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Domain.Value_Object;

namespace Application.DTOs
{
    public class SubastaDTO
    {
        [JsonPropertyName("idSubasta")]
        public Guid Id { get; set; }
        public string nombreSubasta { get; set; }
        public string descripcionSubasta { get; set; }
        [JsonPropertyName("idProducto")]
        public Guid idProductoSubasta { get; set; }
        [JsonPropertyName("fechaInicio")]
        public DateTime fechaInicioSubasta { get; set; }
        [JsonPropertyName("fechaFin")]
        public DateTime fechaFinSubasta { get; set; }
        [JsonPropertyName("incrementoMinimo")] 
        public decimal incrementoMinimoSubasta { get; set; }
        [JsonPropertyName("precioReserva")]
        public decimal precioReservaSubasta { get; set; }
        [JsonPropertyName("estado")]
        public string estadoSubasta { get; set; }

        public Guid idUsuario { get; set; }
    }
}
