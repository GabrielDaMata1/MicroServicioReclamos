using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exception
{
    /// <summary>
    /// Clase Exception que se encarga de manejar los errores producidos al obtener los los reclamos en las bases de datos (PostgreSQL, MongoDB).
    /// </summary>
    public class FalloAlObtenerReclamoException : System.Exception
    {
        public FalloAlObtenerReclamoException() : base("Ha ocurrido un error al obtener el reclamo.") { }

        public FalloAlObtenerReclamoException(string mensaje) : base(mensaje) { }

        public FalloAlObtenerReclamoException(string mensaje, System.Exception innerException)
            : base(mensaje, innerException) { }
    }
}
