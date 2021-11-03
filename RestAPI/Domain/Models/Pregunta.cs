using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Domain.Models
{
    public class Pregunta
    {
        [Key]
        public int IdPregunta { get; set; }
        [Required]
        [Column(TypeName = "varChar(60)")]
        public string Descripcion { get; set; }
    }
}
