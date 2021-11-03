using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestAPI.Domain.ISerivces;
using RestAPI.Domain.Models;
using RestAPI.DTO;
using RestAPI.Presistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditoriaController : ControllerBase
    {
        private readonly IAuditoriaService _auditoriaService;

        public AuditoriaController(IAuditoriaService auditoriaService)
        {
            _auditoriaService = auditoriaService;
        }

        //Endpoint para agregar un objeto Auditoria a la base
        [HttpPost]
        public async Task <IActionResult>Post([FromBody]Auditoria auditoria)
        {
            try
            {
                //Mandamos a llamar el método CrearAuditoria y le pasamos el objeto a meter en la base
                await _auditoriaService.CrearAuditoria(auditoria);
                //Si todo sale bien le mandamos un mensaje confirmando la operación 
                return Ok(new { message = "Auditoría registrada con éxito!" });
            }
            //Le devolvemos un error si algo sale mal
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //Endpoint para obtener todas las auditorias dentro de la base
        [HttpGet]
        public async Task<List<Auditoria>> Get()
        {
            return await _auditoriaService.ObtenerListaAuditorias();
        }

        //Endpoint para obtener la lista de preguntas asociadas a una auditoria
        [Route("VerAuditoria")]
        [HttpGet]
        public async Task<List<PreguntaRespuesta>> Get([FromBody]IdAuditoria id)
        {
                //Variable para guardar el idAuditoria que nos trae el DTO
                int IdAuditoria = id.idAuditoria;
                //Creamos una variable en la que vamos a guardar la lista con las preguntas que nos traiga
                //este método, pasandole el id de la auditoria la cual queremos ver sus preguntas.
                var listaPreguntas   = await _auditoriaService.ObtenerPreguntasAuditoria(IdAuditoria);
                return listaPreguntas;
        }
        
        //Endpoint para contestar (o asignarles más bien) las preguntas dentro de una auditoria.
        [Route("AplicarAuditoria")]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] List<ResponderPreguntaDTO> respPreg)
        {
            try
            {
                //Le mandamos al método ContestarPreguntas nuestro objeto respPreg que tiene la llave primaria
                //IdPreguntaRespuesta y la llave foranea con la respuesta que le vamos a asignar a la pregunta.
                await _auditoriaService.ContestarPreguntas(respPreg);
                //Devolvemos un mensaje confirmando la operación.
                return Ok(new { message = "La auditoría ha sido aplicada con éxito!" });
            }//Mandamos un error si algo sale mal.
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Enpoint para asignarle preguntas a una auditoria
        [Route("CrearAuditoria")]
       [HttpPost]
       public async Task<IActionResult> Post([FromBody]List<PreguntaRespuesta> preguntaRespuesta)
        {
            try
            {
                /*Le pasamos una Lista de tipo PreguntaRespuesta a AsignarPreguntas, esta va a tener
                  la misma llave fóranea de IdAuditoria para cada uno de los objetos dentro de la lista
                   y como llave fóranea de respuesta va a tener siempre un 1, o sea, un nulo*/
                await _auditoriaService.AsignarPreguntas(preguntaRespuesta);
                //Retornamos un mensaje confirmando la operación
                return Ok(new { message = "Preguntas asignadas a la auditoría con éxito" });
            }
            //Mandamos un error si algo sale mal.
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

       

    }
}
