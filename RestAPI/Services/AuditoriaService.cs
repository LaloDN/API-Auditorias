using RestAPI.Domain.IRepositories;
using RestAPI.Domain.ISerivces;
using RestAPI.Domain.Models;
using RestAPI.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Services
{
    public class AuditoriaService : IAuditoriaService
    {
        private readonly IAuditoriaRepository _auditoriaRepository;
        public AuditoriaService(IAuditoriaRepository auditoriaRepository)
        {
            _auditoriaRepository = auditoriaRepository;
        }

        public async Task CrearAuditoria(Auditoria auditoria)
        {
            await _auditoriaRepository.CrearAuditoria(auditoria);
        }
        public async Task AsignarPreguntas(List<PreguntaRespuesta> preguntaRespuesta)
        {
            await _auditoriaRepository.AsignarPreguntas(preguntaRespuesta);
        }

        public async Task<List<Auditoria>> ObtenerListaAuditorias()
        {
           return  await _auditoriaRepository.ObtenerListaAuditorias();
        }

        public async Task<List<PreguntaRespuesta>> ObtenerPreguntasAuditoria(int idAuditoria)
        {
            return await _auditoriaRepository.ObtenerPreguntasAuditoria(idAuditoria);
        }

        public async Task ContestarPreguntas(List<ResponderPreguntaDTO> respPreg)
        {
            await _auditoriaRepository.ContestarPreguntas(respPreg);
        }
     }
}
