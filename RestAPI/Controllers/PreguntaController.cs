using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestAPI.Domain.ISerivces;
using RestAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreguntaController : ControllerBase
    {
        private readonly IPreguntaService _preguntaService;
        public PreguntaController(IPreguntaService preguntaService)
        {
            _preguntaService = preguntaService;
        }

        //Endpoint para cargar pregutnas dentro del sistema.
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Pregunta pregunta)
        {
            try
            {
                //Existence va a tener un valor true o false dependiendo si ya existe la pregunta o no.
                var existence = await _preguntaService.RevisarExistencia(pregunta);
                //Si es verdadero, enviamos un error con un mensaje de que ya existe la pregunta.
                if (existence)
                {
                    return BadRequest(new { message= "La pregunta ya existe dentro de la base"});
                }
                //Si no existe aun la pregunta, la guardamos dentro de la base.
                await _preguntaService.GuardarPregunta(pregunta);
                //Mandamos un mensaje confirmando la operación.
                return Ok(new { message = "Pregunta registrada con éxito!" });
            }//Enviamos un error por si algo sale mal.
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Endpoint para obtener las preguntas que tenemos dentro de nuestro sistema.
        [HttpGet]
        public async Task<List<Pregunta>> Get()
        {
            return await _preguntaService.ObtenerListaPreguntas();
        }
    }
}
