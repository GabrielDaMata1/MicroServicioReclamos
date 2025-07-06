using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events
{
    /// <summary>
    /// Clase Event que es consumida por un consumidor para modificar el estado de la subasta a delivered en el Microservicio Subasta
    /// </summary>
    public record EntregaPremioConfirmadaEvent(Guid idSubasta);

}
