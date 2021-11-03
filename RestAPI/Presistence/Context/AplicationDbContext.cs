using Microsoft.EntityFrameworkCore;
using RestAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Presistence.Context
{
    public class AplicationDbContext : DbContext
    {
        public DbSet<Auditoria> Auditorias { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Pregunta> Preguntas { get; set; }
        public DbSet<Respuesta> Respuestas { get; set; }
        public DbSet<PreguntaRespuesta> Pregunta_Respuesta { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public AplicationDbContext(DbContextOptions<AplicationDbContext> options): base(options) { 
        }
    }
}
