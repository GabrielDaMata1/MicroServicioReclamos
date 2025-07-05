using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Value_Object
{
    public class MetodoEntregaPremioVO
    {
        public string metodoEntrega { get; set; }

        public MetodoEntregaPremioVO(string metodoEntrega)
        {
            this.metodoEntrega = metodoEntrega;
        }
    }
}
