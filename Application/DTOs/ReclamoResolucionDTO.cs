using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ReclamoResolucionDTO
    {
        public Guid idReclamo { get; set; }
        public string correo { get; set; }

        public string descripcion { get; set; }
        public string motivo { get; set; }

        public string urlEvidencia { get; set; }

        public string estado { get; set; }

        public DateTime fecha { get; set; }

        public Guid idSubasta { get; set; }

        public string nombreSubasta { get; set; }

        public Guid idResolucion { get; set; }

        public string descripcionResolucion { get; set; }

        public DateTime fechaResolucion { get; set; }
    }
}
