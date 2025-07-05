using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Command
{
    public class ConfirmarEntregaPremioCommand : IRequest<bool>
    {
        public Guid idReclamoPremio { get; set; }

        public ConfirmarEntregaPremioCommand(Guid idReclamoPremio) 
        {
            this.idReclamoPremio = idReclamoPremio;
        }
    }
}
