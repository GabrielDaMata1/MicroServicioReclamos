using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Value_Object
{
    public class DescripcionReclamoVO
    {
        public string descripcionReclamo { get; set; }

        public DescripcionReclamoVO(string descripcionReclamo) 
        {
            this.descripcionReclamo = descripcionReclamo;
        }
    }
}
