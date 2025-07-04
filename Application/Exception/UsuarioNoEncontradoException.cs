using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exception
{
    public class UsuarioNoEncontradoException: System.Exception
    {
    public UsuarioNoEncontradoException() : base("Error, el usuario proporcionado no se encontró")
    {
    }

    }
}
