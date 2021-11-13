using RestAPI.Domain.IRepositories;
using RestAPI.Domain.ISerivces;
using RestAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Services
{
    public class PreguntaService : IPreguntaService
    {
        private readonly IPreguntaRepository _preguntaRepository;

        public PreguntaService(IPreguntaRepository preguntaRepository)
        {
            _preguntaRepository = preguntaRepository;
        }

        public async Task<int> GuardarPregunta(Pregunta pregunta)
        {
            return await _preguntaRepository.GuardarPregunta(pregunta);
        }

        public async Task<bool> RevisarExistencia(Pregunta pregunta)
        {
            return await _preguntaRepository.RevisarExistencia(pregunta);
        }

        public async Task<List<Pregunta>> ObtenerListaPreguntas()
        {
            return await _preguntaRepository.ObtenerListaPreguntas();
        }
    }
}
