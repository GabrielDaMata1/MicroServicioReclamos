using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.PostgreSQL
{
    public class ResolucionReclamoPostgreSQL
    {
        [Key]
        public Guid Id { get; set; }

        public Guid IdReclamo { get; set; }
        [ForeignKey("IdReclamo")]
        public virtual ReclamoPostgreSQL Reclamo { get; set; }
        [Required]
        public string Descripcion { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
