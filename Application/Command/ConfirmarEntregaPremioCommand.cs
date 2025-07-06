using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Command
{
    /// <summary>
    /// Clase Command que se encarga de enviar la solicitud para confirmar la entrega de el premio de una subasta.
    /// </summary>
    public class ConfirmarEntregaPremioCommand : IRequest<bool>
    {
        /// <summary>
        /// Atributo que corresponde al ID del premio reclamado en la base de datos.
        /// </summary>
        public Guid idReclamoPremio { get; set; }

        public ConfirmarEntregaPremioCommand(Guid idReclamoPremio) 
        {
            this.idReclamoPremio = idReclamoPremio;
        }
    }
}
