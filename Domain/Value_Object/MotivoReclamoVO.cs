using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Value_Object
{
    public class MotivoReclamoVO
    {
        public string motivoReclamo { get; set; }

        public MotivoReclamoVO(string motivoReclamo) 
        {
            this.motivoReclamo = motivoReclamo;
        }
    }
}
