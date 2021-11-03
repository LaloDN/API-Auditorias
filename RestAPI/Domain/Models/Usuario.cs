using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Domain.Models
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }
        [Required]
        [Column(TypeName="varchar(30)")]
        public string NombreUsuario { get; set; }
        [Required]
        [Column(TypeName ="varchar(30)")]
        public string Password { get; set; }
        [Required]
        [Column(TypeName ="varchar(20)")]
        public string Rol { get; set; }
    }
}
