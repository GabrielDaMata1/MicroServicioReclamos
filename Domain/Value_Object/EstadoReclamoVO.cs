using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Value_Object
{
    public class EstadoReclamoVO
    {
        public string estadoReclamo { get; set; }

        public EstadoReclamoVO(string estadoReclamo) 
        {
            this.estadoReclamo = estadoReclamo;
        }
    }
}
