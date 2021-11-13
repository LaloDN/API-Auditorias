using Microsoft.EntityFrameworkCore;
using RestAPI.Domain.IRepositories;
using RestAPI.Domain.Models;
using RestAPI.Presistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Presistence.Repositories
{
    public class PreguntaRepository : IPreguntaRepository
    {
        private readonly AplicationDbContext _context;


        public PreguntaRepository(AplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Guarda una pregunta dentro de la base de datos
        /// </summary>
        /// <param name="pregunta">Objeto pregunta parseado desde un JSON</param>
        /// <returns>Una tarea de guardar la pregunta</returns>
        public async Task<int> GuardarPregunta(Pregunta pregunta)
        {
            _context.Add(pregunta);
            await _context.SaveChangesAsync();
            //Buscamos el id con el que se guardo la pregunta y lo retornamos.
            int id = _context.Preguntas.Max(p => p.IdPregunta);
            return id;
        }

        /// <summary>
        /// Revisa si una pregunta ya está dentro de la base de datos
        /// </summary>
        /// <param name="pregunta">Objeto pregunta parseado desde un JSON</param>
        /// <returns>True si la pregunta ya esta en la base; False en caso contrario</returns>
        public async Task<bool> RevisarExistencia(Pregunta pregunta)
        {
            //Preguntamos si ya existe una pregunta con el mismo campo de descripción dentro de la base con LINQ
            var existence = await _context.Preguntas.AnyAsync(x => x.Descripcion == pregunta.Descripcion);
            return existence;
        }

        /// <summary>
        /// Obtiene la lista de preguntas registrada en la base de datos.
        /// </summary>
        /// <returns>Una lista de tipo Pregunta.</returns>
        public async Task<List<Pregunta>> ObtenerListaPreguntas()
        {
            var listaPreguntas = await _context.Preguntas.ToListAsync();
            return listaPreguntas;
        }
    }
}
