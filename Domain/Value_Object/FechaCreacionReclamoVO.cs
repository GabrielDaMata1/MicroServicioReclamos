using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Value_Object
{
    public class FechaCreacionReclamoVO
    {
        public DateTime fechaCreacionReclamo { get; set; }

        public FechaCreacionReclamoVO(DateTime fechaCreacionReclamo) 
        {
            this.fechaCreacionReclamo = fechaCreacionReclamo;
        }
    }
}
