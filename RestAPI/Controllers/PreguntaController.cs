using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestAPI.Domain.ISerivces;
using RestAPI.Domain.Models;
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
    public class PreguntaController : ControllerBase
    {
        private readonly IPreguntaService _preguntaService;
        public PreguntaController(IPreguntaService preguntaService)
        {
            _preguntaService = preguntaService;
        }

        //Endpoint para cargar preguntas dentro del sistema.
        [HttpPost]
        //Protección del endpoint
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Post([FromBody] Pregunta pregunta)
        {
            try
            {
                //En este objeto, con este HttpContext, me va a traer TODOS LOS ATRIBUTOS DEL TOKEN, incluidos los claims.
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                //Invoco al método que me va a traer exclusivamente el rol del usuario logeado, pasandole el identity.
                string rol = JWTConfigurator.TokenObtenerRol(identity);
                //Preguntamos si el usuario es un RH para seguir con el proceso.
                if (rol == "RH")
                {
                    //Existence va a tener un valor true o false dependiendo si ya existe la pregunta o no.
                    var existence = await _preguntaService.RevisarExistencia(pregunta);
                    //Si es verdadero, enviamos un error con un mensaje de que ya existe la pregunta.
                    if (existence)
                    {
                        return BadRequest(new { message = "La pregunta ya existe dentro de la base" });
                    }
                    //Si no existe aun la pregunta, la guardamos dentro de la base.
                    await _preguntaService.GuardarPregunta(pregunta);
                    //Mandamos un mensaje confirmando la operación.
                    return Ok(new { message = "Pregunta registrada con éxito!" });
                }
                else
                {
                    //Si el usuario no tiene acceso al método mandamos un error
                    return BadRequest(new { message = " No tiene permiso de ejecutar esta operación " });
                }
            }//Enviamos un error por si algo sale mal.
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Endpoint para obtener las preguntas que tenemos dentro de nuestro sistema.
        [HttpGet]
        //Protección del endpoint
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<List<Pregunta>> Get()
        {                
            //En este objeto, con este HttpContext, me va a traer TODOS LOS ATRIBUTOS DEL TOKEN, incluidos los claims.
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            //Invoco al método que me va a traer exclusivamente el rol del usuario logeado, pasandole el identity.
            string rol = JWTConfigurator.TokenObtenerRol(identity);
            //Preguntamos si el usuario es un RH
            if (rol == "RH")
            {
                return await _preguntaService.ObtenerListaPreguntas();
            }
            else
            {
                //Si el usuario no tiene acceso al método retornamos una pregunta erronea.
                List<Pregunta> Error = new List<Pregunta> { new Pregunta { Descripcion = " No tiene permiso de ejecutar esta operación" } };
                return Error;
            }
        }
    }
}
