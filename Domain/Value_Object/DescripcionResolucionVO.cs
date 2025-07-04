using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Value_Object
{
    public class DescripcionResolucionVO
    {
        public string descripcionResolucion { get; set; }

        public DescripcionResolucionVO(string descripcionResolucion)
        {
            this.descripcionResolucion = descripcionResolucion;
        }
    }
}
