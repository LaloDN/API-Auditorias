using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Domain.Models
{
    public class Auditoria
    {
        public Auditoria()
        {
            PreguntaRespuestas = new List<PreguntaRespuesta>();
        }

        [Key]
        public int IdAuditoria { get; set; }
        [Required]
        [Column(TypeName = "varChar(50)")]
        public string Descripcion { get; set; }
        [Required]
        [Column(TypeName = "varChar(50)")]
        public string Estado { get; set; }


        //Relación con departamento
        [ForeignKey("Departamento")]
        public int IdDepartamento_Auditoria { get; set; }
        public Departamento Departamento { get; set; }

        //Relación con PreguntaRespuesta
        public List<PreguntaRespuesta> PreguntaRespuestas { get; set; }
    }
}
