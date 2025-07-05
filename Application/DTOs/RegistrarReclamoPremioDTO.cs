using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class RegistrarReclamoPremioDTO
    {
        public Guid idSubasta { get; set; }
        public string correo { get; set; }
        public string direccionEnvio { get; set; }
        public string metodoEntrega { get; set; }
    }
}
