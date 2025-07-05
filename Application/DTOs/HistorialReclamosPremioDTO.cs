using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class HistorialReclamosPremioDTO
    {

        public Guid id { get; set; }
        public string nombreSubasta { get; set; }
        public string descripcionSubasta { get; set; }
        public DateTime fechaInicioSubasta { get; set; }
        public DateTime fechaFinSubasta { get; set; }
        public decimal incrementoMinimoSubasta { get; set; }
        public decimal precioReservaSubasta { get; set; }
        public string estadoSubasta { get; set; }

        public Guid idReclamoPremio { get; set; }
        public string direccionEnvio { get; set; }

        public string metodoEnvio { get; set; }

        public DateTime fechaReclamo { get; set; }
    }
}
