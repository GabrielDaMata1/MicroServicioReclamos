using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Events
{
    /// <summary>
    /// Clase Event que es consumida por un consumidor para registrar el reclamo de un premio en la base de datos en MongoDB
    /// </summary>
    public record ReclamoPremioRegistradoEvent(ReclamoPremio reclamoPremio);

}
