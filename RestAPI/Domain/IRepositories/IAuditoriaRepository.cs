using RestAPI.Domain.Models;
using RestAPI.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Domain.IRepositories
{
   public interface IAuditoriaRepository
    {
        Task<int> CrearAuditoria(Auditoria auditoria);
        Task AsignarPreguntas(List<PreguntaRespuesta> preguntaRespuesta);
        Task<List<Auditoria>> ObtenerListaAuditorias();
        Task<List<PreguntaRespuesta>> ObtenerPreguntasAuditoria(int idAuditoria);
        Task ContestarPreguntas(List<ResponderPreguntaDTO> respPreg);
        Task<string> ObtenerEstadoAuditoria(int idAuditoria);
        Task CambiarEstado(string estado, Auditoria auditoria);
        Task<Auditoria> ObtenerAuditoria(int idAuditoria);
    }
}
