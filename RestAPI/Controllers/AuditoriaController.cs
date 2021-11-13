using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestAPI.Domain.ISerivces;
using RestAPI.Domain.Models;
using RestAPI.DTO;
using RestAPI.Presistence.Context;
using RestAPI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        //Protección del endpoint
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<int> Post([FromBody] Auditoria auditoria)
        {
            try
            {
                //En este objeto, con este HttpContext, me va a traer TODOS LOS ATRIBUTOS DEL TOKEN, incluidos los claims.
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                //Invoco al método que me va a traer exclusivamente el rol del usuario logeado, pasandole el identity.
                string rol = JWTConfigurator.TokenObtenerRol(identity);
                //Preguntamos si el usuario es un RH para continuar con el proceso
                if (rol == "RH")
                {
                    //Mandamos a llamar el método CrearAuditoria y le pasamos el objeto a meter en la base
                    int id = await _auditoriaService.CrearAuditoria(auditoria);
                    //Si todo sale bien, retornamos el id con el que se guardo la auditoria en la base
                    return id;
                }
                else
                {
                    //Le mandamos un mensaje de error si no puede entrar al método.
                    return -1;
                    // return BadRequest(new { message = " No tiene permiso de ejecutar esta operación " });
                }
            }
            //Le devolvemos un error si algo sale mal
            catch (Exception ex)
            {
                return -1;
                // return BadRequest(ex.Message);
            }
        }

        //Enpoint para asignarle preguntas a una auditoria
        [Route("CrearAuditoria")]
        [HttpPost]
        //Protección del endpoint
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Post([FromBody] List<PreguntaRespuesta> preguntaRespuesta)
        {
            try
            {
                //En este objeto, con este HttpContext, me va a traer TODOS LOS ATRIBUTOS DEL TOKEN, incluidos los claims.
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                //Invoco al método que me va a traer exclusivamente el rol del usuario logeado, pasandole el identity.
                string rol = JWTConfigurator.TokenObtenerRol(identity);
                //Preguntamos si el usuario es RH para seguir con la operación
                if (rol == "RH")
                {
                    /*Le pasamos una Lista de tipo PreguntaRespuesta a AsignarPreguntas, esta va a tener
                      la misma llave fóranea de IdAuditoria para cada uno de los objetos dentro de la lista
                       y como llave fóranea de respuesta va a tener siempre un 1, o sea, un nulo*/
                    await _auditoriaService.AsignarPreguntas(preguntaRespuesta);
                    //Retornamos un mensaje confirmando la operación
                    return Ok(new { message = "Preguntas asignadas a la auditoría con éxito" });
                }
                else
                {
                    //Si no tiene acceso al método le mandamos un error
                    return BadRequest(new { message = " No tiene permiso de ejecutar esta operación " });
                }
            }
            //Mandamos un error si algo sale mal.
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Endpoint para obtener todas las auditorias dentro de la base
        [HttpGet]
        //Protección del endpoint
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<List<Auditoria>> Get()
        {
            //En este objeto, con este HttpContext, me va a traer TODOS LOS ATRIBUTOS DEL TOKEN, incluidos los claims.
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            //Invoco al método que me va a traer exclusivamente el rol del usuario logeado, pasandole el identity.
            string rol = JWTConfigurator.TokenObtenerRol(identity);
            //Preguntamos si tiene el rol de Auditor o de RH, ambos pueden acceder a este método
            if (rol == "RH" || rol == "Auditor")
            {
                return await _auditoriaService.ObtenerListaAuditorias();
            }
            else
            {
                //Si no es auditor o RH, le mando una lista con una auditoria erronea.
                List<Auditoria> Error = new List<Auditoria> { new Auditoria { Descripcion = "Error no tiene permiso de ejectuar esta operación" } };
                return Error;
            }
        }

        //Endpoint para obtener la lista de preguntas asociadas a una auditoria
        [Route("VerAuditoria")]
        [HttpGet]
        //Protección del endpoint
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<List<PreguntaRespuesta>> Get([FromBody] IdAuditoria id)
        {
            //En este objeto, con este HttpContext, me va a traer TODOS LOS ATRIBUTOS DEL TOKEN, incluidos los claims.
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            //Invoco al método que me va a traer exclusivamente el rol del usuario logeado, pasandole el identity.
            string rol = JWTConfigurator.TokenObtenerRol(identity);
            //Preguntamos si el usuario es Auditor o RH
            if (rol == "Auditor" || rol == "RH")
            {
                //Variable para guardar el idAuditoria que nos trae el DTO
                int IdAuditoria = id.idAuditoria;
                //Creamos una variable en la que vamos a guardar la lista con las preguntas que nos traiga
                //este método, pasandole el id de la auditoria la cual queremos ver sus preguntas.
                var listaPreguntas = await _auditoriaService.ObtenerPreguntasAuditoria(IdAuditoria);
                return listaPreguntas;
            }
            else
            {
                //Mandamos una PreguntaRespuesta erroneo si no tiene acceso al método.
                List<PreguntaRespuesta> Error = new List<PreguntaRespuesta> { new PreguntaRespuesta { IdPreguntaRespuesta = -1 } };
                return Error;
            }
        }
        
        //Enpoint para cambiar el estado de una auditoria cuando la vamos a contestar.
        [Route("CambiarEstadoEP")]
        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<int> Put(IdAuditoria idAud)
        {
            //En este objeto, con este HttpContext, me va a traer TODOS LOS ATRIBUTOS DEL TOKEN, incluidos los claims.
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            //Invoco al método que me va a traer exclusivamente el rol del usuario logeado, pasandole el identity.
            string rol = JWTConfigurator.TokenObtenerRol(identity);
            //Si el rol del que quiere acceder a este método, no es el auditor, regreso un -1
            if (rol != "Auditor")
            {
                return -1;
            }
            //Buscamos el estado de la auditoria a través de su id
            string estado = await _auditoriaService.ObtenerEstadoAuditoria(idAud.idAuditoria);
            //Preguntamos si la auditoría ya esta en proceso, si es así, retornamos un -1 para indicar un error.
            if(estado=="En Proceso")
            {
                return 0;
            }
            //Si la auditoria no esta en proceso, cambiamos su estado a "En proceso" antes de empezar a contestarla.
            else 
            {
                //Obtenemos la auditoria que vamos a aplicar
                Auditoria audi = await _auditoriaService.ObtenerAuditoria(idAud.idAuditoria);
                //Y la auditoria que tenemos, se la pasamos al método cambiar estado con "En Proceso"
                await _auditoriaService.CambiarEstado("En Proceso", audi);
                //Retornamos un 1 para indicar que la operación se hizo de manera exitosa
                return 1;
            }
        }

        //Endpoint para actualizar el estado a "Aplicada" cuando terminamos de contestar una auditoría.
        [Route("CambiarEstadoAP")]
        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PutAP(IdAuditoria idAud)
        {
            try
            {
                //En este objeto, con este HttpContext, me va a traer TODOS LOS ATRIBUTOS DEL TOKEN, incluidos los claims.
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                //Invoco al método que me va a traer exclusivamente el rol del usuario logeado, pasandole el identity.
                string rol = JWTConfigurator.TokenObtenerRol(identity);
                if (rol == "Auditor")
                {
                    //Buscamos la auditoria por su id
                    Auditoria audi = await _auditoriaService.ObtenerAuditoria(idAud.idAuditoria);
                    //Se lo pasamos al método para que cambie su estado a "Aplicada"
                    await _auditoriaService.CambiarEstado("Aplicada", audi);
                    return Ok(new { message = "Se ha contestado la auditoria con el id " +idAud.idAuditoria});
                }
                else
                {
                    return BadRequest(new { message = "No tiene acceso a esta operación" });
                }
            }
            catch (Exception ex)
            { //Si algo sale mal mandamos un error
                return BadRequest(ex.Message);
                
            }
        }

        //Endpoint para contestar (o asignarles más bien) las preguntas dentro de una auditoria.
        [Route("AplicarAuditoria")]
        [HttpPut]
        //Protección del endpoint
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Put([FromBody] List<ResponderPreguntaDTO> respPreg)
        {

            try
            {

                //En este objeto, con este HttpContext, me va a traer TODOS LOS ATRIBUTOS DEL TOKEN, incluidos los claims.
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                //Invoco al método que me va a traer exclusivamente el rol del usuario logeado, pasandole el identity.
                string rol = JWTConfigurator.TokenObtenerRol(identity);
                //Preguntamos si el usuario tiene el rol de Auditor
                if (rol == "Auditor")
                {
                    //Le mandamos al método ContestarPreguntas nuestro objeto respPreg que tiene la llave primaria
                    //IdPreguntaRespuesta y la llave foranea con la respuesta que le vamos a asignar a la pregunta.
                    await _auditoriaService.ContestarPreguntas(respPreg);
                    //Devolvemos un mensaje confirmando la operación.
                    return Ok(new { message = "La auditoría ha sido aplicada con éxito!" });
                }
                else
                {
                    //Mandamos un error si no tiene acceso al método.
                    return BadRequest(new { message = " No tiene permiso de ejecutar esta operación" });
                }

            }//Mandamos un error si algo sale mal.

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




    }
}
