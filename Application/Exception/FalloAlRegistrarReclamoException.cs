using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exception
{
    public class FalloAlRegistrarReclamoException : System.Exception
    {
        public FalloAlRegistrarReclamoException() : base("Ha ocurrido un error al registrar el reclamo.") { }

        public FalloAlRegistrarReclamoException(string mensaje) : base(mensaje) { }

        public FalloAlRegistrarReclamoException(string mensaje, System.Exception innerException)
            : base(mensaje, innerException) { }
    }
}
