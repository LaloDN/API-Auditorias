using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestAPI.Domain.ISerivces;
using RestAPI.Domain.Models;
using RestAPI.Presistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
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

        //Endpoint para obtener los departamentos dentro de la base.
        [HttpGet]
        public Task<List<Departamento>> Get()
        {
            //Creamos una variable para guardar la lista con los departamentos de la base que 
            //obtenemos del método ObtenerListaDepartamentos.
            var departamentos = _departamentoService.ObtenerListaDepartamentos();
            return departamentos;
        }
    }
}
