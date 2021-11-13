using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestAPI.Domain.ISerivces;
using RestAPI.Domain.Models;
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
    public class DepartamentoController : ControllerBase
    {
        private readonly IDepartamentoService _departamentoService;

        public DepartamentoController(IDepartamentoService departamentoService)
        {
            _departamentoService = departamentoService;
        }

        //Endpoint para agregar un objeto Departamento a la base
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Departamento departamento)
        {
            try
            {
                //Revisamos si un departamento ya existe, con el método RevisarExistencia como
                //el dapartamento que queremos introducir a la base como parámetro.
                var existence =await _departamentoService.RevisarExistencia(departamento);
                //Si existence es true, significa que ya existe.
                if (existence)
                {
                    //Le mandamos un mensaje al usuario diciendo que ya existe el departamento.
                    return BadRequest(new { message = "El departamento ya existe dentro de la base!" });
                }
                //Si es false, el proceso continua y agregamos el departamento con el método CrearDepartamento.
                await _departamentoService.CrearDepartamento(departamento);
                //Le mandamos un mensaje confirmando la operación.
                return Ok( new { message = "El departamento ha sido agregado con éxito!!" });
            }//Si algo sale mal, le mandamos un error.
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Endpoint para obtener los departamentos dentro de la base para definir a cual departamento va la auditoria.
        [HttpGet]
        //Protección del endpoint
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<List<Departamento>> Get()
        {
            //En este objeto, con este HttpContext, me va a traer TODOS LOS ATRIBUTOS DEL TOKEN, incluidos los claims.
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            //Invoco al método que me va a traer exclusivamente el rol del usuario logeado, pasandole el identity.
            string rol = JWTConfigurator.TokenObtenerRol(identity);
            //Preguntamos si es un RH para seguir con el proceso
            if (rol == "RH")
            {
                //Creamos una variable para guardar la lista con los departamentos de la base que 
                //obtenemos del método ObtenerListaDepartamentos.
                var departamentos = await _departamentoService.ObtenerListaDepartamentos();
                return departamentos;
            }
            else
            {
                List<Departamento> Error = new List<Departamento> { new Departamento { IdDepartamento = -1, NombreDepartamento = "Error" } };
                return Error;
            }

        }
    }
}
