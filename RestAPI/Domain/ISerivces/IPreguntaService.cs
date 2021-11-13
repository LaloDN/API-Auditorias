using RestAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Domain.ISerivces
{
    public interface IPreguntaService
    {
        Task<int> GuardarPregunta(Pregunta pregunta);
        Task<bool> RevisarExistencia(Pregunta pregunta);
        Task<List<Pregunta>> ObtenerListaPreguntas();
    }
}
