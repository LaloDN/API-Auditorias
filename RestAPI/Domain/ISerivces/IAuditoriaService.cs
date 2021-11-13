using RestAPI.Domain.Models;
using RestAPI.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Domain.ISerivces
{
    public interface IAuditoriaService
    {
        Task<int> CrearAuditoria(Auditoria auditoria);
        Task AsignarPreguntas(List<PreguntaRespuesta> preguntaRespuesta);
        Task<List<Auditoria>> ObtenerListaAuditorias();
        Task ContestarPreguntas(List<ResponderPreguntaDTO> respPreg);
        Task<List<PreguntaRespuesta>> ObtenerPreguntasAuditoria(int idAuditoria);
        Task<string> ObtenerEstadoAuditoria(int idAuditoria);
        Task CambiarEstado(string estado, Auditoria auditoria);
        Task<Auditoria> ObtenerAuditoria(int idAuditoria);
    }
}
