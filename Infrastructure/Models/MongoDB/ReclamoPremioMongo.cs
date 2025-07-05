using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Value_Object;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Infrastructure.Models.MongoDB
{
    public class ReclamoPremioMongo
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        [BsonRepresentation(BsonType.String)]
        [BsonElement("IdSubasta")]
        public Guid IdSubasta { get; set; }

        [BsonRepresentation(BsonType.String)]
        [BsonElement("IdUsuario")]
        public Guid IdUsuario { get; set; }
        [BsonElement("DireccionEnvio")]
        public string DireccionEnvio { get; set; }
        [BsonElement("MetodoEntrega")]
        public string MetodoEntrega { get; set; }
        [BsonElement("FechaReclamo")]
        public DateTime FechaReclamo { get; set; }
    }
}
