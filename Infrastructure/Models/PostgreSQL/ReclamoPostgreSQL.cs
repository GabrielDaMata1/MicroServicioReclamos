using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.PostgreSQL
{
    public class ReclamoPostgreSQL
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid IdUsuario { get; set; }

        [Required]
        public Guid IdSubasta { get; set; }
        [Required]
        public string Descripcion { get; set; }
        [Required]
        public string Motivo { get; set; }

        [Required]
        public string UrlEvidencia { get; set; }

        [Required]
        public string Estado { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
