using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Value_Object;

namespace Application.DTOs
{
    public class RegistrarReclamoDTO
    {
        public string correo { get; set; }

        public Guid idSubasta { get; set; }
        public string descripcion { get; set; }
        public string motivo { get; set; }

        public string urlEvidencia { get; set; }

    }
}
