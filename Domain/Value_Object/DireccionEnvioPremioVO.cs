using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Value_Object
{
    public class DireccionEnvioPremioVO
    {
        public string direccionEnvio  { get; set; }

        public DireccionEnvioPremioVO(string direccionEnvio) 
        {
            this.direccionEnvio = direccionEnvio;
        }
    }   

}
