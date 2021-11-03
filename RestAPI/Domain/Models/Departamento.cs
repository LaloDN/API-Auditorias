using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Domain.Models
{
    public class Departamento
    {
        public Departamento()
        {
            Auditorias = new List<Auditoria>();
        }

        [Key]
        public int IdDepartamento { get; set; }
        [Required]
        [Column(TypeName = "varChar(50)")]
        public string NombreDepartamento { get; set; }

        //Relación con auditoria
        public List<Auditoria> Auditorias { get; set; }
    }
}
