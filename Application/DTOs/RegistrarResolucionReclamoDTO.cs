using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class RegistrarResolucionReclamoDTO
    {
        public string resolucion { get; set; }

        public Guid idReclamo { get; set; }
    }
}
