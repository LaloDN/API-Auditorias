using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Domain.Models
{
    public class PreguntaRespuesta
    {
        [Key]
        public int IdPreguntaRespuesta { get; set; }

        //Relación con mi tabla pregunta
        [ForeignKey("Pregunta")]
        public int IdPregunta_PreguntaRespuesta { get; set; }
        public Pregunta Pregunta { get; set; }

        //Realción con mi tabla respuesta
        [ForeignKey("Respuesta")]
        public int IdRespuesta_PreguntaRespuesta { get; set; }
        public Respuesta Respuesta { get; set; }

        //Relación con mi tabla auditoria
        [ForeignKey("Auditoria")]
        public int IdAuditoria_PreguntaRespuesta { get; set; }
        public Auditoria Auditoria { get; set; }
    }
}
