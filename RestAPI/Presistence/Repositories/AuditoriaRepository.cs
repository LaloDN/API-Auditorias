﻿using Microsoft.EntityFrameworkCore;
using RestAPI.Domain.IRepositories;
using RestAPI.Domain.Models;
using RestAPI.DTO;
using RestAPI.Presistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Presistence.Repositories
{
    public class AuditoriaRepository : IAuditoriaRepository
    {
        private readonly AplicationDbContext _context;

        public AuditoriaRepository(AplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Agrega un objeto Auditoria a la base de datos.
        /// </summary>
        /// <param name="auditoria">Objeto de tipo Auditoria a añadir.</param>
        /// <returns>Una tarea guardando la auditoria en la base.</returns>
        public async Task<int> CrearAuditoria(Auditoria auditoria)
        {
            _context.Add(auditoria);
            await _context.SaveChangesAsync();
            //Obtenemos el id con el que guardamos la auditoria y lo retornamos
            int id = _context.Auditorias.Max(a=>a.IdAuditoria);
            return id;
        }

        /// <summary>
        /// Agrega una lista de preguntas a una determinada auditoria por su id.
        /// </summary>
        /// <param name="preguntaRespuesta">Lista de tipo PreguntaRespuesta, en la cuál todas tienen
        ///                                 el mismo IdAuditoria y un IdRespuesta como 1 (nulo).</param>
        /// <returns>Una tarea guardando las pregutnas asignadas a la base.</returns>
        public async Task AsignarPreguntas(List<PreguntaRespuesta> preguntaRespuesta)
        {
            //AddRange me permite guardar una lista de registros dentro de la base 
            //de un jalón.
            _context.AddRange(preguntaRespuesta);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Obtiene todos los registros dentro de la tabla Auditorias.
        /// </summary>
        /// <returns>Una lista de tipo Auditoria</returns>
         public async Task<List<Auditoria>> ObtenerListaAuditorias()
        {
            var listaAuditorias = await _context.Auditorias.ToListAsync();
            return listaAuditorias;
        }

        /// <summary>
        /// Obtiene la lista de preguntas que estan dentro de una auditoría.
        /// </summary>
        /// <param name="idAuditoria">Id de la auditoría de la cuál queremos obtener sus preguntas.</param>
        /// <returns>Una lista de tipo PreguntaRespuesta con el mismo IdAuditoria.</returns>
       public async Task<List<PreguntaRespuesta>> ObtenerPreguntasAuditoria(int idAuditoria)
        {
            //Creamos una variable, en la que voy a guardar los registros de la tabla pregunta_Respuesta que su campo de IdAuditoria
            //coincida con el del parámetro id 
            var listaPreguntas = await _context.Pregunta_Respuesta.Where
                (x => x.IdAuditoria_PreguntaRespuesta == idAuditoria).ToListAsync();
            return listaPreguntas;
        }

        /// <summary>
        /// Modifica la respuesta de los registros de la tabla Pregunta_Respuesta,
        /// asignandoles una nueva respuesta diferente de nula.
        /// </summary>
        /// <param name="respPreg">Lista de tipo ResponderPreguntaDTO, en el que accedemos a las preguntas
        ///                         que vamos a contestar.</param>
        /// <returns>Una tarea guardando los cambios en los registros afectados de Pregunta_Respuesta</returns>
        public async Task ContestarPreguntas(List<ResponderPreguntaDTO> respPreg)
        {
            //Creamos una lista vacía con los objetos PreguntaRespuesta
            List<PreguntaRespuesta> LPreg = new List<PreguntaRespuesta>();
            foreach(ResponderPreguntaDTO rp in respPreg)
            {  //Creamos una variable auxiliar
                var registro = (from p in _context.Pregunta_Respuesta where p.IdPreguntaRespuesta == rp.idPreguntaRespuesta select p).FirstOrDefault();
                //Modificamos su valor
                registro.IdRespuesta_PreguntaRespuesta = rp.respuestaEscogida;
                //Y añadimos este objeto a LPreg
                LPreg.Add(registro);
            }
            _context.UpdateRange(LPreg);          
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Obtiene el estado de una auditoria según su id.
        /// </summary>
        /// <param name="idAuditoria">Id de la auditoria con el estado a obtener.</param>
        /// <returns>Una cadena con el estado de la auditoria,</returns>
        public async Task<string> ObtenerEstadoAuditoria(int idAuditoria)
        {
            //string estado = await _context.Auditorias.Select(a=>a.Estado).FirstOrDefaultAsync();
            string estado = await _context.Auditorias.Where(a => a.IdAuditoria == idAuditoria).Select(a => a.Estado).FirstOrDefaultAsync();
            return estado;
        }

        /// <summary>
        /// Recibe el estado que le voy a poner a mi auditoria y lo actualiza.
        /// </summary>
        /// <param name="estado">Cadena con el estado nuevo de la auditoria.</param>
        /// <param name="auditoria">Auditoria a la cual le voy a cambiar mi estado.</param>
        /// <returns>Una tarea guardando el nuevo estado de la auditoria.</returns>
        public async Task CambiarEstado(string estado,Auditoria auditoria)
        {
            auditoria.Estado = estado;
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Busca un auditoria a través de su id.
        /// </summary>
        /// <param name="idAuditoria">Id de la auditoria a buscar en la base de datos.</param>
        /// <returns>Un objeto Auditoria con la id que le pasamos.</returns>
        public async Task<Auditoria> ObtenerAuditoria(int idAuditoria)
        {
            //Encuentra el primer resultado que encuentre con el id que le pasamos por parámetro
            var auditoria = await _context.Auditorias.Where(a => a.IdAuditoria == idAuditoria).FirstOrDefaultAsync();
            return auditoria;
        }
    }
}
