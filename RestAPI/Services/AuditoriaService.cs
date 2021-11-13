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

        public async Task<int> CrearAuditoria(Auditoria auditoria)
        {
            return await _auditoriaRepository.CrearAuditoria(auditoria);
        }
        public async Task AsignarPreguntas(List<PreguntaRespuesta> preguntaRespuesta)
        {
            await _auditoriaRepository.AsignarPreguntas(preguntaRespuesta);
        }

        public async Task<List<Auditoria>> ObtenerListaAuditorias()
        {
            return await _auditoriaRepository.ObtenerListaAuditorias();
        }

        public async Task<List<PreguntaRespuesta>> ObtenerPreguntasAuditoria(int idAuditoria)
        {
            return await _auditoriaRepository.ObtenerPreguntasAuditoria(idAuditoria);
        }

        public async Task ContestarPreguntas(List<ResponderPreguntaDTO> respPreg)
        {
            await _auditoriaRepository.ContestarPreguntas(respPreg);
        }

        public async Task<string> ObtenerEstadoAuditoria(int idAuditoria)
        {
            return await _auditoriaRepository.ObtenerEstadoAuditoria(idAuditoria);
        }
       public async Task CambiarEstado(string estado, Auditoria auditoria)
        {
            await _auditoriaRepository.CambiarEstado(estado, auditoria);
        }
        public async Task<Auditoria> ObtenerAuditoria(int idAuditoria)
        {
            return await _auditoriaRepository.ObtenerAuditoria(idAuditoria);
        }

    }
}
