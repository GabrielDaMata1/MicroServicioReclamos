using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Value_Object
{
    public class FechaResolucionVO
    {
        public DateTime fechaResolucion { get; set; }

        public FechaResolucionVO(DateTime fechaResolucion) 
        {
            this.fechaResolucion = fechaResolucion; 
        }
    }
}
