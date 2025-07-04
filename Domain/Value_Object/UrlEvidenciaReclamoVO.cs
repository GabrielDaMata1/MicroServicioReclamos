using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Value_Object
{
    public class UrlEvidenciaReclamoVO
    {
        public string urlEvidenciaReclamo { get; set; }

        public UrlEvidenciaReclamoVO  (string urlEvidenciaReclamo)
        { 
           this.urlEvidenciaReclamo = urlEvidenciaReclamo;
        
        }
    }
}
