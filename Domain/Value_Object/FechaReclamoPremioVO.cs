using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Value_Object
{
    public class FechaReclamoPremioVO
    {
        public DateTime fechaReclamo { get; set; }

        public FechaReclamoPremioVO(DateTime fechaReclamo)
        {
            this.fechaReclamo = fechaReclamo;
        }
    }
}
