using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exception
{
    /// <summary>
    /// Clase Exception que se encarga de manejar los errores producidos al intentar obtener un usuario en el Microservicio Usuario.
    /// </summary>
    public class UsuarioNoEncontradoException: System.Exception
    {
    public UsuarioNoEncontradoException() : base("Error, el usuario proporcionado no se encontró")
    {
    }

    }
}
